using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace collabzone.Models;

[Table("users_projects")]
public class Users_Project : IModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("User")]
    public int User_id { get; set; }

    [Required]
    [ForeignKey("Project")]
    public int Project_id { get; set; }

    [Required]
    public bool Is_Admin { get; set; }

    public DateTime Joined_At { get; set; } = DateTime.Now;
}
