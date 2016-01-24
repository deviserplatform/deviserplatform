using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library
{
    public class SiteBootstrapper : ISiteBootstrapper
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;
        private ILifetimeScope container;

        private ISiteSettingProvider siteSettingProvider;
        private IPageProvider pageProvider; 

        public SiteBootstrapper(ILifetimeScope container)
        {
            this.container = container;
            siteSettingProvider = container.Resolve<ISiteSettingProvider>();
            pageProvider = container.Resolve<IPageProvider>();
        }

        public void UpdateSiteSettings()
        {
            try
            {
                List<SiteSetting> siteSetting = siteSettingProvider.GetSettings();
                int homeTabId;
                var homeTabIdSetting = siteSetting.FirstOrDefault(s => s.SettingName == "HomePageId");
                if (homeTabIdSetting != null && !string.IsNullOrEmpty(homeTabIdSetting.SettingValue) && int.TryParse(homeTabIdSetting.SettingValue,out homeTabId))
                {
                    Globals.HomePage = pageProvider.GetPage(homeTabId);
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
