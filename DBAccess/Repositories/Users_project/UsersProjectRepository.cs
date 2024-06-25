using collabzone.DBAccess.Context;
using collabzone.DBAccess.Repositories;
using collabzone.DTOS;
using collabzone.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> is_admin(int user_id, int project_id)
    {
        try{
            var userProject = await _context.Users_Projects
                .FirstOrDefaultAsync(up => up.User_id == user_id && up.Project_id == project_id);
            if(userProject == null){
                throw new Exception("User project not found");
            }
            return userProject.Is_Admin;
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<int>> GetProjectIDs(int user_id)
    {
        try{
            var projectIDs = await _context.Users_Projects
                .Where(up => up.User_id == user_id)
                .Select(up => up.Project_id)
                .ToListAsync();
            return projectIDs;
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<int>> GetAdmins(int project_id)
    {
        try{
            var admins = await _context.Users_Projects
                .Where(up => up.Project_id == project_id && up.Is_Admin == true)
                .Select(up => up.User_id)
                .ToListAsync();
            
            return admins;
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<int>> GetCollabs(int project_id)
    {
        try{
            var collabs = await _context.Users_Projects
                .Where(up => up.Project_id == project_id && up.Is_Admin == false)
                .Select(up => up.User_id)
                .ToListAsync();
            
            return collabs;
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task BanUser(int user_id, int project_id)
    {
        try{
            var userProject = await _context.Users_Projects
                .FirstOrDefaultAsync(up => up.User_id == user_id && up.Project_id == project_id);
            if(userProject == null){
                throw new Exception("User project not found");
            }
            _context.Users_Projects.Remove(userProject);
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to ban user");
            }
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task ChangeAdmin(int user_id, int project_id)
    {
        try{
            var userProject = await _context.Users_Projects
                .FirstOrDefaultAsync(up => up.User_id == user_id && up.Project_id == project_id);
            if(userProject == null){
                throw new Exception("User project not found");
            }
            userProject.Is_Admin = !userProject.Is_Admin;
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to change admin status");
            }
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> is_in_project(int user_id, int project_id)
    {
        try{
            var userProject = await _context.Users_Projects
                .FirstOrDefaultAsync(up => up.User_id == user_id && up.Project_id == project_id);
            if(userProject == null){
                return false;
            }
            return true;
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }
}
