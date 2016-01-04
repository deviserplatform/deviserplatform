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

    public class SiteSettingProvider :  DataProviderBase , ISiteSettingProvider 
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;
        private IContainer container;

        DeviserDBContext context;

        //Constructor
        public SiteSettingProvider(IContainer container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        //Custom Field Declaration
        public List<SiteSetting> GetSettings()
        {
            try
            {
                using (context = container.Resolve<DeviserDBContext>())
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
