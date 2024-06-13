using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace collabzone.Models;

[Table("projects")]
public class Project : IModel
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [ForeignKey("Image")]
    public int? Pic_id { get; set; }

    public Guid Invite_guid{ get; set; } = Guid.NewGuid();

    public DateTime Started_at { get; set; } = DateTime.Now;
}
