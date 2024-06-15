using collabzone.DBAccess.Context;
using collabzone.DBAccess.Repositories;
using collabzone.Models;
using Microsoft.EntityFrameworkCore;

namespace collabzone.Repositories;

public class VerificationTokenRepository : BaseRepository<CZContext>, IVerificationTokenRepository
{
    public VerificationTokenRepository(CZContext context) : base(context)
    {
    }

    public async Task Create(Guid token)
    {
        try
        {
            var ver_token = new Verification_token { Token = token };
            await _context.Verification_tokens.AddAsync(ver_token);
            int rowsAffected = await _context.SaveChangesAsync();
            if (rowsAffected == 0)
            {
                throw new Exception("Failed to create verification token");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<Verification_token?> GetByToken(Guid token)
    {
        try
        {
            var verificationToken = await _context.Verification_tokens.FirstOrDefaultAsync(x => x.Token == token);
            return verificationToken;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Verification_token>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Verification_token> GetById(int id)
    {
        throw new NotImplementedException();
    }
}
