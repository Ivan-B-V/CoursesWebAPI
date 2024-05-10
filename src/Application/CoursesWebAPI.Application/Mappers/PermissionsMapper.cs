using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Core.Identity;
using Riok.Mapperly.Abstractions;

namespace CoursesWebAPI.Application.Mappers
{
    [Mapper]
    public static partial class PermissionsMapper
    {
        private static IEnumerable<Permission> EnumsToObjects(this IEnumerable<Permissions> source) =>
            source.Select(s => s.EnumToObject());

        private static IEnumerable<Permissions> ObjectsToEnums(this IEnumerable<Permission> source) =>
            source.Select(s => s.ObjectToEnum());

        public static Permission EnumToObject(this Permissions source) =>
            new() { Id = (int)source, Name = source.ToString() };

        public static Permissions ObjectToEnum(this Permission source) =>
            Enum.Parse<Permissions>(source.Id.ToString());

        public static IEnumerable<Permissions> ToEnumList(this IEnumerable<Permission> source) =>
            ObjectsToEnums(source);
        public static ICollection<Permissions> ToEnumList(this ICollection<Permission> source) =>
            ObjectsToEnums(source).ToHashSet();

        public static ICollection<Permission> ToPermissionList(this ICollection<Permissions> source) =>
            EnumsToObjects(source).ToHashSet();

        public static IEnumerable<Permission> ToPermissionList(this IEnumerable<Permissions> source) =>
            EnumsToObjects(source);
    }
}
