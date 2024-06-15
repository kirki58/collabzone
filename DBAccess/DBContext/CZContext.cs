using collabzone.Models;
using Microsoft.EntityFrameworkCore;

namespace collabzone.DBAccess.Context;

public class CZContext : DbContext
{
    public CZContext(DbContextOptions<CZContext> options) : base(options)
    {   
    }

    //Tables in the database
    public DbSet<Image> Images { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Users_Project> Users_Projects { get; set; }
    public DbSet<Verification_token> Verification_tokens { get; set; }
}
