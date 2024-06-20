using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace collabzone.Models;

[Table("images")]
public class Image : IModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("User")]
    public int Added_by_user { get; set; }

    public DateTime Added_at { get; set; } = DateTime.Now;
    
    public Guid Guid { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(5)]
    public required string Extension { get; set; }


}
