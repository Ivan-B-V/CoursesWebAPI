using CoursesWebAPI.Core.Identity;

namespace CoursesWebAPI.Core.Contracts.Repositories
{
    public interface IRefreshTokenRepository
    {
        public void Create(RefreshToken token);
        public Task<ICollection<RefreshToken>> GetByUserIdAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken = default);
        public Task<RefreshToken?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);
        public Task<RefreshToken?> GetByJwtIdAsync(Guid jwtId, bool trackChanges, CancellationToken cancellationToken = default);
        public void Update(RefreshToken token);
        public void Delete(Guid id);
    }
}
