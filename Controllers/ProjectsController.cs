using collabzone.DBAccess.Repositories;
using collabzone.DTOS;
using collabzone.Models;
using collabzone.Repositories;
using collabzone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace collabzone.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUsersProjectRepository _usersProjectRepository;
    private readonly IUserRepository _userRepository;

    public ProjectsController(IProjectRepository projectRepository, IUsersProjectRepository upRepository, IUserRepository userRepository)
    {
        _projectRepository = projectRepository;
        _usersProjectRepository = upRepository;
        _userRepository = userRepository;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] string name, [FromForm] int user_id)
    {
        try
        {
            var project = await _projectRepository.Create(name);
            var userProject = new CreateUsersProjectDTO(user_id, project.Id, true);
            await _usersProjectRepository.Create(userProject);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost]
    [Route("join")]
    [Authorize]
    public async Task<IActionResult> JoinProject([FromForm] int user_id, [FromForm] Guid guid)
    {
        try
        {
            var project = await _projectRepository.GetByGuid(guid);
            var userProject = new CreateUsersProjectDTO(user_id, project.Id, false);

            await _usersProjectRepository.Create(userProject);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("user/{user_id}")]
    public async Task<IActionResult> GetProjects(int user_id)
    {
        try
        {
            var claims = AuthService.DecodeToken(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            var sub = claims.FirstOrDefault(c => c.Type == "sub").Value;
            if (sub != user_id.ToString())
            {
                return Unauthorized();
            }

            var projectIDs = await _usersProjectRepository.GetProjectIDs(user_id);
            if(projectIDs.Count == 0){
                return NotFound();
            }
            var projects = new List<Project>();
            projects = await _projectRepository.GetProjectsById(projectIDs);
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{guid}")]
    public async Task<IActionResult> GetProject(Guid guid)
    {
        try
        {
            var project = await _projectRepository.GetByGuid(guid);
            return Ok(project);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{guid}")]
    public async Task<IActionResult> Update([FromForm] Guid guid,[FromForm] string? name ,[FromForm] bool refresh_guid)
    {
        try
        {
            var claims = AuthService.DecodeToken(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            var sub = claims.FirstOrDefault(c => c.Type == "sub").Value;

            var project = await _projectRepository.GetByGuid(guid);
            if (!await _usersProjectRepository.is_admin(int.Parse(sub), project.Id))
            {
                return Unauthorized();
            }

            UpdateProjectDTO dto = new UpdateProjectDTO
            {
                Name = name,
                Invite_guid = refresh_guid
            };
            await _projectRepository.Update(project.Id, dto);
            return Ok(new {
                guid = project.Invite_guid
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("admins/{project_guid}")]
    public async Task<IActionResult> GetAdmins(Guid project_guid)
    {
        try
        {
            var project = await _projectRepository.GetByGuid(project_guid);
            var admins_ids = await _usersProjectRepository.GetAdmins(project.Id);
            var admins = await _userRepository.GetUsersById(admins_ids);
            if(admins.Count == 0){
                return NotFound();
            }
            return Ok(admins);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [Authorize]
    [HttpGet("collabs/{project_guid}")]
    public async Task<IActionResult> GetCollabs(Guid project_guid)
    {
        try
        {
            var project = await _projectRepository.GetByGuid(project_guid);
            var collabs_ids = await _usersProjectRepository.GetCollabs(project.Id);
            var collabs = await _userRepository.GetUsersById(collabs_ids);
            if(collabs.Count == 0){
                return NotFound();
            }
            return Ok(collabs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
