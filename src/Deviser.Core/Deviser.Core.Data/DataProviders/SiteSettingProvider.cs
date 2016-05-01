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

        //Constructor
        public SiteSettingProvider(ILifetimeScope container)
            :base(container)
        {
            logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        //Custom Field Declaration
        public List<SiteSetting> GetSettings()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<SiteSetting> returnData = context.SiteSetting.ToList();
                    return new List<SiteSetting>(returnData); 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetSettings", ex);
            }
            return null;
        }

    }

}//End namespace
