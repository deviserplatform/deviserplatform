using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.DataProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Deviser.Core.Data.Extension
{
    public class ModuleDbContext : DbContext
    {
        private IInstallationProvider _installationProvider;
        public ModuleMetaInfo ModuleMetaInfo { get; set; }

        //public ModuleDbContext(DbContextOptions options)
        //    : base(options)
        //{

        //}

        //public ModuleDbContext(IServiceProvider serviceProvider)
        //{
        //    _installationProvider = serviceProvider.GetService<InstallationProvider>();
        //}

        public static ServiceProvider ServiceProvider { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            //var _serviceScope = ServiceProviderCache.Instance.GetOrAdd(optionBuilder.Options, providerRequired: true)
            //            .GetRequiredService<IServiceScopeFactory>()
            //            .CreateScope();

            //var scopedServiceProvider = _serviceScope.ServiceProvider;

            //var serviceCollection = new ServiceCollection();
            //serviceCollection.AddSingleton<IInstallationProvider, InstallationProvider>();
            //var _serviceProvider = serviceCollection.BuildServiceProvider();


            //var scopedServiceProvider = ((IInfrastructure<IServiceProvider>)this).Instance;

            var _installationProvider = ServiceProvider.GetService<IInstallationProvider>();

            if (ModuleMetaInfo == null)
                throw new ArgumentNullException(nameof(ModuleMetaInfo));

            if(string.IsNullOrEmpty(ModuleMetaInfo.ModuleName))
                throw new ArgumentNullException(nameof(ModuleMetaInfo.ModuleName));

            if (string.IsNullOrEmpty(ModuleMetaInfo.ModuleAssembly))
                throw new ArgumentNullException(nameof(ModuleMetaInfo.ModuleAssembly));

            if (_installationProvider != null)
            {
                _installationProvider.GetDbContextOptionsBuilder(optionBuilder, ModuleMetaInfo.ModuleAssembly);
            }
        }
    }
}
