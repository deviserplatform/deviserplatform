using Autofac;
using Deviser.Core.Data;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.IO;
using Deviser.Core.Library.Internal;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Routing;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Media;

namespace Deviser.Core.Library.Infrastructure
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<DeviserViewEngine>().As<IRazorViewEngine>();

            //builder.RegisterType<ModuleActionInvoker>().As<IModuleActionInvoker>();
            
            builder.RegisterType<ScopeService>().As<IScopeService>().InstancePerDependency();
            builder.RegisterType<ImageOptimizer>().As<IImageOptimizer>().InstancePerDependency();
            

            builder.RegisterType<ActionInvoker>().As<IActionInvoker>();
            builder.RegisterType<TypeActivatorCache>().As<ITypeActivatorCache>();

            //builder.RegisterType<ModuleInvokerProvider>().As<IModuleInvokerProvider>();
            builder.RegisterType<PageUrlConstraint>().As<IRouteConstraint>().SingleInstance();
            builder.RegisterType<DeviserControllerFactory>().As<IDeviserControllerFactory>();
            builder.RegisterType<DeviserRouteHandler>().As<DeviserRouteHandler>();
            
            builder.RegisterType<LayoutRepository>().As<ILayoutRepository>().InstancePerDependency();
            builder.RegisterType<LayoutTypeRepository>().As<ILayoutTypeRepository>().InstancePerDependency();
            builder.RegisterType<ContentTypeRepository>().As<IContentTypeRepository>().InstancePerDependency();
            builder.RegisterType<ModuleRepository>().As<IModuleRepository>().InstancePerDependency();
            builder.RegisterType<PageContentRepository>().As<IPageContentRepository>().InstancePerDependency();
            builder.RegisterType<PageRepository>().As<IPageRepository>().InstancePerDependency();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerDependency();
            builder.RegisterType<SiteSettingRepository>().As<ISiteSettingRepository>().InstancePerDependency();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerDependency();
            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>().InstancePerDependency();
            builder.RegisterType<OptionListRepository>().As<IOptionListRepository>().InstancePerDependency();
            builder.RegisterType<PropertyRepository>().As<IPropertyRepository>().InstancePerDependency();

            //builder.RegisterType<ContactProvider>().As<IContactProvider>();
                        
            builder.RegisterType<PageManager>().As<IPageManager>();
            builder.RegisterType<ModuleManager>().As<IModuleManager>();
            builder.RegisterType<ContentManager>().As<IContentManager>(); 
            builder.RegisterType<LayoutManager>().As<ILayoutManager>();
            builder.RegisterType<ThemeManager>().As<IThemeManager>();
            builder.RegisterType<Navigation>().As<INavigation>();
            builder.RegisterType<FileManagement>().As<IFileManagement>();
            builder.RegisterType<LanguageManager>().As<ILanguageManager>();
            builder.RegisterType<SettingManager>().As<ISettingManager>();
            
            //Autofac Property injection is not working
            //ref: https://github.com/autofac/Autofac.Mvc/issues/1
            builder.RegisterType<ModuleController>().PropertiesAutowired();

            //builder.RegisterType<DeviserDbContext>().As<DeviserDbContext>();
                        
        }
    }
}
