using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using Riok.Mapperly.Abstractions;

namespace CoursesWebAPI.Application.Mappers
{
    [Mapper]
    [UseStaticMapper(typeof(PersonMapper))]
    [UseStaticMapper(typeof(RoleMapper))]
    public static partial class UserMapper
    {
       public static partial UserDto ToUserDto(this User source);

       public static partial User ToUser(this UserForRegistrationDto source);

       [MapperIgnoreTarget(nameof(User.Roles))]
       public static partial void Map(UserForUpdateDto source, User target);
       
       public static ICollection<UserDto> ToUserDtos(this ICollection<User> source) => 
            source.Select(user => user.ToUserDto()).ToList();
    }
}
