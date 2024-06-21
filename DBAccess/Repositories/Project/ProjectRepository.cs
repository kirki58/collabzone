using collabzone.DBAccess.Context;
using collabzone.DBAccess.Repositories;
using collabzone.Models;
using Microsoft.EntityFrameworkCore;

namespace collabzone.Repositories;

public class ProjectRepository : BaseRepository<CZContext>, IProjectRepository
{
    public ProjectRepository(CZContext context) : base(context)
    {
    }
    public async Task<Project> Create(string name){
        try{
            var project = new Project{
                Name = name
            };
            await _context.Projects.AddAsync(project);
            int rowsAffected =  await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to create project");
            }
            return project;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<Project> GetByGuid(Guid guid){
        try{
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Invite_guid == guid);
            if(project == null){
                throw new Exception("Project not found");
            }
            return project;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Project>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Project> GetById(int id)
    {
        try{
            var project = await _context.FindAsync<Project>(id);
            if(project == null){
                throw new Exception("Project not found");
            }
            return project;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Project>> GetProjectsById(List<int> projectIds)
    {
        try{
            var projects = await _context.Projects.Where(p => projectIds.Contains(p.Id)).ToListAsync();
            return projects;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<Project> Update(int id, UpdateProjectDTO dto)
    {
        try{
            var project = await _context.Projects.FindAsync(id);
            if(project == null){
                throw new Exception("Project not found");
            }
            project.Name = dto.Name ?? project.Name;
            
            if(dto.Invite_guid){
                project.Invite_guid = Guid.NewGuid();
            }

            _context.Projects.Update(project);
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to update project");
            }
            return project;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
}
