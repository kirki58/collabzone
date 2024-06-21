using collabzone.DTOS;
using collabzone.Models;

namespace collabzone.Repositories;

public interface IUsersProjectRepository : IRepository<Users_Project>
{
    Task Create(CreateUsersProjectDTO dto);
    Task<List<int>> GetProjectIDs(int user_id);
    Task<bool> is_admin(int user_id, int project_id);
    Task<List<int>> GetAdmins(int project_id);
    Task<List<int>> GetCollabs(int project_id);
}
