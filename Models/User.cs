using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace collabzone.Models;

[Table("users")]
public class User : IModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MaxLength(256)]
    public required string Password_hash { get; set; }

    [ForeignKey("Image")]
    public int? Pp_img_id { get; set; }

    public DateTime Created_at { get; set; } = DateTime.Now;
}
