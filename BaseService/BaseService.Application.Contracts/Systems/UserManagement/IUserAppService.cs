using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace BaseService.Systems.UserManagement
{
    public interface IUserAppService: IApplicationService
    {
        Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input);
        Task DeleteAsync(Guid id);
        Task DeleteManyAsync(List<Guid> ids);
        Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync();
        Task<IdentityUserDto> GetAsync(Guid id);
        Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input);
        Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);
        Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input);
        Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);
    }
}