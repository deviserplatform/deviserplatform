using Autofac;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.DataProviders
{
    public class PermissionProvider : DataProviderBase
    {
        //Logger
        private readonly ILogger<PermissionProvider> logger;

        //Constructor
        public PermissionProvider(ILifetimeScope container)
            :base(container)
        {
            logger = container.Resolve<ILogger<PermissionProvider>>();
        }

        public List<Permission> GetPermissions()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var result = context.Permission
                               .ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Permissions", ex);
            }
            return null;
        }
    }
}
