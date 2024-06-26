using collabzone.DTOS;
using collabzone.Models;

namespace collabzone.Repositories;

public interface ITaskRepository : IRepository<UserTask>
{
    Task Create(CreateTaskDTO dto);
    Task Update(int id, DateTime due_at);
    Task<List<UserTask>> GetAllTasksOfUser(int user_id);
    Task<List<UserTask>> GetAllTasksOfProject(int project_id);
}
