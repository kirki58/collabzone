using collabzone.DTOS;
using collabzone.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace collabzone.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUsersProjectRepository _usersProjectRepository;

    public ProjectsController(IProjectRepository projectRepository, IUsersProjectRepository upRepository)
    {
        _projectRepository = projectRepository;
        _usersProjectRepository = upRepository;
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

    [HttpGet("{guid}")]
    [Authorize]
    public async Task<IActionResult> GetByGuid(Guid guid)
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
}
