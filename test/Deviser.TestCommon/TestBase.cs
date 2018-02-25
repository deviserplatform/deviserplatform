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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Xunit;
using User = Deviser.Core.Data.Entities.User;
using Role = Deviser.Core.Data.Entities.Role;
using Microsoft.AspNetCore.Routing;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using Deviser.Core.Library.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Deviser.Core.Library.Infrastructure;
using AutoMapper;

namespace Deviser.TestCommon
{
    public class TestBase
    {
        protected readonly ILifetimeScope _container;
        protected readonly IServiceProvider _serviceProvider;

        public TestBase()
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            var services = new ServiceCollection();

            var hostingEnvMock = new Mock<IHostingEnvironment>(MockBehavior.Strict);
            var currentAssemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
            hostingEnvMock.Setup(he => he.ApplicationName).Returns(currentAssemblyName);

            services.AddOptions();
            services
                .AddDbContext<DeviserDbContext>(b => b.UseInMemoryDatabase("DeviserWI").UseInternalServiceProvider(efServiceProvider));

            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<DeviserDbContext>();

            services.AddSingleton<DiagnosticSource>(new DiagnosticListener("Microsoft.AspNetCore"));
            services.AddSingleton<IHostingEnvironment>(hostingEnvMock.Object);
            services.Add(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, SerializerSettingsSetup>());
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.TryAddSingleton<ObjectMethodExecutorCache>();
            services.TryAddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
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


            //Mapper.Reset();
            MapperConfig.CreateMaps();

            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();
            containerBuilder.Populate(services);
            _container = containerBuilder.Build();
            _serviceProvider = new AutofacServiceProvider(_container);

        }

        protected static HttpContext CreateHttpContext(string httpMethod)
        {
            var routeData = new RouteData();
            routeData.Routers.Add(new Mock<IRouter>(MockBehavior.Strict).Object);

            var serviceProvider = new ServiceCollection().BuildServiceProvider();

            var httpContext = new Mock<HttpContext>(MockBehavior.Strict);

            var request = new Mock<HttpRequest>(MockBehavior.Strict);
            request.SetupGet(r => r.Method).Returns(httpMethod);
            request.SetupGet(r => r.Path).Returns(new PathString());
            request.SetupGet(r => r.Headers).Returns(new HeaderDictionary());
            httpContext.SetupGet(c => c.Request).Returns(request.Object);
            httpContext.SetupGet(c => c.RequestServices).Returns(serviceProvider);

            return httpContext.Object;
        }
    }
}
