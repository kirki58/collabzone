using collabzone.DTOS;
using collabzone.Models;
using collabzone.Repositories;

namespace collabzone.DBAccess.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task Create(CreateUserDTO dto);
    Task Update(int id,  UpdateUserDTO dto);

    Task<User?> GetByEmail(string email);
    Task<User?> GetByName(string name);
    Task<List<User>> GetUsersById(List<int> userIds);
}
