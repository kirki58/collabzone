using Microsoft.EntityFrameworkCore;

namespace collabzone.DBAccess.Repositories;

public class BaseRepository<TContext> where TContext : DbContext
{
    protected readonly TContext _context;

    public BaseRepository(TContext context)
    {
        _context = context;
    }
}
