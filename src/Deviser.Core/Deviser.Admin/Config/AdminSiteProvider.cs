
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
            List<TypeInfo> adminConfiguratorTypes = assemblies.GetDerivedTypeInfos(typeof(IAdminConfigurator<>));

            foreach (var adminConfiguratorType in adminConfiguratorTypes)
            {
                Type acInterface = adminConfiguratorType.GetInterfaces().First();
                Type dbContextType = acInterface.GetGenericArguments()[0];


                using (var contextObj = (DbContext)_serviceProvider.GetRequiredService(dbContextType))
                {
                    try
                    {
                        IAdminSite adminSite = new AdminSite(contextObj, _serviceProvider.GetRequiredService<IModelMetadataProvider>());
                        if (!contextObj.Database.Exists())
                            throw new InvalidOperationException($"Database is not exist for {dbContextType}, create a database and try again");

                        /** 
                         * 1. Crete AdminSite instance
                         * 2. Create AdminConfigurator instance
                         * 3. Invoke IAdminConfigurator.ConfigureAdmin and pass adminSite as argument
                        */

                        var objAdminConfigurator = Activator.CreateInstance(adminConfiguratorType);
                        var genericInterface = typeof(IAdminConfigurator);
                        //var adminConfiguratorInterface = genericInterface.MakeGenericType(dbContextType);
                        var configureAdminMethodInfo = genericInterface.GetMethod("ConfigureAdmin"); 
                        configureAdminMethodInfo.Invoke(objAdminConfigurator, new object[] { adminSite });                        

                        _adminConfigStore.GetOrAdd(adminConfiguratorType.AsType(), adminSite);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            //}
        }


    }
}
