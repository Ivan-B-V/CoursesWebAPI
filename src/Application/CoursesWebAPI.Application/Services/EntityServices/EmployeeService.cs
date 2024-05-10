using CoursesWebAPI.Application.Contracts.Data;
using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Application.Mappers;
using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Contracts.Repositories;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using FluentResults;
using Serilog;

namespace CoursesWebAPI.Application.Services.EntityServices
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly ILogger logger;
        private readonly IRepositoryManager repositoryManager;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeService(ILogger logger, IRepositoryManager repositoryManager)
        {
            this.logger = logger;
            this.repositoryManager = repositoryManager;
            employeeRepository = repositoryManager.EmployeeRepository;
        }
        public async Task<Result<EmployeeDto>> CreateAsync(EmployeeForCreatingDto employeeDto, CancellationToken cancellationToken = default)
        {
            var newEmployee = employeeDto.ToEmployee();
            employeeRepository.Create(newEmployee);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("New employee: {Firstname} {Lastname} {Patronomic} was created.", 
                newEmployee.FirsName, newEmployee.LastName, newEmployee.Patronomic);
            return newEmployee.ToEmployeeDto();
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var employee = await employeeRepository.GetByIdAsync(id, trackChanges: false, cancellationToken);
            return employee?.ToEmployeeDto();
        }

        public Task<IEnumerable<EmployeeDto>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<PageList<EmployeeDto>> GetEmployeesAsync(PersonQueryParameters queryParameters, CancellationToken cancellationToken = default)
        {
            var pageList = await employeeRepository.GetByParametersAsync(queryParameters, cancellationToken);
            var employeeDtos = pageList.Select(e => e.ToEmployeeDto());
            return PageList<EmployeeDto>.ToPageList(employeeDtos, pageList.MetaData);
        }

        public async Task<Result<EmployeeDto>> UpdateAsync(Guid id, EmployeeForUpdateDto employeeDto, CancellationToken cancellationToken = default)
        {
            var dbEmployee = await employeeRepository.GetByIdAsync(id, trackChanges: true, cancellationToken);
            if (dbEmployee is null)
            {
                return Result.Fail($"Employee with id: {id} does not exists.");
            }
            EmployeeMapper.Map(employeeDto, dbEmployee);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("Employee {id} updated.", id);
            return dbEmployee.ToEmployeeDto();
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            employeeRepository.Delete(id);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("Employee {id} deleted.", id);
            return true;
        }
    }
}
