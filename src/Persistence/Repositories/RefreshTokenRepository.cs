using CoursesWebAPI.Core.Contracts.Repositories;
using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebAPI.Persistence.Repositories;

public sealed class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    { }

    public void Create(RefreshToken token) => Add(token);

    public async Task<RefreshToken?> GetByJwtIdAsync(Guid jwtId, bool trackChanges, CancellationToken cancellationToken = default) =>
        await FindByCondition(t => t.JwtId.Equals(jwtId), trackChanges).FirstOrDefaultAsync(cancellationToken);

    public async Task<ICollection<RefreshToken>> GetByUserIdAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken = default) =>
        await FindAll(trackChanges).Where(t => t.UserId.Equals(userId)).ToListAsync(cancellationToken);
    
    public override void Update(RefreshToken token) => base.Update(token);

    public void Delete(Guid id)
    {
        var token = _context.RefreshTokens.Where(t => t.Id.Equals(id)).ExecuteDelete();
        //?? throw new NullReferenceException($"There is no refresh token with id: {id}");
        //_context.Remove(token);
    }

    public async Task<RefreshToken?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default) =>
        await FindByCondition(t => t.Id.Equals(id), trackChanges).FirstOrDefaultAsync(cancellationToken);
}
