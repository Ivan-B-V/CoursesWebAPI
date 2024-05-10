using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Application.Mappers;
using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace CoursesWebAPI.Application.Services.IdentityServices
{
    public sealed class ApplicationRoleManager : RoleManager<Role>, IRoleService
    {
        //private readonly ILogger logger;
        //private readonly RoleManager<Role> roleManager;

        //public RoleService(ILogger logger, RoleManager<Role> roleManager)
        //{
        //    this.logger = logger;
        //    this.roleManager = roleManager;
        //}

        public ApplicationRoleManager(IRoleStore<Role> store, 
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            Microsoft.Extensions.Logging.ILogger<RoleManager<Role>> logger) 
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            var dbRoles = await Roles.ToListAsync(cancellationToken);
            return dbRoles.ToRoleDtos();
        }

        public async Task<RoleFullDto?> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var role = await GetFullRoleByExpression(r => r.Id.Equals(id), cancellationToken);
            if (role is null)
            {
                return null;
            }
            return role.ToRoleFullDto();
        }

        public async Task<RoleFullDto?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            var role = await FindByNameAsync(name);
            if (role is null)
            {
                return null;
            }
            return await GetRoleAsDtoWithClaimsAsync(role);
        }

        public async Task<Result<RoleFullDto>> CreateRoleAsync(RoleForCreatingDto roleDto, CancellationToken cancellationToken = default)
        {
            if (await RoleExistsAsync(roleDto.Name))
            {
                return Result.Fail($"Role with name: {roleDto.Name} is already exists.");
            }
            var role = roleDto.ToRole();
            var result = await CreateAsync(role);
            if (!result.Succeeded)
            {
                return Result.Fail(result.Errors.Select(e => e.Description));
            }
            await AddClaimsToRoleAsync(role, roleDto.Claims.ToClaims());
            return role.ToRoleFullDto();
        }

        public async Task<Result<RoleFullDto>> UpdateRole(Guid id, RoleForUpdateDto roleForUpdateDto, CancellationToken cancellationToken = default)
        {
            var role = await FindByIdAsync(id.ToString());
            if (role is null)
            {
                return Result.Fail($"There is no role with id: {id}");
            }
            RoleMapper.Map(roleForUpdateDto, role);
            role.Permissions = roleForUpdateDto.Permissions.Select(p => new Permission { Id = (int)p, Name = p.ToString() }).ToHashSet();
            //put this into transaction
            //begin transaction
            var result = await UpdateAsync(role);
            if (!result.Succeeded)
            {
                return Result.Fail(result.Errors.Select(e => e.Description));
            }
            //await RemoveAllClaimsFromRole(role);
            //await AddClaimsToRoleAsync(role, roleForUpdateDto.Claims.ToClaims());
            //end transaction
            //finish method
            return role.ToRoleFullDto();
        }

        public async Task DeleteRoleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var role = await FindByIdAsync(id.ToString());
            if(role is null)
            {
                return;
            }
            await DeleteAsync(role);
        }

        private async Task<Role?> GetFullRoleByExpression(Expression<Func<Role, bool>> expression, CancellationToken cancellationToken)
        {
            return await Roles.Where(expression)
                              .Include(r => r.Permissions)
                              .AsSplitQuery()
                              .FirstOrDefaultAsync(cancellationToken);
        }

        private async Task<RoleFullDto> GetRoleAsDtoWithClaimsAsync(Role role)
        {
            var claims = await GetClaimsAsync(role);
            return new()
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                Claims = claims.ToClaimDtos()
            };
        }

        private async Task AddClaimsToRoleAsync(Role role, IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                await AddClaimAsync(role, claim);
            }
        }

        private async Task RemoveAllClaimsFromRole(Role role)
        {
            var claims = await GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await RemoveClaimAsync(role, claim);
            }
        }
    }
}
