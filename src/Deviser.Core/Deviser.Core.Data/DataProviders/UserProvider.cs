using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Common.DomainTypes;
using Microsoft.Extensions.Logging;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{

    public interface IUserProvider
    {
        List<User> GetUsers();
        User GetUser(Guid userId);

    }

    public class UserProvider : DataProviderBase, IUserProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> _logger;

        //Constructor
        public UserProvider(ILifetimeScope container)
            :base(container)
        {
            _logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        //Custom Field Declaration
        public List<User> GetUsers()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Users
                                .Include(u => u.Roles)
                                .ToList();
                    return Mapper.Map<List<User>>(result); 
                }
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
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Users
                               .Where(e => e.Id == userId)
                               .Include(u => u.Roles)
                               .FirstOrDefault();
                    return Mapper.Map<User>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetUser", ex);
            }
            return null;
        }

    }

}//End namespace
