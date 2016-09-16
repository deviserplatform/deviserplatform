using Autofac;
using Autofac.Extensions.DependencyInjection;
using Deviser.Core.Data.Entities;
using Deviser.WI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

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
                .AddDbContext<DeviserDBContext>(b => b.UseInMemoryDatabase().UseInternalServiceProvider(efServiceProvider));

            //services.AddIdentity<User, Role>()
            //        .AddEntityFrameworkStores<DeviserDBContext>();

            services.AddLogging();
            services.AddOptions();

            // IHttpContextAccessor is required for SignInManager, and UserManager
            //var context = new DefaultHttpContext();
            //context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature() { Handler = new TestAuthHandler() });
            //services.AddSingleton<IHttpContextAccessor>(
            //    new HttpContextAccessor()
            //    {
            //        HttpContext = context,
            //    });

            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();
            containerBuilder.Populate(services);
            container = containerBuilder.Build();
            serviceProvider = new AutofacServiceProvider(container);
        }
    }
}
