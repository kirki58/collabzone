using collabzone.Repositories;

namespace collabzone.Models;

public interface IVerificationTokenRepository : IRepository<Verification_token>
{
    Task Create (Guid token);
    Task<Verification_token?> GetByToken(Guid token);
}
