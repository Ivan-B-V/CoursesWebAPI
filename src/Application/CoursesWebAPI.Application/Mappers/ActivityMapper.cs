using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using Riok.Mapperly.Abstractions;

namespace CoursesWebAPI.Application.Mappers
{
    [Mapper]
    public static partial class ActivityMapper
    {
        public static partial Activity ToActivity(this ActivityForCreatingDto source);

        public static partial Activity ToActivity(this ActivityForUpdateDto source);

        public static partial void Map(ActivityForUpdateDto source, Activity target);

        public static partial ActivityFullDto ToActivityDto(this Activity source);

        public static partial List<ActivityFullDto> ToActivityDtos(this IEnumerable<Activity> source);
    }
}
