using collabzone.DBAccess.Context;
using collabzone.DBAccess.Repositories;
using collabzone.DTOS;
using collabzone.Repositories;
using Microsoft.EntityFrameworkCore;

namespace collabzone.Models;

public class ImageRepository : BaseRepository<CZContext>, IImageRepository
{
    public ImageRepository(CZContext context) : base(context)
    {
    }

    public async Task<Image> Create(CreateImageDTO dto){
        try{
            var img = new Image{
                Added_by_user = dto.Added_by_user,
                Extension = dto.Extension
            };
            await _context.Images.AddAsync(img);
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to create image");
            }
            return img;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    //CreateImageDTO and UpdateImageDTO are the same so no need for UpdateImageDTO.
    // "id" is the id of the user who added the image
    public async Task<Image> Update(int id, CreateImageDTO dto){
        try{
            var img = await _context.Images.FirstOrDefaultAsync(x => x.Added_by_user == id);
            if(img == null){
                throw new Exception("Image not found");
            }
            img.Added_by_user = dto.Added_by_user;
            img.Extension = dto.Extension;
            img.Added_at = DateTime.Now;

            _context.Images.Update(img);
            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to update image");
            }
            return img;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task Delete(int id)
    {
        try{
            var img = await _context.Images.FindAsync(id);
            if(img == null){
                throw new Exception("Image not found");
            }
            _context.Images.Remove(img);

            int rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected == 0){
                throw new Exception("Failed to delete image");
            }
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public Task<IEnumerable<Image>> GetAll()
    {
        throw new NotImplementedException();
    }


    // "id" is the id of the user who added the image
    public async Task<Image> GetById(int id)
    {
        try{
            var img = await _context.Images.FirstOrDefaultAsync(x => x.Added_by_user == id);
            if(img == null){
                throw new Exception("Image not found");
            }
            return img;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
}
