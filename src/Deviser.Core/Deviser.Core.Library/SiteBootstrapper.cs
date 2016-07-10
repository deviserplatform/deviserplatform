using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Core.Common;

namespace Deviser.Core.Library
{
    public class SiteBootstrapper : ISiteBootstrapper
    {
        //Logger
        private readonly ILogger<SiteBootstrapper> logger;
        private ILifetimeScope container;

        private ISiteSettingProvider siteSettingProvider;
        private IPageProvider pageProvider;

        public SiteBootstrapper(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<SiteBootstrapper>>();
            siteSettingProvider = container.Resolve<ISiteSettingProvider>();
            pageProvider = container.Resolve<IPageProvider>();
        }

        public void InitializeSite()
        {
            try
            {
                List<SiteSetting> siteSetting = siteSettingProvider.GetSettings();
                Guid homeTabId;
                string strHomeTabId = siteSettingProvider.GetSettingValue("HomePageId");
                string siteRoot = siteSettingProvider.GetSettingValue("SiteRoot");

                if (!string.IsNullOrEmpty(strHomeTabId) && Guid.TryParse(strHomeTabId, out homeTabId))
                {
                    Globals.HomePage = pageProvider.GetPage(homeTabId);
                    Globals.SiteRoot = siteRoot;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while initializing site ");
                logger.LogError(errorMessage, ex);
            }
        }
    }
}
