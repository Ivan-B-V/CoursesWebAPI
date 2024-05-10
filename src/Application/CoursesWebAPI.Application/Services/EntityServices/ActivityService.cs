using CoursesWebAPI.Application.Contracts.Data;
using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Application.Mappers;
using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using FluentResults;
using Serilog;

namespace CoursesWebAPI.Application.Services.EntityServices
{
    public sealed class ActivityService : IActivityService
    {
        private readonly ILogger logger;
        private readonly IRepositoryManager repositoryManager;

        public ActivityService(ILogger logger, IRepositoryManager repositoryManager)
        {
            this.logger = logger;
            this.repositoryManager = repositoryManager;
        }

        public async Task<Result<ActivityFullDto>> CreateAsync(ActivityForCreatingDto activityDto, CancellationToken cancellationToken = default)
        {
            var newActivity = activityDto.ToActivity();
            repositoryManager.ActivityRepository.Create(newActivity);
            var contracts = await repositoryManager.ContractRepository.GetContractsByIdsAsync(activityDto.ContractsIds, trackChanges: true, cancellationToken);
            newActivity.Contracts = contracts.ToHashSet();
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("Activity with id: {id} created.", newActivity.Id);
            //var createdActivity = await repositoryManager.ActivityRepository.GetByIdAsync(newActivity.Id, false, cancellationToken) ?? throw new NullReferenceException("");
            return newActivity.ToActivityDto();
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            repositoryManager.ActivityRepository.Delete(id);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("Activity with id: {id} deleted.", id);
            return Result.Ok();
        }

        public async Task<ActivityFullDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var activity = await repositoryManager.ActivityRepository.GetByIdAsync(id, false, cancellationToken);
            return activity?.ToActivityDto();
        }

        public async Task<PageList<ActivityFullDto>> GetByParametersAsync(ActivityQueryParameters queryParameters, CancellationToken cancellationToken = default)
        {
            var pagelist = await repositoryManager.ActivityRepository.GetByParametersAsync(queryParameters, cancellationToken);
            var pagelistDto = PageList<ActivityFullDto>.ToPageList(pagelist.Items.Select(a => a.ToActivityDto()), pagelist.MetaData);
            return pagelistDto;
        }

        public async Task<IEnumerable<ActivityFullDto>> GetByStudentIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            //var student = await repositoryManager.StudentRepository.GetByIdAsync(id, trackChanges: false, cancellationToken);
            if(await repositoryManager.StudentRepository.GetByIdAsync(id, trackChanges: false, cancellationToken) is not Student student)
            {
                return Enumerable.Empty<ActivityFullDto>();
            }
            var studentContractsIds = student.Contracts.Select(c => c.Id); 
            var acvitivies = await repositoryManager.ActivityRepository
                                  .GetByExpressionAsync(a => a.Contracts.Select(c => c.Id)
                                                                        .Intersect(studentContractsIds)
                                                                        .Any(),
                                   trackChanges: false, cancellationToken);
            return acvitivies.ToActivityDtos();
        }

        public async Task<Result<Guid>> UpdateAsync(Guid id, ActivityForUpdateDto activityDto, CancellationToken cancellationToken = default)
        {
            //var dbActivity = await repositoryManager.ActivityRepository.GetByIdAsync(id, trackChanges: true, cancellationToken);
            if (await repositoryManager.ActivityRepository.GetByIdAsync(id, trackChanges: true, cancellationToken) is not Activity dbActivity)
            {
                logger.Warning("Activity with id: {id} was not found.", id);
                return Result.Fail($"There is no activity with id: {id}");
            }

            var contractsToAdd = await repositoryManager.ContractRepository
                                         .GetContractsByIdsAsync(activityDto.ContractsIds, trackChanges: true, cancellationToken);

            dbActivity.Contracts = contractsToAdd.ToHashSet();
            ActivityMapper.Map(activityDto, dbActivity);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("Activity with id: {id} was updated.", id);
            return dbActivity.Id;
        }
    }
}
