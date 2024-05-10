using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.RequestFeatures;

namespace CoursesWebAPI.Core.Contracts.Repositories
{
    public interface IContractRepository
    {
        public void Create(Contract contract);

        public Task<PageList<Contract>> GetStudentContractsAsync(Guid studentId, ContractQueryParameters parameters, CancellationToken cancellationToken = default);
        
        public Task<Contract?> GetContractByNumberAsync(string number, bool trackChanges, CancellationToken cancellationToken = default);
        public Task<bool> IsContractExists(string number, CancellationToken cancellationToken = default);

        public Task<PageList<Contract>> GetContractsByParametersAsync(ContractQueryParameters parameters, CancellationToken cancellationToken = default);
        
        public Task<IEnumerable<Contract>> GetContractsByIdsAsync(IEnumerable<Guid> ids, bool trackChanges, CancellationToken cancellationToken = default);

        public Task<Contract?> GetContractByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);

        public void Update(Contract contract);

        public void Delete(Guid id);
    }
}
