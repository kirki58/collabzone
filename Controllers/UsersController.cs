using collabzone.DBAccess.Repositories;
using collabzone.DTOS;
using collabzone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace collabzone.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly AuthService _authService;
    public UsersController(IUserRepository repository, AuthService authService)
    {
        _repository = repository;
        _authService = authService;
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

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDTO dto){
        try{
            await _repository.Create(dto);
            return Ok();
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
    [HttpGet("GetToken/{id}")]
    public async Task<IActionResult> GetJwt(int id){
        try{
            var user = await _repository.GetById(id);
            var token = _authService.GenerateToken(user);
            return Ok(token);
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }
}
