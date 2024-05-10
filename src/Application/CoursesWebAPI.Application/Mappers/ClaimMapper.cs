using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using Riok.Mapperly.Abstractions;
using System.Security.Claims;

namespace CoursesWebAPI.Application.Mappers
{
    [Mapper]
    public static partial class ClaimMapper
    {
        public static partial ClaimDto ToClaimDto(this Claim source);
        public static partial Claim ToClaimDto(this ClaimDto source);
        public static partial ICollection<ClaimDto> ToClaimDtos(this IEnumerable<Claim> source);
        public static partial ICollection<Claim> ToClaims(this IEnumerable<ClaimDto> source);
    }
}
