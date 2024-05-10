using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using Riok.Mapperly.Abstractions;

namespace CoursesWebAPI.Application.Mappers
{
    [Mapper]
    public static partial class ActivityTypeMapper
    {
        public static partial ActivityType ToActivityType(this ActivityTypeForUpdateDto source);

        public static partial ActivityType ToActivityType(this ActivityTypeForCreatingDto source);

        public static partial void Map(ActivityTypeForUpdateDto source, ActivityType target);
        
        public static partial ActivityTypeDto ToActivityTypeDto(this ActivityType source);
    }
}
