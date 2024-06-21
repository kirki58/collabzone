using collabzone.Models;

namespace collabzone.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project> Create(string name);
    Task<Project> GetByGuid(Guid guid);
    Task<List<Project>> GetProjectsById(List<int> projectIds);
    Task<Project> Update(int id, UpdateProjectDTO dto);
}
