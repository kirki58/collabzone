using collabzone.DTOS;
using collabzone.Models;

namespace collabzone.Repositories;

public interface IUsersProjectRepository : IRepository<Users_Project>
{
    Task Create(CreateUsersProjectDTO dto);
}
