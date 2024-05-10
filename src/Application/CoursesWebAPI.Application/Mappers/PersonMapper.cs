using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using Riok.Mapperly.Abstractions;

namespace CoursesWebAPI.Application.Mappers
{
    [Mapper]
    public static partial class PersonMapper
    {
        public static PersonForUserDto ToPersonForUserDto(this Person source)
        {
            return new PersonForUserDto
            {
                Id = source.Id,
                FullName = string.Join(' ', source.FirsName, source.LastName, source.Patronomic),
                Sex = source.Sex.ToString()
            };
        }

        public static partial PersonDto ToRersonFullDto(this Person source);
    }
}
