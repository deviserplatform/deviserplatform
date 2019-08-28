
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Deviser.Admin.Extensions;
using Deviser.Core.Common;
using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
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

        public IAdminSite GetAdminConfig(Type adminConfiguratorType)
        {
            object entityConfiguration;
            return _adminConfigStore.TryGet(adminConfiguratorType, out entityConfiguration) ? (IAdminSite)entityConfiguration : null;
        }

        public void RegisterAdminSites()
        {
            //IServiceScopeFactory _serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            //using (var scope = _serviceScopeFactory.CreateScope())
            //{
            //    var provider = scope.ServiceProvider;

            var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.PlatformAssembly);
            List<TypeInfo> adminConfiguratorTypes = assemblies.GetDerivedTypeInfos(typeof(IAdminConfigurator));

            //assemblies.GetDerivedTypeInfos(typeof(IAdminConfigurator))[0].GetInterfaces()[1].IsGenericType

            foreach (var adminConfiguratorType in adminConfiguratorTypes)
            {
                var acInterface = adminConfiguratorType.GetInterfaces().Where(i => i.IsGenericType).FirstOrDefault();
                Type dbContextType = acInterface?.GetGenericArguments().FirstOrDefault();
                bool hasDbContext = dbContextType != null;
                IAdminSite adminSite;
                AdminBuilder adminBuilder;
                /** 
                    * 1. Crete AdminSite instance
                    * 2. Create AdminConfigurator instance
                    * 3. Invoke IAdminConfigurator.ConfigureAdmin and pass adminSite as argument
                */

                if (hasDbContext)
                {
                    using (var contextObj = (DbContext)_serviceProvider.GetRequiredService(dbContextType))
                    {

                        adminSite = new AdminSite(_serviceProvider, contextObj, _serviceProvider.GetRequiredService<IModelMetadataProvider>());
                        adminBuilder = new AdminBuilder(adminSite);

                        if (!contextObj.Database.Exists())
                            throw new InvalidOperationException($"Database is not exist for {dbContextType}, create a database and try again");

                        ConfigureAdminSites(adminConfiguratorType, adminSite, adminBuilder);
                    }
                }
                else
                {
                    adminSite = new AdminSite(_serviceProvider, _serviceProvider.GetRequiredService<IModelMetadataProvider>());
                    adminBuilder = new AdminBuilder(adminSite);
                    ConfigureAdminSites(adminConfiguratorType, adminSite, adminBuilder);
                }
            }
        }

        private void ConfigureAdminSites(TypeInfo adminConfiguratorType, IAdminSite adminSite, AdminBuilder adminBuilder)
        {
            var objAdminConfigurator = Activator.CreateInstance(adminConfiguratorType);
            var genericInterface = typeof(IAdminConfigurator);
            //var adminConfiguratorInterface = genericInterface.MakeGenericType(dbContextType);
            var configureAdminMethodInfo = genericInterface.GetMethod("ConfigureAdmin");
            configureAdminMethodInfo.Invoke(objAdminConfigurator, new object[] { adminBuilder });

            _adminConfigStore.GetOrAdd(adminConfiguratorType.AsType(), adminSite);
        }
    }
}
