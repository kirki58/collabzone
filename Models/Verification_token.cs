using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace collabzone.Models;

public class Verification_token : IModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public Guid Token { get; set; }

    public DateTime Expiry_date { get; set; } = DateTime.Now.AddMinutes(5);
}
