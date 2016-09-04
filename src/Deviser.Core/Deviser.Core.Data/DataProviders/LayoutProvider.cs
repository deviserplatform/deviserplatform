using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{
    public interface ILayoutProvider
    {
        Layout CreateLayout(Layout layout);
        List<Layout> GetLayouts();
        Layout GetLayout(Guid layoutId);        
        Layout UpdateLayout(Layout layout);
    }

    public class LayoutProvider : DataProviderBase, ILayoutProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;

        //Constructor
        public LayoutProvider(ILifetimeScope container)
            :base(container)
        {
            logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        public Layout CreateLayout(Layout layout)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Layout resultLayout;
                    resultLayout = context.Layout.Add(layout).Entity;
                    context.SaveChanges();
                    return resultLayout;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling creating layout", ex);
            }
            return null;
        }
        
        public List<Layout> GetLayouts()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<Layout> returnData = context.Layout
                               .ToList();

                    return new List<Layout>(returnData); 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting all layouts", ex);
            }
            return null;
        }

        public Layout GetLayout(Guid layoutId)
        {
            try
            {

                using (var context = new DeviserDBContext(dbOptions))
                {
                    Layout returnData = context.Layout

                               .Where(e => e.Id == layoutId)
                               .FirstOrDefault();

                    return returnData; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting a layout", ex);
            }
            return null;
        }
        
        public Layout UpdateLayout(Layout layout)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Layout resultLayout;
                    resultLayout = context.Layout.Attach(layout).Entity;
                    context.Entry(layout).State = EntityState.Modified;

                    context.SaveChanges();
                    return resultLayout; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating layout", ex);
            }
            return null;
        }

    }

}//End namespace
