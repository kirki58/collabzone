using collabzone.DBAccess.Context;
using collabzone.DTOS;
using collabzone.Models;
using Microsoft.EntityFrameworkCore;

namespace collabzone.DBAccess.Repositories;

public class UserRepository : BaseRepository<CZContext>, IUserRepository
{
    public UserRepository(CZContext context) : base(context)
    {
    }

    public async Task Delete(int id)
    {
        try{
            var user = await _context.Users.FindAsync(id);
            if(user == null){
                throw new Exception("User not found");
            }
            _context.Users.Remove(user);

            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to delete user");
            }
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public Task<IEnumerable<User>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetById(int id)
    {
        try{
            var user = await _context.Users.FindAsync(id);
            if(user == null){
                throw new Exception("User not found");
            }
            return user;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task Create(CreateUserDTO dto){
        try{
            var user = new User{
                Name = dto.Name,
                Email = dto.Email,
                Password_hash = dto.Password_hash
            };
            await _context.Users.AddAsync(user);
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to create user");
            }
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task Update(int id, UpdateUserDTO dto)
    {
        try{
            var user = await _context.Users.FindAsync(id);
            if(user == null){
                throw new Exception("User not found");
            }
            user.Name = dto.Name ?? user.Name;
            user.Email = dto.Email ?? user.Email;
            user.Password_hash = dto.Password_hash ?? user.Password_hash;

            _context.Users.Update(user);
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to update user");
            }
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<User?> GetByEmail(string email)
    {
        try{
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<User?> GetByName(string name)
    {
        try{
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == name);
            return user;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<User>> GetUsersById(List<int> userIds)
    {
        try{
            var users = await _context.Users.Where(x => userIds.Contains(x.Id)).ToListAsync();
            return users;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
}
