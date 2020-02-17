using AutoMapper;
using Deviser.Core.Data;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Internal;
using Deviser.Core.Library.IO;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Media;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Deviser.Web.DependencyInjection;
using Deviser.Web.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Reflection;
using Role = Deviser.Core.Data.Entities.Role;
using User = Deviser.Core.Data.Entities.User;

namespace Deviser.TestCommon
{
    public class TestBase
    {
        protected IServiceProvider ServiceProvider { get; }

        public TestBase()
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            var services = new ServiceCollection();

            var hostingEnvMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);
            var currentAssemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
            hostingEnvMock.Setup(he => he.ApplicationName).Returns(currentAssemblyName);

            services.AddOptions();
            services.AddDbContext<DeviserDbContext>(b => b.UseInMemoryDatabase("DeviserWI").UseInternalServiceProvider(efServiceProvider));

            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<DeviserDbContext>();

            //services.AddSingleton<DiagnosticSource>(new DiagnosticListener("Microsoft.AspNetCore"));
            //services.AddSingleton<IWebHostEnvironment>(hostingEnvMock.Object);
            ////services.Add(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, SerializerSettingsSetup>());
            //services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            //services.TryAddSingleton<ObjectMethodExecutorCache>();
            //services.TryAddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
            ////services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance)

            //services.AddLogging();
            //services.AddOptions();


            services.AddHttpContextAccessor();


            services.AddAutoMapper(typeof(DeviserPlatformServiceCollectionExtensions).Assembly);

            services.AddScoped<ViewResultExecutor>();
            services.AddScoped<IScopeService, ScopeService>();
            services.AddScoped<IImageOptimizer, ImageOptimizer>();


            services.AddScoped<IActionInvoker, ActionInvoker>();
            services.AddScoped<ITypeActivatorCache, TypeActivatorCache>();
            //builder.RegisterType<ModuleInvokerProvider>().As<IModuleInvokerProvider>();
            //services.AddScoped<DeviserRouteHandler>();

            services.AddTransient<ILayoutRepository, LayoutRepository>();
            services.AddTransient<ILayoutTypeRepository, LayoutTypeRepository>();
            services.AddTransient<IContentTypeRepository, ContentTypeRepository>();
            services.AddTransient<IModuleRepository, ModuleRepository>();
            services.AddTransient<IPageContentRepository, PageContentRepository>();
            services.AddTransient<IPageRepository, PageRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILanguageRepository, LanguageRepository>();
            services.AddTransient<IOptionListRepository, OptionListRepository>();
            services.AddTransient<IPropertyRepository, PropertyRepository>();

            //builder.RegisterType<ContactProvider>().As<IContactProvider>();
            services.AddScoped<IRouteConstraint, PageUrlConstraint>();
            services.AddScoped<IDeviserControllerFactory, DeviserControllerFactory>();

            services.AddScoped<IPageManager, PageManager>();
            services.AddScoped<IModuleManager, ModuleManager>();
            services.AddScoped<IContentManager, ContentManager>();
            services.AddScoped<ILayoutManager, LayoutManager>();
            services.AddScoped<IThemeManager, ThemeManager>();
            services.AddScoped<INavigation, Navigation>();
            services.AddScoped<IFileManagement, FileManagement>();
            services.AddScoped<ILanguageManager, LanguageManager>();
            services.AddScoped<ISettingManager, SettingManager>();
            services.AddScoped<ISitemapService, SitemapService>();

            ServiceProvider = services.BuildServiceProvider();

            // services.AddMvc(option =>
            // {

            // })
            //.AddRazorOptions(options =>
            //{
            //    options.ViewLocationExpanders.Add(new ModuleLocationRemapper());
            //});

            //MapperConfig.CreateMaps();

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
