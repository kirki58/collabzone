using System.Runtime.InteropServices;
using collabzone.DTOS;
using collabzone.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace collabzone.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly string _imagesPath;
    private readonly IWebHostEnvironment _env; 
    private readonly IImageRepository _repository;
    public ImagesController(IImageRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
        _imagesPath = Path.Combine(_env.ContentRootPath, "Resources", "images");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] IFormFile file, [FromForm] int id){
        try{
            string ext = Path.GetExtension(file.FileName);
            var dto = new CreateImageDTO(id, ext);
            var img = await _repository.Create(dto);

            var uniqueFileName = img.Guid + ext;
            var filePath = Path.Combine(_imagesPath, uniqueFileName);

            if(GetContentType(filePath) == "application/octet-stream"){
                return BadRequest("Invalid file type");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using(var stream = new FileStream(filePath, FileMode.Create)){
                await file.CopyToAsync(stream);
            }

            return Ok(img);
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromForm] IFormFile file, [FromForm] int id){
        try{
            var oldImg = await _repository.GetById(id);
            if(oldImg == null){
                return NotFound("Image not registered");
            }
        
            var oldFilePath = Path.Combine(_imagesPath, oldImg.Guid + oldImg.Extension);
            if(System.IO.File.Exists(oldFilePath)){
                System.IO.File.Delete(oldFilePath);
            }
            
            var img = await _repository.Update(id, new CreateImageDTO(id, Path.GetExtension(file.FileName)));
            var uniqueFileName = img.Guid + img.Extension;
            var filePath = Path.Combine(_imagesPath, uniqueFileName);

            if(GetContentType(filePath) == "application/octet-stream"){
                return BadRequest("Invalid file type");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using(var stream = new FileStream(filePath, FileMode.Create)){
                await file.CopyToAsync(stream);
            }

            return Ok(img);
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}")] 
    public async Task<IActionResult> GetById(int id){
        // "id" is the id of the user who added the image
        try{
            var image = await _repository.GetById(id);
            if(image == null){
                return NotFound("Image not registered");
            }
            var filePath = Path.Combine(_imagesPath, image.Guid + image.Extension);

            if(!System.IO.File.Exists(filePath)){
                return NotFound("Image not found");
            }
            var imageBytes = System.IO.File.ReadAllBytes(filePath);
            return File(imageBytes, GetContentType(filePath));

        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("does-image-exist/{id}")]
    public async Task<IActionResult> DoesImageExist(int id){
        try{
            var image = await _repository.GetById(id);
            if(image == null){
                return NotFound("Image not registered");
            }
            var filePath = Path.Combine(_imagesPath, image.Guid + image.Extension);
            if(!System.IO.File.Exists(filePath)){
                return NotFound("Image not found");
            }
            return Ok();
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    private string GetContentType(string path)
    {
        var types = new Dictionary<string, string>
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".svg", "image/svg+xml" }
        };

        var ext = Path.GetExtension(path).ToLowerInvariant();
        return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
    }
}
