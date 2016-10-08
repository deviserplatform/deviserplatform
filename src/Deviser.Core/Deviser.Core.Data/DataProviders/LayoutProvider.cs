using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Common.DomainTypes;
using Microsoft.Extensions.Logging;
using Autofac;
using AutoMapper;
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
        private readonly ILogger<LayoutProvider> _logger;

        //Constructor
        public LayoutProvider(ILifetimeScope container)
            :base(container)
        {
            _logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        public Layout CreateLayout(Layout layout)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbLayout = Mapper.Map<Entities.Layout>(layout);
                    var result = context.Layout.Add(dbLayout).Entity;
                    context.SaveChanges();
                    return Mapper.Map<Layout>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling creating layout", ex);
            }
            return null;
        }
        
        public List<Layout> GetLayouts()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Layout
                               .ToList();

                    return Mapper.Map<List<Layout>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all layouts", ex);
            }
            return null;
        }

        public Layout GetLayout(Guid layoutId)
        {
            try
            {

                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Layout
                        .FirstOrDefault(e => e.Id == layoutId);

                    return Mapper.Map<Layout>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting a layout", ex);
            }
            return null;
        }
        
        public Layout UpdateLayout(Layout layout)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbLayout = Mapper.Map<Entities.Layout>(layout);

                    var result = context.Layout.Attach(dbLayout).Entity;
                    context.Entry(layout).State = EntityState.Modified;

                    context.SaveChanges();
                    return Mapper.Map<Layout>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating layout", ex);
            }
            return null;
        }
    }

}//End namespace
