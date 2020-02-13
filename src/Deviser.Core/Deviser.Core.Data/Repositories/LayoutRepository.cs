using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public class LayoutRepository : ILayoutRepository
    {
        //Logger
        private readonly ILogger<LayoutRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public LayoutRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<LayoutRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        public Layout CreateLayout(Layout layout)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbLayout = _mapper.Map<Entities.Layout>(layout);
                var result = context.Layout.Add(dbLayout).Entity;
                context.SaveChanges();
                return _mapper.Map<Layout>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Layout
                    .ToList();

                return _mapper.Map<List<Layout>>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Layout
                    .Where(e => e.IsDeleted)
                    .ToList();

                return _mapper.Map<List<Layout>>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Layout
                    .FirstOrDefault(e => e.Id == layoutId);

                return _mapper.Map<Layout>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var dbLayout = _mapper.Map<Entities.Layout>(layout);

                var result = context.Layout.Update(dbLayout).Entity;

                context.SaveChanges();
                return _mapper.Map<Layout>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var layout = GetLayout(layoutId);
                var dbLayout = _mapper.Map<Entities.Layout>(layout);

                context.Layout.Remove(dbLayout);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while deleting the layout", ex);
            }
            return false;
        }
    }

}//End namespace
