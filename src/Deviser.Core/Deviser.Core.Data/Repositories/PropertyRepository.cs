using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        //Logger
        private readonly ILogger<PropertyRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public PropertyRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<PropertyRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        public Property CreateProperty(Property property)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbProperty = _mapper.Map<Entities.Property>(property);
            dbProperty.CreatedDate = dbProperty.LastModifiedDate = DateTime.Now;
            var result = context.Property.Add(dbProperty).Entity;
            context.SaveChanges();
            return _mapper.Map<Property>(result);
        }

        public Property GetProperty(Guid propertyId)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Property.Where(e => e.Id == propertyId)
                .Include(p => p.OptionList)
                .FirstOrDefault();
            return _mapper.Map<Property>(result);
        }

        public bool IsPropertyExist(string propertyName)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Property.Count(e => e.Name == propertyName);
            return result > 0;
        }

        public List<Property> GetProperties()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Property
                .Include(p => p.OptionList)
                .ToList();
            return _mapper.Map<List<Property>>(result);
        }

        public Property UpdateProperty(Property property)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbProperty = _mapper.Map<Entities.Property>(property);
            //property.LastModifiedDate = DateTime.Now;
            var result = context.Property.Update(dbProperty).Entity;
            context.SaveChanges();
            return _mapper.Map<Property>(result);
        }
    }
}
