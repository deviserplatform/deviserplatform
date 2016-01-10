using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Data.Entity;

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
        private ILifetimeScope container;

        DeviserDBContext context;

        //Constructor
        public UserProvider(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<LayoutProvider>>();
            context = container.Resolve<DeviserDBContext>();
        }

        //Custom Field Declaration
        public List<User> GetUsers()
        {
            try
            {
                IEnumerable<User> returnData = context.Users
                    .Include(u => u.Roles)
                    .ToList();
                return new List<User>(returnData);
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
                User returnData = context.Users
                   .Where(e => e.Id == userId)
                   .Include(u => u.Roles)
                   .FirstOrDefault();
                return returnData;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling GetUser", ex);
            }
            return null;
        }

    }

}//End namespace
