using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace collabzone.Models;

[Table("tasks")]
public class UserTask : IModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("User")]
    public int Given_by { get; set; }

    [Required]
    [ForeignKey("Project")]
    public int Given_at { get; set; }

    [Required]
    public DateTime Due_at { get; set; }

    [Required]
    public required string Header { get; set; }

    [Required]
    public required string Description { get; set; }
}
