using System;

namespace Deviser.Admin.Config
{
    public interface IAdminSiteProvider
    {
        IAdminSite GetAdminConfig(Type adminConfiguratorType);

        void RegisterAdminSites(IServiceProvider serviceProvider);
    }
}
