using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{
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
            using var context = new DeviserDbContext(_dbOptions);
            var dbLayout = _mapper.Map<Entities.Layout>(layout);
            var result = context.Layout.Add(dbLayout).Entity;
            context.SaveChanges();
            return _mapper.Map<Layout>(result);
        }

        public List<Layout> GetLayouts()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Layout
                .ToList();

            return _mapper.Map<List<Layout>>(result);
        }

        public List<Layout> GetDeletedLayouts()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Layout
                .Where(e => !e.IsActive)
                .ToList();

            return _mapper.Map<List<Layout>>(result);
        }

        public Layout GetLayout(Guid layoutId)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Layout
                .FirstOrDefault(e => e.Id == layoutId);

            return _mapper.Map<Layout>(result);
        }

        public Layout UpdateLayout(Layout layout)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbLayout = _mapper.Map<Entities.Layout>(layout);

            var result = context.Layout.Update(dbLayout).Entity;

            context.SaveChanges();
            return _mapper.Map<Layout>(result);
        }

        public bool DeleteLayout(Guid layoutId)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var layout = GetLayout(layoutId);
            var dbLayout = _mapper.Map<Entities.Layout>(layout);

            context.Layout.Remove(dbLayout);
            context.SaveChanges();
            return true;
        }
    }

}//End namespace
