using CoursesWebAPI.Core.Contracts.Repositories;

namespace CoursesWebAPI.Application.Contracts.Data
{
    public interface IRepositoryManager : IDisposable
    {
        public IStudentRepository StudentRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public IActivityTypeRepository ActivityTypeRepository { get; }
        public IActivityRepository ActivityRepository { get; }
        public IContractRepository ContractRepository { get; }
        public Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
