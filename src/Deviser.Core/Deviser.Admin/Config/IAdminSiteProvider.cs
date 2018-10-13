using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config
{
    public interface IAdminSiteProvider
    {
        IAdminSite GetAdminConfig(Type adminConfiguratorType);

        void RegisterAdminSites();
    }
}
