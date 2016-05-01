using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Data.Entity;

namespace Deviser.Core.Data.DataProviders
{
    public interface ILayoutProvider
    {
        List<Layout> GetLayouts();
        Layout GetLayout(int layoutId);
        Layout CreateLayout(Layout layout);
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

        //Custom Field Declaration
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
                logger.LogError("Error occured while getting GetLayouts", ex);
            }
            return null;
        }

        public Layout GetLayout(int layoutId)
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
                logger.LogError("Error occured while calling GetLayout", ex);
            }
            return null;
        }
        
        public Layout CreateLayout(Layout layout)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Layout resultLayout;
                    resultLayout = context.Layout.Add(layout, GraphBehavior.SingleObject).Entity;
                    context.SaveChanges();
                    return resultLayout; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling CreateLayout", ex);
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
                    resultLayout = context.Layout.Attach(layout, GraphBehavior.SingleObject).Entity;
                    context.Entry(layout).State = EntityState.Modified;

                    context.SaveChanges();
                    return resultLayout; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdateLayout", ex);
            }
            return null;
        }

    }

}//End namespace
