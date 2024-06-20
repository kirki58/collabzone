using collabzone.DBAccess.Context;
using collabzone.DBAccess.Repositories;
using collabzone.DTOS;
using collabzone.Models;

namespace collabzone.Repositories;

public class UsersProjectRepository : BaseRepository<CZContext>, IUsersProjectRepository
{
    public UsersProjectRepository(CZContext context) : base(context)
    {
    }

    public async Task Create(CreateUsersProjectDTO dto)
    {
        try{
            var userProject = new Users_Project
            {
                User_id = dto.User_id,
                Project_id = dto.Project_id,
                Is_Admin = dto.Is_Admin
            };
            await _context.Users_Projects.AddAsync(userProject);
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to create user project");
            }
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Users_Project>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Users_Project> GetById(int id)
    {
        throw new NotImplementedException();
    }
}
