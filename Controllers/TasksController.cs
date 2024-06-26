using collabzone.DTOS;
using collabzone.Repositories;
using collabzone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace collabzone.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUsersProjectRepository _userProjectRepository;

    public TasksController(ITaskRepository taskRepository, IUsersProjectRepository userProjectRepository)
    {
        _taskRepository = taskRepository;
        _userProjectRepository = userProjectRepository;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDTO dto)
    {
        try{
            var claims = AuthService.DecodeToken(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            var sub = claims.FirstOrDefault(c => c.Type == "sub").Value;

            if(dto.given_by != int.Parse(sub)){
                return Unauthorized();
            }
            if(!await _userProjectRepository.is_admin(int.Parse(sub), dto.given_at)){
                return Unauthorized();
            }
            await _taskRepository.Create(dto);
            return Ok();
        }
        catch(Exception e){
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDTO dto)
    {
        try{
            var claims = AuthService.DecodeToken(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            var sub = claims.FirstOrDefault(c => c.Type == "sub").Value;

            if(!await _userProjectRepository.is_admin(int.Parse(sub), dto.project_id)){
                return Unauthorized();
            }

            await _taskRepository.Update(id, dto.due_at);
            return Ok();
        }
        catch(Exception e){
            return BadRequest(e.Message);
        
        }
    }

    // [Authorize]
    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetById(int id)
    // {
    //     try{
    //         var task = await _taskRepository.GetById(id);
    //         return Ok(task);
    //     }
    //     catch(Exception e){
    //         return BadRequest(e.Message);
    //     }
    // }

    [Authorize]
    [HttpGet("user/{user_id}")]
    public async Task<IActionResult> GetAllTasksOfUser(int user_id)
    {
        try{
            var tasks = await _taskRepository.GetAllTasksOfUser(user_id);
            return Ok(tasks);
        }
        catch(Exception e){
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpGet("project/{project_id}")]
    public async Task<IActionResult> GetAllTasksOfProject(int project_id)
    {
        try{
            var tasks = await _taskRepository.GetAllTasksOfProject(project_id);
            return Ok(tasks);
        }
        catch(Exception e){
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, [FromBody] DeleteTaskDTO dto)
    {
        try{
            var claims = AuthService.DecodeToken(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            var sub = claims.FirstOrDefault(c => c.Type == "sub").Value;
            if(!await _userProjectRepository.is_admin(int.Parse(sub), dto.project_id)){
                return Unauthorized();
            }

            await _taskRepository.Delete(id);
            return Ok();
        }
        catch(Exception e){
            return BadRequest(e.Message);
        }
    }
}

public record UpdateTaskDTO(DateTime due_at, int project_id);
public record DeleteTaskDTO(int project_id);
