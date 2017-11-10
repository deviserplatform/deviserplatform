﻿using Autofac;
using Deviser.Core.Data;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Library;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.FileManagement;
using Deviser.Core.Library.Internal;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Sites;
using Deviser.WI.Infrastructure;
using Microsoft.AspNetCore.Routing;

namespace Deviser.WI
{
    public class DefaultModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //In 6.0.0.rc1 Custom ViewEngine is not working. In future, we can change the implementation.
            //builder.RegisterType<DeviserViewEngine>().As<IRazorViewEngine>();
            //builder.RegisterType<ModuleActionInvoker>().As<IModuleActionInvoker>();
            builder.RegisterType<ActionInvoker>().As<IActionInvoker>();
            builder.RegisterType<TypeActivatorCache>().As<ITypeActivatorCache>();
            
            //builder.RegisterType<ModuleInvokerProvider>().As<IModuleInvokerProvider>();
            builder.RegisterType<PageUrlConstraint>().As<IRouteConstraint>().SingleInstance();
            builder.RegisterType<DeviserControllerFactory>().As<IDeviserControllerFactory>();
            

            builder.RegisterType<LayoutProvider>().As<ILayoutProvider>();
            builder.RegisterType<LayoutTypeProvider>().As<ILayoutTypeProvider>();
            builder.RegisterType<ContentTypeProvider>().As<IContentTypeProvider>();
            builder.RegisterType<ModuleProvider>().As<IModuleProvider>();
            builder.RegisterType<PageContentProvider>().As<IPageContentProvider>();
            builder.RegisterType<PageProvider>().As<IPageProvider>();
            builder.RegisterType<RoleProvider>().As<IRoleProvider>();
            builder.RegisterType<SiteSettingProvider>().As<ISiteSettingProvider>();
            builder.RegisterType<UserProvider>().As<IUserProvider>();
            builder.RegisterType<LanguageProvider>().As<ILanguageProvider>();
            builder.RegisterType<OptionListProvider>().As<IOptionListProvider>();
            builder.RegisterType<PropertyProvider>().As<IPropertyProvider>(); 
                        
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

            builder.RegisterType<DeviserDbContext>().As<DeviserDbContext>();
        }
    }
}
