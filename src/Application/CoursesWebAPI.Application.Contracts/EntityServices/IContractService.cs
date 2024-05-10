using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using FluentResults;

namespace CoursesWebAPI.Application.Contracts.EntityServices
{
    public interface IContractService
    {
        public Task<ContractFullDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<PageList<ContractFullDto>> GetByParametersAsync(ContractQueryParameters parameters, CancellationToken cancellationToken = default);
        public Task<Result<ContractFullDto>> CreateAsync(ContractForCreatingDto contractForCreatingDto, 
                                                         CancellationToken cancellationToken = default);
        public Task<Result<ContractFullDto>> UpdateAsync(Guid id, ContractForUpdateDto contractForUpdateDto, 
                                                         CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
