using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using Volo.Abp;

namespace BaseService.Systems.UserManagement
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        protected IdentityUserManager _userManager { get; }
        protected IIdentityUserRepository _userRepository { get; }
        public IIdentityRoleRepository _roleRepository { get; }

        public UserAppService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<IdentityUserDto> GetAsync(Guid id)
        {
            var user = await _userManager.GetByIdAsync(id);
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        /// <summary>
        /// 查询分页用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
            var count = await _userRepository.GetCountAsync(input.Filter);
            var list = await _userRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityUserDto>(
                count,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list)
            );
        }

        /// <summary>
        /// 根据用户ID获取用户角色信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            var roles = await _userRepository.GetRolesAsync(id);
            var dtos = ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles);

            return new ListResultDto<IdentityRoleDto>(dtos);
        }

        /// <summary>
        /// 获取可以给用户分配的角色
        /// </summary>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
        {
            var list = await _roleRepository.GetListAsync();
            var dtos = ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list);

            return new ListResultDto<IdentityRoleDto>(dtos);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var user = new IdentityUser(
                GuidGenerator.Create(),
                input.UserName,
                input.Email,
                CurrentTenant.Id
            );

            input.MapExtraPropertiesTo(user);

            (await _userManager.CreateAsync(user, input.Password)).CheckErrors();

            await UpdateUserByInput(user, input);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        /// <summary>
        /// 根据用户ID更新用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await _userManager.GetByIdAsync(id);
            user.ConcurrencyStamp = input.ConcurrencyStamp;

            (await _userManager.SetUserNameAsync(user, input.UserName)).CheckErrors();

            await UpdateUserByInput(user, input);
            input.MapExtraPropertiesTo(user);

            (await _userManager.UpdateAsync(user)).CheckErrors();

            if (!input.Password.IsNullOrEmpty())
            {
                (await _userManager.RemovePasswordAsync(user)).CheckErrors();
                (await _userManager.AddPasswordAsync(user, input.Password)).CheckErrors();
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        /// <summary>
        /// 根据用户ID修改用户绑定的角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
        {
            var user = await _userManager.GetByIdAsync(id);
            (await _userManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
            await _userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// 根据用户ID删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            if (CurrentUser.Id == id)
            {
                throw new BusinessException(code: IdentityErrorCodes.UserSelfDeletion);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return;
            }


            var result = await _userManager.DeleteAsync(user);
            result.CheckErrors();
            //(await _userManager.DeleteAsync(user)).CheckErrors();
        }

        /// <summary>
        /// 根据用户ID删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Delete)]
        public virtual async Task DeleteManyAsync(List<Guid> ids)
        {
            if (ids.Contains(CurrentUser.Id.Value))
            {
                throw new BusinessException(code: IdentityErrorCodes.UserSelfDeletion);
            }

            // todo 这里暂时用循环的方式处理批量删除，考虑后期扩展，IdentityUserManager和Microsoft.AspNetCore.Identity.UserManager<TUser>中没有批量删除的方法
            foreach (var id in ids)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user == null) continue;

                (await _userManager.DeleteAsync(user)).CheckErrors();
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
        {
            if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                (await _userManager.SetEmailAsync(user, input.Email)).CheckErrors();
            }

            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                (await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            }

            (await _userManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

            user.Name = input.Name;
            user.Surname = input.Surname;

            if (input.RoleNames != null)
            {
                (await _userManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
            }
        }
    }
}
