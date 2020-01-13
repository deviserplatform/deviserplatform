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

namespace Deviser.Modules.UserManagement
{
    public class UserAdminService : IAdminService<User>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<Core.Data.Entities.User> _userManager;

        public UserAdminService(IUserRepository userRepository, UserManager<Core.Data.Entities.User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<User> CreateItem(User item)
        {
            if (item != null)
            {
                var user = Mapper.Map<Core.Data.Entities.User>(item);
                user.Id = Guid.NewGuid();
                user.UserName = item.Email;
                var result = await _userManager.CreateAsync(user, item.Password);
                if (result.Succeeded)
                {
                    Mapper.Map<User>(user);
                }
            }
            return null;
        }

        public async Task<User> DeleteItem(string itemId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == Guid.Parse(itemId));
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result != null)
                {
                    return Mapper.Map<User>(user);
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

        public async Task<User> GetItem(string itemId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == Guid.Parse(itemId));
            if (user != null)
            {
                return Mapper.Map<User>(user);
            }
            return null;
        }

        public async Task<User> UpdateItem(User item)
        {
            var user = Mapper.Map<Core.Data.Entities.User>(item);
            var result = await _userManager.UpdateAsync(user);
            if (result != null)
            {
                return Mapper.Map<User>(user);
            }
            return null;
        }

        public async Task<FormResult> ResetPassword(PasswordReset passwordReset)
        {
            if (passwordReset == null || passwordReset.UserId == Guid.Empty ||
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

            var user = _userManager.Users.FirstOrDefault(u => u.Id == passwordReset.UserId);
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
    }
}
