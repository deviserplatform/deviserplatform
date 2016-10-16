using Autofac;
using Autofac.Extensions.DependencyInjection;
using Deviser.Core.Common.DomainTypes;
using Deviser.WI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Deviser.Core.Data;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.Modules;
using Deviser.WI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Xunit;
using User = Deviser.Core.Data.Entities.User;
using Role = Deviser.Core.Data.Entities.Role;

namespace Deviser.TestCommon
{
    public class TestBase
    {
        protected readonly ILifetimeScope container;
        protected readonly IServiceProvider serviceProvider;

        public TestBase()
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            var services = new ServiceCollection();
            services.AddOptions();
            services
                .AddDbContext<DeviserDbContext>(b => b.UseInMemoryDatabase().UseInternalServiceProvider(efServiceProvider));

            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<DeviserDbContext, Guid>();

            services.AddSingleton<DiagnosticSource>(new DiagnosticListener("Microsoft.AspNetCore"));
            services.Add(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, SerializerSettingsSetup>());
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            //services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance)

            services.AddLogging();
            services.AddOptions();

            services.AddMvc(option =>
            {
                //var jsonOutputFormatter = new JsonOutputFormatter();
                //jsonOutputFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                ////jsonOutputFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                //jsonOutputFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                //jsonOutputFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                //var jsonOutputFormatterOld = option.OutputFormatters.FirstOrDefault(formatter => formatter is JsonOutputFormatter);
                //if (jsonOutputFormatterOld != null)
                //{
                //    option.OutputFormatters.Remove(jsonOutputFormatterOld);
                //}
                ////options.OutputFormatters.RemoveAll(formatter => formatter.Instance.GetType() == typeof(JsonOutputFormatter));
                //option.OutputFormatters.Insert(0, jsonOutputFormatter);
            })
           .AddRazorOptions(options =>
           {
               options.ViewLocationExpanders.Add(new ModuleLocationRemapper());
           });

            // IHttpContextAccessor is required for SignInManager, and UserManager
            //var context = new DefaultHttpContext();
            //context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature() { Handler = new TestAuthHandler() });
            //services.AddSingleton<IHttpContextAccessor>(
            //    new HttpContextAccessor()
            //    {
            //        HttpContext = context,
            //    });

            

            MapperConfig.CreateMaps();

            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();
            containerBuilder.Populate(services);
            container = containerBuilder.Build();
            serviceProvider = new AutofacServiceProvider(container);

        }
    }
}
