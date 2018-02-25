using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Common.DomainTypes;
using Microsoft.Extensions.Logging;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.Repositories
{

    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUser(Guid userId);
        Guid GetUser(string userName);
      
    }

    public class UserRepository : RepositoryBase, IUserRepository
    {
        //Logger
        private readonly ILogger<UserRepository> _logger;

        //Constructor
        public UserRepository(ILifetimeScope container)
            : base(container)
        {
            _logger = container.Resolve<ILogger<UserRepository>>();
        }

        //Custom Field Declaration
        public List<User> GetUsers()
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.Users
                                .Include(u => u.UserRoles)
                                .ToList();
                    var resturnResult = Mapper.Map<List<User>>(result);
                    foreach (var user in result)
                    {
                        if (user.UserRoles!= null && user.UserRoles.Count > 0)
                        {
                            var targetUser = resturnResult.First(u => u.Id == user.Id);
                            targetUser.Roles = new List<Role>();
                            foreach (var userRole in user.UserRoles)
                            {
                                if (userRole != null)
                                {
                                    var role = context.Roles.FirstOrDefault(e => e.Id == userRole.RoleId);
                                    targetUser.Roles.Add(Mapper.Map<Role>(role));
                                }
                            }
                        }
                    }

                    return resturnResult;
                //}
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
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.Users
                               .Where(e => e.Id == userId)
                               .Include(u => u.UserRoles)
                               .FirstOrDefault();
                    var returnResult = Mapper.Map<User>(result);

                    if (result.UserRoles != null && result.UserRoles.Count > 0)
                    {
                        returnResult.Roles = new List<Role>();
                        foreach (var userRole in result.UserRoles)
                        {
                            if (userRole != null)
                            {
                                var role = context.Roles.FirstOrDefault(e => e.Id == userRole.RoleId);
                                returnResult.Roles.Add(Mapper.Map<Role>(role));
                            }
                        }
                    }

                    return returnResult;
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetUser", ex);
            }
            return null;
        }

        public Guid GetUser(string userName)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var userId = context.User
                        .Where(u => u.UserName == userName)
                        .Select(u => u.Id).FirstOrDefault();
                    return userId;
                //}
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occured while retrieving user details", ex);
            }
            return Guid.Empty;
        }


    }

}//End namespace
