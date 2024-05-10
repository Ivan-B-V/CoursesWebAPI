using CoursesWebAPI.Application.Contracts.Data;
using CoursesWebAPI.Core.Contracts.Repositories;

namespace CoursesWebAPI.Persistence;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationDbContext _context;

    public RepositoryManager(ApplicationDbContext context,
        IStudentRepository studentRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IActivityTypeRepository activityTypeRepository,
        IActivityRepository activityRepository,
        IContractRepository contractRepository,
        IEmployeeRepository employeeRepository)
    {
        _context = context;
        StudentRepository = studentRepository;
        RefreshTokenRepository = refreshTokenRepository;
        ActivityTypeRepository = activityTypeRepository;
        ActivityRepository = activityRepository;
        ContractRepository = contractRepository;
        EmployeeRepository = employeeRepository;
    }

    public IStudentRepository StudentRepository { get; }

    public IRefreshTokenRepository RefreshTokenRepository { get; }

    public IActivityTypeRepository ActivityTypeRepository { get; }

    public IActivityRepository ActivityRepository { get; }

    public IContractRepository ContractRepository { get; }

    public IEmployeeRepository EmployeeRepository { get; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
