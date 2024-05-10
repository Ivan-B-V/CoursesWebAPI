using CoursesWebAPI.Application.Contracts.Data;
using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Application.Mappers;
using CoursesWebAPI.Core.Contracts.Repositories;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using FluentResults;
using Serilog;

namespace CoursesWebAPI.Application.Services.EntityServices
{
    public sealed class ActivityTypeService : IActivityTypeService
    {
        private readonly ILogger logger;
        private readonly IRepositoryManager repositoryManager;
        private readonly IActivityTypeRepository activityTypeRepository;

        public ActivityTypeService(ILogger logger, IRepositoryManager repositoryManager)
        {
            this.logger = logger;
            this.repositoryManager = repositoryManager;
            activityTypeRepository = repositoryManager.ActivityTypeRepository;
        }

        public async Task<Result<ActivityTypeDto>> CreateAsync(ActivityTypeForCreatingDto activityTypeDto, CancellationToken cancellationToken = default)
        {
            if (await activityTypeRepository.IsTypeExists(activityTypeDto.Name, cancellationToken))
            {
                return Result.Fail($"Activity type with name: {activityTypeDto.Name} is already exists.");
            }
            var newActivityType = activityTypeDto.ToActivityType();
            activityTypeRepository.Create(newActivityType);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("New Activity type with id: {id} created.", newActivityType.Id);
            return newActivityType.ToActivityTypeDto();
        }


        public async Task<IEnumerable<ActivityTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var dbTypes = await repositoryManager.ActivityTypeRepository.GetAllAsync(trackChanges: false, cancellationToken);
            return dbTypes.Select(t => t.ToActivityTypeDto());
        }

        public async Task<ActivityTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (await repositoryManager.ActivityTypeRepository.GetByIdAsync(id, false, cancellationToken) is not ActivityType activityType)
            {
                logger.Warning("There is not activity type with id: {id}", id);
                return null;
            }
            return activityType.ToActivityTypeDto();
        }

        public async Task<Result<ActivityTypeDto>> UpdateAsync(Guid id, ActivityTypeForUpdateDto activityTypeDto, CancellationToken cancellationToken = default)
        {
            var dbActivityType = await repositoryManager.ActivityTypeRepository.GetByIdAsync(id, trackChanges: true, cancellationToken);
            if (dbActivityType is null)
            {
                return Result.Fail($"Activity type with id {id} doen't exists.");
            }
            ActivityTypeMapper.Map(activityTypeDto, dbActivityType);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            return dbActivityType.ToActivityTypeDto();
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            repositoryManager.ActivityTypeRepository.Delete(id);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
