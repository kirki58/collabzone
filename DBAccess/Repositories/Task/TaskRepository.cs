using collabzone.DBAccess.Context;
using collabzone.DBAccess.Repositories;
using collabzone.DTOS;
using collabzone.Models;
using Microsoft.EntityFrameworkCore;

namespace collabzone.Repositories;

public class TaskRepository : BaseRepository<CZContext>, ITaskRepository
{
    public TaskRepository(CZContext context) : base(context)
    {
    }

    public async Task Create(CreateTaskDTO dto)
    {
        try{
            var task = new UserTask
            {
                Given_by = dto.given_by,
                Given_at = dto.given_at,
                Due_at = dto.due_at,
                Header = dto.header,
                Description = dto.description,
                Given_to = dto.given_to
            };
            await _context.Tasks.AddAsync(task);
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to create task");
            }
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task Delete(int id)
    {
        try{
            var task = await _context.Tasks.FindAsync(id);
            if(task == null){
                throw new Exception("Task not found");
            }
            _context.Tasks.Remove(task);
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to delete task");
            }
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public Task<IEnumerable<UserTask>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserTask>> GetAllTasksOfProject(int project_id)
    {
        try{
            var tasks = await _context.Tasks.Where(t => t.Given_at == project_id).ToListAsync();
            if(tasks == null){
                throw new Exception("No tasks found");
            }
            return tasks;
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<UserTask>> GetAllTasksOfUser(int user_id)
    {
        try{
            var tasks = await _context.Tasks.Where(t => t.Given_to == user_id).ToListAsync();
            if(tasks == null){
                throw new Exception("No tasks found");
            }
            return tasks;
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<UserTask> GetById(int id)
    {
        try{
            var task = await _context.Tasks.FindAsync(id);
            if(task == null){
                throw new Exception("Task not found");
            }
            return task;
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    

    public async Task Update(int id, DateTime due_at)
    {
        try{
            var task = await _context.Tasks.FindAsync(id);
            if(task == null){
                throw new Exception("Task not found");
            }
            task.Due_at = due_at;
            _context.Tasks.Update(task);
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to update task");
            }
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }
}
