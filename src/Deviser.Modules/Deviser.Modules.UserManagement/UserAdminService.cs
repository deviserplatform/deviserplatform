using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Extensions;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Modules.UserManagement
{
    public class UserAdminService : IAdminService<User>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Core.Data.Entities.User> _userManager;
        private readonly RoleManager<Core.Data.Entities.Role> _roleManager;

        public UserAdminService(IUserRepository userRepository,
            IMapper mapper,
            UserManager<Core.Data.Entities.User> userManager,
            RoleManager<Core.Data.Entities.Role> roleManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<IFormResult<User>> CreateItem(User item)
        {
            if (item != null)
            {
                var user = _mapper.Map<Core.Data.Entities.User>(item);
                user.Id = Guid.NewGuid();
                user.UserName = item.Email;
                var identityResult = await _userManager.CreateAsync(user, item.Password);
                if (identityResult.Succeeded)
                {
                    var resultUser = _mapper.Map<User>(user);
                    var result = new FormResult<User>(resultUser);
                    return result;
                }
            }
            return null;
        }

        public async Task<IAdminResult<User>> DeleteItem(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == Guid.Parse(userId));
            if (user != null)
            {
                var identityResult = await _userManager.DeleteAsync(user);
                if (identityResult != null)
                {
                    var userResult = _mapper.Map<User>(user);
                    var result = new AdminResult<User>(userResult);
                    return result;
                }
            }
            return null;
        }

        public async Task<PagedResult<User>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            var users = _userRepository.GetUsers();
            if (filter != null)
            {
                users = users.ApplyFilter(filter).ToList();
            }
            var pagedResult = new PagedResult<User>(users, pageNo, pageSize, orderByProperties);
            return await Task.FromResult(pagedResult);
        }

        public async Task<User> GetItem(string userId)
        {
            var user = _userManager.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Id == Guid.Parse(userId));

            if (user == null) return await Task.FromResult<User>(null);

            var result = _mapper.Map<User>(user);
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<User>> UpdateItem(User user)
        {
            var dbUser = _userManager.Users.FirstOrDefault(u => u.Id == user.Id);
            if (dbUser != null)
            {
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;

                var currentRoles = await _userManager.GetRolesAsync(dbUser);
                foreach (var role in currentRoles)
                {
                    await _userManager.RemoveFromRoleAsync(dbUser, role);
                }

                var roles = _roleManager.Roles.Where(r => user.Roles.Any(ur => ur.Id == r.Id)).ToList();

                foreach (var role in roles)
                {
                    await _userManager.AddToRoleAsync(dbUser, role.Name);
                }

                var identityResult = await _userManager.UpdateAsync(dbUser);
                if (identityResult != null)
                {
                    var userResult = _mapper.Map<User>(user);
                    var result = new FormResult<User>(userResult);
                    return await Task.FromResult(result);
                }
            }

            return await Task.FromResult<FormResult<User>>(null);
        }

        public async Task<IFormResult> ResetPassword(PasswordReset passwordReset)
        {
            FormResult result;
            if (passwordReset == null || passwordReset.Id == Guid.Empty ||
                string.IsNullOrEmpty(passwordReset.CurrentPassword) || string.IsNullOrEmpty(passwordReset.NewPassword))
            {
                result = new FormResult()
                {
                    IsSucceeded = false,
                    SuccessMessage = "Invalid parameters"
                };
                return await Task.FromResult(result);
            }


            var currentPassword = passwordReset.CurrentPassword;
            var newPassword = passwordReset.NewPassword;

            var user = _userManager.Users.FirstOrDefault(u => u.Id == passwordReset.Id);
            var identityResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (identityResult.Succeeded)
            {
                result = new FormResult()
                {
                    IsSucceeded = true,
                    FormBehaviour = FormBehaviour.StayOnEditMode,
                    SuccessMessage = "Password has been reset successfully"
                };
                return await Task.FromResult(result);
            }

            result = new FormResult(identityResult)
            {
                IsSucceeded = false,
                SuccessMessage = "Unable to reset Password",
            };

            return await Task.FromResult(result);
        }

        public async Task<IFormResult<User>> UnlockUserAccount(User user)
        {
            var dbUser = _userManager.Users.FirstOrDefault(u => u.Id == user.Id);
            var identityResult = await _userManager.SetLockoutEnabledAsync(dbUser, false);
            IFormResult<User> result;

            if (identityResult.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(dbUser);
            }

            if (identityResult.Succeeded)
            {
                result = new FormResult<User>()
                {
                    IsSucceeded = true,
                    SuccessMessage = "User has been unlocked successfully"
                };
                return await Task.FromResult(result);
            }

            result = new FormResult<User>()
            {
                IsSucceeded = false,
                SuccessMessage = "Unable to unlock the user",
            };
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<User>> LockUserAccount(User user)
        {
            IFormResult<User> result;
            var dbUser = _userManager.Users.FirstOrDefault(u => u.Id == user.Id);
            var identityResult = await _userManager.SetLockoutEnabledAsync(dbUser, true);
            if (identityResult.Succeeded)
            {
                identityResult = await _userManager.SetLockoutEndDateAsync(dbUser, DateTimeOffset.MaxValue);

                result = new FormResult<User>()
                {
                    IsSucceeded = true,
                    SuccessMessage = "User has been locked successfully"
                };
                return await Task.FromResult(result);
            }

            result = new FormResult<User>()
            {
                IsSucceeded = false,
                SuccessMessage = "Unable to lock the user",
            };
            return await Task.FromResult(result);
        }
    }
}
