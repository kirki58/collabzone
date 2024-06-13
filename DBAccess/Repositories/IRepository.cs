using collabzone.Models;

namespace collabzone.Repositories;

public interface IRepository<TModel> where TModel : IModel
{
    Task<IEnumerable<TModel>> GetAll();
    Task<TModel> GetById(int id);
    Task Delete(int id);
}
