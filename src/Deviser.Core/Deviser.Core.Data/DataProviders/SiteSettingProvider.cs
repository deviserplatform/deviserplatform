using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Data.Entity;

namespace Deviser.Core.Data.DataProviders
{

    public interface ISiteSettingProvider
    {
        List<SiteSetting> GetSettings();

    }

    public class SiteSettingProvider : DataProviderBase, ISiteSettingProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;
        private ILifetimeScope container;

        DeviserDBContext context;

        //Constructor
        public SiteSettingProvider(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<LayoutProvider>>();
            context = container.Resolve<DeviserDBContext>();
        }

        //Custom Field Declaration
        public List<SiteSetting> GetSettings()
        {
            try
            {
                IEnumerable<SiteSetting> returnData = context.SiteSetting.ToList();
                return new List<SiteSetting>(returnData);
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetSettings", ex);
            }
            return null;
        }

    }

}//End namespace
