﻿using collabzone.DBAccess.Repositories;
using collabzone.DTOS;
using collabzone.Models;
using collabzone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace collabzone.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly AuthService _authService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IVerificationTokenRepository _verificationRepository;
    private readonly EmailService _emailService;
    public UsersController(IUserRepository repository, AuthService authService, IPasswordHasher<User> passwordHasher, IVerificationTokenRepository verificationRepository, EmailService emailService)
    {
        _repository = repository;
        _authService = authService;
        _passwordHasher = passwordHasher;
        _verificationRepository = verificationRepository;
        _emailService = emailService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] IFormCollection form){
        try{
            if(string.IsNullOrEmpty(form["registerEmail"]) || string.IsNullOrEmpty(form["registerPassword"]) ||
               string.IsNullOrEmpty(form["validatePassword"]) || string.IsNullOrEmpty(form["registerName"])){
                return BadRequest("Fields are required");
            }
            if(form["registerPassword"] != form["validatePassword"]){
                return BadRequest("Passwords do not match");
            }
            // Hash password
            string hashedPassword = _passwordHasher.HashPassword(null, form["registerPassword"]);

            //create token
            Guid gd = Guid.NewGuid();
            await _verificationRepository.Create(gd);

            //Send gd as email
            await _emailService.SendValidationMail(form["registerEmail"], gd);
            
            //Continue in ValidateEmail
            Response.Cookies.Append("email", form["registerEmail"], new CookieOptions { Expires = DateTime.Now.AddMinutes(8) });
            Response.Cookies.Append("password", hashedPassword, new CookieOptions { Expires = DateTime.Now.AddMinutes(8) });
            Response.Cookies.Append("name", form["registerName"], new CookieOptions { Expires = DateTime.Now.AddMinutes(8) });
            
            return Ok();
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("validate-email/{token}")]
    public async Task<IActionResult> ValidateEmail(Guid token){
        try{
            var verificationToken = await _verificationRepository.GetByToken(token);
            if(verificationToken == null){
                return NotFound("Token not found");
            }
            if(verificationToken.Expiry_date < DateTime.Now){
                return BadRequest("Token expired");
            }
            var name = Request.Cookies["name"];
            var email = Request.Cookies["email"];
            var password = Request.Cookies["password"];
            CreateUserDTO dto = new CreateUserDTO(name, email, password);

            await _repository.Create(dto);
            
            return Ok("Your email is verified, You can close this page and login"); 
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _repository.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id){
        try{
            var user = await _repository.GetById(id);
            return Ok(user);
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserDTO dto){
        try{
            await _repository.Update(id, dto);
            return Ok();
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] IFormCollection form){
        try{
            if(string.IsNullOrEmpty(form["loginEmail"]) || string.IsNullOrEmpty(form["loginPassword"])){
                return BadRequest("Email and password are required");
            }
            string passwordSubmitted = form["loginPassword"];
            string emailSubmitted = form["loginEmail"];

            var user = await _repository.GetByEmail(emailSubmitted);

            if(user == null){
                return NotFound("User not found");
            }

            if(_passwordHasher.VerifyHashedPassword(user, user.Password_hash, passwordSubmitted) == PasswordVerificationResult.Failed){
                return Unauthorized("Invalid password");   
            }

            var token = _authService.GenerateToken(user);
            return Ok(token);
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }
}
