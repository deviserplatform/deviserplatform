using Autofac;
using Deviser.Core.Data;
using Deviser.Core.Data.DataProviders;
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

namespace Deviser.Core.Library.Infrastructure
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<DeviserViewEngine>().As<IRazorViewEngine>();

            //builder.RegisterType<ModuleActionInvoker>().As<IModuleActionInvoker>();
            
            builder.RegisterType<ScopeService>().As<IScopeService>().InstancePerDependency();

            builder.RegisterType<ActionInvoker>().As<IActionInvoker>();
            builder.RegisterType<TypeActivatorCache>().As<ITypeActivatorCache>();

            //builder.RegisterType<ModuleInvokerProvider>().As<IModuleInvokerProvider>();
            builder.RegisterType<PageUrlConstraint>().As<IRouteConstraint>();//.SingleInstance();
            builder.RegisterType<DeviserControllerFactory>().As<IDeviserControllerFactory>();
            builder.RegisterType<DeviserRouteHandler>().As<DeviserRouteHandler>();
            
            builder.RegisterType<LayoutProvider>().As<ILayoutProvider>().InstancePerDependency();
            builder.RegisterType<LayoutTypeProvider>().As<ILayoutTypeProvider>().InstancePerDependency();
            builder.RegisterType<ContentTypeProvider>().As<IContentTypeProvider>().InstancePerDependency();
            builder.RegisterType<ModuleProvider>().As<IModuleProvider>().InstancePerDependency();
            builder.RegisterType<PageContentProvider>().As<IPageContentProvider>().InstancePerDependency();
            builder.RegisterType<PageProvider>().As<IPageProvider>().InstancePerDependency();
            builder.RegisterType<RoleProvider>().As<IRoleProvider>().InstancePerDependency();
            builder.RegisterType<SiteSettingProvider>().As<ISiteSettingProvider>().InstancePerDependency();
            builder.RegisterType<UserProvider>().As<IUserProvider>().InstancePerDependency();
            builder.RegisterType<LanguageProvider>().As<ILanguageProvider>().InstancePerDependency();
            builder.RegisterType<OptionListProvider>().As<IOptionListProvider>().InstancePerDependency();
            builder.RegisterType<PropertyProvider>().As<IPropertyProvider>().InstancePerDependency();

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
