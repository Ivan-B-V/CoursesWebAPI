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
    public sealed class ContractService : IContractService
    {
        private readonly ILogger logger;
        private readonly IRepositoryManager repositoryManager;
        private readonly IContractRepository contractRepository;

        public ContractService(ILogger logger, IRepositoryManager repositoryManager)
        {
            this.logger = logger;
            this.repositoryManager = repositoryManager;
            contractRepository = repositoryManager.ContractRepository;
        }

        public async Task<Result<ContractFullDto>> CreateAsync(ContractForCreatingDto contractForCreatingDto, CancellationToken cancellationToken = default)
        {
            if (await contractRepository.IsContractExists(contractForCreatingDto.Number, cancellationToken))
            {
                return Result.Fail($"Contract with number {contractForCreatingDto.Number} is already exists.");
            }
            var newContract = contractForCreatingDto.ToContact();
            contractRepository.Create(newContract);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("New contractwith id: {id} created.", newContract.Id);
            return newContract.ToContactFullDto();
        }

        public async Task<ContractFullDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var contract = await contractRepository.GetContractByIdAsync(id, trackChanges: false, cancellationToken);
            return contract?.ToContactFullDto();
        }

        public async Task<PageList<ContractFullDto>> GetByParametersAsync(ContractQueryParameters parameters, CancellationToken cancellationToken = default)
        {
            var pageList = await contractRepository.GetContractsByParametersAsync(parameters, cancellationToken);
            return PageList<ContractFullDto>.ToPageList(pageList.Items.Select(c => c.ToContactFullDto()), pageList.MetaData);
        }

        public async Task<Result<ContractFullDto>> UpdateAsync(Guid id, ContractForUpdateDto contractForUpdateDto, CancellationToken cancellationToken = default)
        {
            var contractForUpdate = await contractRepository.GetContractByIdAsync(id, trackChanges: true, cancellationToken);
            if (contractForUpdate is null)
            {
                return Result.Fail($"Contract with id {id} doesn't exists.");
            }
            ContractMapper.Map(contractForUpdateDto, contractForUpdate);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("Cantract {id} updated.", id);
            return contractForUpdate.ToContactFullDto();
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            contractRepository.Delete(id);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("Cantract {id} deleted.", id);
            return true;
        }
    }
}
