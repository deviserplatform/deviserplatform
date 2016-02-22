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
        private ILifetimeScope container;

        DeviserDBContext context;

        //Constructor
        public LayoutProvider(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<LayoutProvider>>();
            context = container.Resolve<DeviserDBContext>();
        }

        //Custom Field Declaration
        public List<Layout> GetLayouts()
        {
            try
            {
                IEnumerable<Layout> returnData = context.Layout
                    .ToList();

                return new List<Layout>(returnData);
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

                Layout returnData = context.Layout

                   .Where(e => e.Id == layoutId)
                   .FirstOrDefault();

                return returnData;
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
                Layout resultLayout;
                resultLayout = context.Layout.Add(layout, GraphBehavior.SingleObject).Entity;
                context.SaveChanges();
                return resultLayout;
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
                Layout resultLayout;
                resultLayout = context.Layout.Attach(layout, GraphBehavior.SingleObject).Entity;
                context.Entry(layout).State = EntityState.Modified;

                context.SaveChanges();
                return resultLayout;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdateLayout", ex);
            }
            return null;
        }

    }

}//End namespace
