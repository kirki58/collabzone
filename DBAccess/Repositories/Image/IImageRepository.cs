using collabzone.DTOS;
using collabzone.Models;

namespace collabzone.Repositories;

public interface IImageRepository : IRepository<Image>
{
    public Task<Image> Create(CreateImageDTO dto);
    public Task<Image> Update(int id, CreateImageDTO dto);
}
