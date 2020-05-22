using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{
    public interface IPropertyRepository
    {
        Property CreateProperty(Property dbProperty);
        List<Property> GetProperties();
        Property GetProperty(Guid propertyId);
        bool IsPropertyExist(string propertyName);
        Property UpdateProperty(Property dbProperty);
    }

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
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbProperty = _mapper.Map<Entities.Property>(property);
                dbProperty.CreatedDate = dbProperty.LastModifiedDate = DateTime.Now;
                var result = context.Property.Add(dbProperty).Entity;
                context.SaveChanges();
                return _mapper.Map<Property>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating a property", ex);
            }
            return null;
        }

        public Property GetProperty(Guid propertyId)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Property.Where(e => e.Id == propertyId)
                    .Include(p => p.OptionList)
                    .FirstOrDefault();
                return _mapper.Map<Property>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Property by id", ex);
            }
            return null;
        }

        public bool IsPropertyExist(string propertyName)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Property.Count(e => e.Name == propertyName);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while checking whether property Property by propertyName", ex);
            }
            return false;
        }

        public List<Property> GetProperties()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Property
                    .Include(p => p.OptionList)
                    .ToList();
                return _mapper.Map<List<Property>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Properties", ex);
            }
            return null;
        }

        public Property UpdateProperty(Property property)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbProperty = _mapper.Map<Entities.Property>(property);
                //property.LastModifiedDate = DateTime.Now;
                var result = context.Property.Update(dbProperty).Entity;
                context.SaveChanges();
                return _mapper.Map<Property>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating OptionList", ex);
            }
            return null;
        }
    }
}
