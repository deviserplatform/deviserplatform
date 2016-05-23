using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{

    public interface IUserProvider
    {
        List<User> GetUsers();
        User GetUser(string userId);

    }

    public class UserProvider : DataProviderBase, IUserProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;

        //Constructor
        public UserProvider(ILifetimeScope container)
            :base(container)
        {
            logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        //Custom Field Declaration
        public List<User> GetUsers()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<User> returnData = context.Users
                                .Include(u => u.Roles)
                                .ToList();
                    return new List<User>(returnData); 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetUsers", ex);
            }
            return null;
        }

        public User GetUser(string userId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    User returnData = context.Users
                               .Where(e => e.Id == userId)
                               .Include(u => u.Roles)
                               .FirstOrDefault();
                    return returnData; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling GetUser", ex);
            }
            return null;
        }

    }

}//End namespace
