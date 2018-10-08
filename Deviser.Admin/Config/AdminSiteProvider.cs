
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Deviser.Admin.Extensions;
using Deviser.Core.Common;
using Deviser.Core.Common.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Admin.Config
{
    public class AdminSiteProvider : IAdminSiteProvider
    {
        private readonly IAdminConfigStore _adminConfigStore;
        private readonly IServiceProvider _serviceProvider;

        public AdminSiteProvider(IAdminConfigStore adminConfigStore, IServiceProvider serviceProvider)
        {
            _adminConfigStore = adminConfigStore;
            _serviceProvider = serviceProvider;

        }

        public AdminConfig<TEntity> GetAdminConfig<TEntity>()
            where TEntity : class
        {
            object entityConfiguration;
            return _adminConfigStore.TryGet(typeof(TEntity), out entityConfiguration) ? (AdminConfig<TEntity>)entityConfiguration : null;
        }

        public void RegisterAdminSites()
        {
            IServiceScopeFactory _serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var provider = scope.ServiceProvider;




                var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.PlatformAssembly);
                List<TypeInfo> adminConfiguratorTypes = new List<TypeInfo>();
                Type adminConfiguratorInterface = typeof(IAdminConfigurator<>);

                foreach (var assembly in assemblies)
                {
                    var controllerTypes = assembly.DefinedTypes.Where(t => adminConfiguratorInterface.IsAssignableFrom(t)).ToList();

                    if (controllerTypes != null && controllerTypes.Count > 0)
                        adminConfiguratorTypes.AddRange(controllerTypes);
                }

                foreach (var adminConfiguratorType in adminConfiguratorTypes)
                {
                    Type adminConfigureInterface = adminConfiguratorType.GetInterfaces().First();
                    Type dbContextType = adminConfigureInterface.GetGenericArguments()[0];
                    

                    using (var contextObj = (DbContext)provider.GetRequiredService(dbContextType))
                    {
                        try
                        {
                            IAdminSite adminSite = new AdminSite(contextObj);

                            if (!contextObj.Database.Exists())
                                throw new InvalidOperationException($"Database is not exist for {dbContextType}, create a database and try again");
                            var objAdminConfigurator = Activator.CreateInstance(adminConfiguratorType);
                            var configureAdminMethodInfo = typeof(IAdminConfigurator<>).GetMethod("ConfigureAdmin");
                            configureAdminMethodInfo.Invoke(objAdminConfigurator, new object[] { adminSite });

                            /** 
                             * 1. Create TAdminConfigurator instance
                             * 2. Crete AdminSite instance
                             * 3. Invoke IAdminConfigurator.ConfigureAdmin and pass adminSite as argument
                            */

                            //var adminOption = new AdminOption(contextObj, adminConfigStore, serviceProvider);//sp.GetService<IAdminOption>();
                            //adminConfigurator.ConfigureAdmin(adminOption);

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                }
            }
        }
    }
}
