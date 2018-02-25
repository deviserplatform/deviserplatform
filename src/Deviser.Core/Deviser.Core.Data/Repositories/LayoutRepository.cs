using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Common.DomainTypes;
using Microsoft.Extensions.Logging;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.Repositories
{
    public interface ILayoutRepository
    {
        Layout CreateLayout(Layout layout);
        List<Layout> GetLayouts();
        List<Layout> GetDeletedLayouts();
        Layout GetLayout(Guid layoutId);        
        Layout UpdateLayout(Layout layout);
        bool DeleteLayout(Guid layoutId);
    }

    public class LayoutRepository : RepositoryBase, ILayoutRepository
    {
        //Logger
        private readonly ILogger<LayoutRepository> _logger;

        //Constructor
        public LayoutRepository(ILifetimeScope container)
            :base(container)
        {
            _logger = container.Resolve<ILogger<LayoutRepository>>();
        }

        public Layout CreateLayout(Layout layout)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var dbLayout = Mapper.Map<Entities.Layout>(layout);
                    var result = context.Layout.Add(dbLayout).Entity;
                    context.SaveChanges();
                    return Mapper.Map<Layout>(result);
                //}
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
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.Layout
                               .ToList();

                    return Mapper.Map<List<Layout>>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all layouts", ex);
            }
            return null;
        }

        public List<Layout> GetDeletedLayouts()
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.Layout
                               .Where(e => e.IsDeleted)
                               .ToList();

                    return Mapper.Map<List<Layout>>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting deleted layouts", ex);
            }
            return null;
        }

        public Layout GetLayout(Guid layoutId)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.Layout
                        .FirstOrDefault(e => e.Id == layoutId);

                    return Mapper.Map<Layout>(result);
                //}
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
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var dbLayout = Mapper.Map<Entities.Layout>(layout);

                    var result = context.Layout.Update(dbLayout).Entity;                   

                    context.SaveChanges();
                    return Mapper.Map<Layout>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating the layout", ex);
            }
            return null;
        }

        public bool DeleteLayout(Guid layoutId)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var layout = GetLayout(layoutId);
                    var dbLayout = Mapper.Map<Entities.Layout>(layout);

                    context.Layout.Remove(dbLayout);
                    context.SaveChanges();
                    return true;
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while deleting the layout", ex);
            }
            return false;
        }
    }  

}//End namespace
