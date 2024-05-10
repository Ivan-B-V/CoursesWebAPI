using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using FluentResults;

namespace CoursesWebAPI.Application.Contracts.IdentityServices
{
    public interface IRoleService
    {
        public Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default);
        public Task<RoleFullDto?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default);
        public Task<RoleFullDto?> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Result<RoleFullDto>> CreateRoleAsync(RoleForCreatingDto roleDto, CancellationToken cancellationToken = default);
        public Task<Result<RoleFullDto>> UpdateRole(Guid id, RoleForUpdateDto roleForUpdateDto, CancellationToken cancellationToken = default);
        public Task DeleteRoleAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
