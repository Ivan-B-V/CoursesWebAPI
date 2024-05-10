using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using Riok.Mapperly.Abstractions;

namespace CoursesWebAPI.Application.Mappers
{
    [Mapper]
    [UseStaticMapper(typeof(PermissionsMapper))]
    public static partial class RoleMapper
    {
        public static partial RoleFullDto ToRoleFullDto(this Role source);
        public static partial RoleDto ToRoleDto(this Role source);
        public static partial IEnumerable<RoleDto> ToRoleDtos(this IEnumerable<Role> source);
        public static ICollection<RoleDto> ToRoleDtos(this ICollection<Role> source) =>
            source.Select(r => r.ToRoleDto()).ToHashSet();
        public static partial Role ToRole(this RoleForCreatingDto source);
        public static partial void Map(RoleForUpdateDto source, Role target);
    }
} 
