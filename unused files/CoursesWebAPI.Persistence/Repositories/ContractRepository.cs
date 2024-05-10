using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Contracts.Repositories;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Persistence.Extensions;
using CoursesWebAPI.Persistence.Extensions.RepositoryExtensions;
using CoursesWebAPI.Persistence.Repositories.Base;
using CoursesWebAPI.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebAPI.Persistence.Repositories
{
    public sealed class ContractRepository : RepositoryBase<Contract>, IContractRepository
    {
        public ContractRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void Create(Contract contract) => Add(contract);


        public async Task<Contract?> GetContractByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default) =>
            await FindByCondition(c => c.Id.Equals(id), trackChanges)
                  .TagWith($"Contract with id: {id}")
                  .FirstOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<Contract>> GetContractsByIdsAsync(IEnumerable<Guid> ids, bool trackChanges, CancellationToken cancellationToken = default)
        {
            if (ids is null || !ids.Any())
            {
                return Enumerable.Empty<Contract>();
            }
            var contracts = await FindByCondition(c => ids.Contains(c.Id), trackChanges).ToArrayAsync(cancellationToken);
            return contracts;
        }

        public async Task<PageList<Contract>> GetContractsByParametersAsync(ContractQueryParameters parameters, CancellationToken cancellationToken = default)
        {
            var query = GetQueryByParameters(parameters, false);
            return await PageList<Contract>.ToPageListAsync(query, parameters.PageNumber, parameters.PageSize, cancellationToken);
        }

        public async Task<PageList<Contract>> GetStudentContractsAsync(Guid studentId, ContractQueryParameters parameters, CancellationToken cancellationToken = default)
        {
            var query = FindByCondition(c => c.StudentId.Equals(studentId), false)
                        .TagWith($"Contracts for studend id: {studentId}");
            return await PageList<Contract>.ToPageListAsync(query, parameters.PageNumber, parameters.PageSize, cancellationToken);
        }

        private IQueryable<Contract> GetQueryByParameters(ContractQueryParameters parameters, bool trackChanges) =>
            FindAll(trackChanges)
            .Include(c => c.Student)
            .AsSplitQuery()
            .Search(parameters.SearchTerm)
            .Sort(parameters.OrderBy)
            .TagWith($"Contracts with searchTerm: {parameters.SearchTerm}, ordered by: {parameters.OrderBy}");

        public override void Update(Contract contract) => base.Update(contract);
        
        public void Delete(Guid id) => Delete(new Contract {Id = id});

        public async Task<Contract?> GetContractByNumberAsync(string number, bool trackChanges, CancellationToken cancellationToken = default)
        {
            var contract = await FindByCondition(c => c.Number.Equals(number), trackChanges).SingleOrDefaultAsync(cancellationToken);
            return contract;
        }

        public async Task<bool> IsContractExists(string number, CancellationToken cancellationToken = default) =>
            await FindByCondition(c => c.Number.Equals(number), false).AnyAsync(cancellationToken);
    }
}
