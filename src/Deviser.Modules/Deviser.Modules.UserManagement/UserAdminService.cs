using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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

        public async Task<User> CreateItem(User item)
        {
            if (item != null)
            {
                var user = _mapper.Map<Core.Data.Entities.User>(item);
                user.Id = Guid.NewGuid();
                user.UserName = item.Email;
                var identityResult = await _userManager.CreateAsync(user, item.Password);
                if (identityResult.Succeeded)
                {
                    var result = _mapper.Map<User>(user);
                    return result;
                }
            }
            return null;
        }

        public async Task<User> DeleteItem(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == Guid.Parse(userId));
            if (user != null)
            {
                var identityResult = await _userManager.DeleteAsync(user);
                if (identityResult != null)
                {
                    var result = _mapper.Map<User>(user);
                    return result;
                }
            }
            return null;
        }

        public async Task<PagedResult<User>> GetAll(int pageNo, int pageSize, string orderByProperties)
        {
            var users = _userRepository.GetUsers();
            int skip = (pageNo - 1) * pageSize;
            int total = users.Count;
            var result = users.Skip(skip).Take(pageSize);
            return new PagedResult<User>(result, pageNo, pageSize, total);
        }

        public async Task<User> GetItem(string userId)
        {
            var user = _userManager.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Id == Guid.Parse(userId));

            if (user != null)
            {
                var result = _mapper.Map<User>(user);
                return result;
            }
            return null;
        }

        public async Task<User> UpdateItem(User user)
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

                var result = await _userManager.UpdateAsync(dbUser);
                if (result != null)
                {
                    return _mapper.Map<User>(user);
                }
            }

            return null;
        }

        public async Task<FormResult> ResetPassword(PasswordReset passwordReset)
        {
            if (passwordReset == null || passwordReset.Id == Guid.Empty ||
                string.IsNullOrEmpty(passwordReset.CurrentPassword) || string.IsNullOrEmpty(passwordReset.NewPassword))
            {
                return new FormResult()
                {
                    IsSucceeded = false,
                    SuccessMessage = "Invalid parameters"
                };
            }


            var currentPassword = passwordReset.CurrentPassword;
            var newPassword = passwordReset.NewPassword;

            var user = _userManager.Users.FirstOrDefault(u => u.Id == passwordReset.Id);
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result.Succeeded)
            {
                return new FormResult()
                {
                    IsSucceeded = true,
                    SuccessMessage = "Password has been reset successfully"
                };
            }

            return new FormResult(result)
            {
                IsSucceeded = false,
                SuccessMessage = "Unable to reset Password",
            };
        }

        public async Task<FormResult> UnlockUserAccount(User user)
        {
            var dbUser = _userManager.Users.FirstOrDefault(u => u.Id == user.Id);
            var result = await _userManager.SetLockoutEnabledAsync(dbUser, false);
            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(dbUser);
            }

            if (result.Succeeded)
            {
                return new FormResult()
                {
                    IsSucceeded = true,
                    SuccessMessage = "User has been unlocked successfully"
                };
            }

            return new FormResult(result)
            {
                IsSucceeded = false,
                SuccessMessage = "Unable to unlock the user",
            };

        }

        public async Task<FormResult> LockUserAccount(User user)
        {
            var dbUser = _userManager.Users.FirstOrDefault(u => u.Id == user.Id);

            var result = await _userManager.SetLockoutEnabledAsync(dbUser, true);
            if (result.Succeeded)
            {
                result = await _userManager.SetLockoutEndDateAsync(dbUser, DateTimeOffset.MaxValue);

                return new FormResult()
                {
                    IsSucceeded = true,
                    SuccessMessage = "User has been locked successfully"
                };
            }

            return new FormResult(result)
            {
                IsSucceeded = false,
                SuccessMessage = "Unable to lock the user",
            };
        }
    }
}
