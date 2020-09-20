using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{

    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUser(Guid userId);
        User GetUser(string userName);

    }

    public class UserRepository : IUserRepository
    {
        //Logger
        private readonly ILogger<UserRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public UserRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<UserRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        //Custom Field Declaration
        public List<User> GetUsers()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbUsers = context.Users
                    .Include(u => u.UserRoles)
                    .ToList();
                var result = _mapper.Map<List<User>>(dbUsers);
                foreach (var user in dbUsers)
                {
                    if (user.UserRoles != null && user.UserRoles.Count > 0)
                    {
                        var targetUser = result.First(u => u.Id == user.Id);
                        targetUser.Roles = new List<Role>();
                        foreach (var userRole in user.UserRoles)
                        {
                            if (userRole != null)
                            {
                                var role = context.Roles.FirstOrDefault(e => e.Id == userRole.RoleId);
                                targetUser.Roles.Add(_mapper.Map<Role>(role));
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetUsers", ex);
            }
            return null;
        }

        public User GetUser(Guid userId)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbUser = context.Users
                    .Where(e => e.Id == userId)
                    .Include(u => u.UserRoles)
                    .FirstOrDefault();
                var user = _mapper.Map<User>(dbUser);

                if (dbUser?.UserRoles == null || dbUser.UserRoles.Count <= 0) return user;

                AddUserRoles(user, context, dbUser);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetUser", ex);
            }
            return null;
        }
        
        public User GetUser(string userName)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbUser = context.User
                    .Include(u => u.UserRoles)
                    .FirstOrDefault(u => u.UserName == userName);

                var user = _mapper.Map<User>(dbUser);

                if (dbUser?.UserRoles == null || dbUser.UserRoles.Count <= 0) return user;

                AddUserRoles(user, context, dbUser);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while retrieving user details", ex);
            }
            return null;
        }

        private void AddUserRoles(User returnResult, DeviserDbContext context, Entities.User dbUser)
        {
            returnResult.Roles = new List<Role>();
            var allRoles = context.Role.ToDictionary(k => k.Id, v => v);
            foreach (var userRole in dbUser.UserRoles)
            {
                var role = allRoles[userRole.RoleId];
                returnResult.Roles.Add(_mapper.Map<Role>(role));
            }
        }
    }

}//End namespace
