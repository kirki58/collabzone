using collabzone.DBAccess.Repositories;
using collabzone.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace collabzone.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;
    public UsersController(IUserRepository repository)
    {
        _repository = repository;
    }
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
}
