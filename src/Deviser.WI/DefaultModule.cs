using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.Controllers;

namespace Deviser.WI
{
    public class DefaultModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //In 6.0.0.rc1 Custom ViewEngine is not working. In future, we can change the implementation.
            //builder.RegisterType<DeviserViewEngine>().As<IRazorViewEngine>();

            builder.RegisterType<ModuleActionInvoker>().As<IModuleActionInvoker>();
            builder.RegisterType<ModuleInvokerProvider>().As<IModuleInvokerProvider>();

            builder.RegisterType<LayoutProvider>().As<ILayoutProvider>();
            builder.RegisterType<ModuleProvider>().As<IModuleProvider>();
            builder.RegisterType<PageContentProvider>().As<IPageContentProvider>();
            builder.RegisterType<PageProvider>().As<IPageProvider>();
            builder.RegisterType<RoleProvider>().As<IRoleProvider>();
            builder.RegisterType<SiteSettingProvider>().As<ISiteSettingProvider>();
            builder.RegisterType<UserProvider>().As<IUserProvider>();
            builder.RegisterType<DeviserDBContext>().As<DeviserDBContext>();
        }
    }
}
