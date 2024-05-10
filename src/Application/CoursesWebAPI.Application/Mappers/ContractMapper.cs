using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using Riok.Mapperly.Abstractions;

namespace CoursesWebAPI.Application.Mappers
{
    [Mapper]
    public static partial class ContractMapper
    {
        public static partial Contract ToContact(this ContractForCreatingDto source);

        [MapProperty(nameof(Contract.Activities), nameof(ContractFullDto.ActivitiesIds))]
        public static partial ContractFullDto ToContactFullDto(this Contract source);

        public static partial void Map(ContractForUpdateDto source, Contract target);

        private static ICollection<Guid> ActvitiesToIds(ICollection<Activity> activities) =>
            activities.Select(a => a.Id).ToHashSet();
    }
}
