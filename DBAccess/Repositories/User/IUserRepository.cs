using collabzone.DTOS;
using collabzone.Models;
using collabzone.Repositories;

namespace collabzone.DBAccess.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task Create(CreateUserDTO dto);
    Task Update(int id,  UpdateUserDTO dto);
}
