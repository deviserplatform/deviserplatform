using Autofac;
using Deviser.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Deviser.Core.Data.DataProviders
{
    public interface IPropertyProvider
    {
        Property CreateProperty(Property property);
        List<Property> GetProperties();
        Property GetProperty(Guid propertyId);
        Property UpdateProperty(Property property);
    }

    public class PropertyProvider : DataProviderBase, IPropertyProvider
    {
        //Logger
        private readonly ILogger<PropertyProvider> logger;

        //Constructor
        public PropertyProvider(ILifetimeScope container)
            :base(container)
        {
            logger = container.Resolve<ILogger<PropertyProvider>>();
        }

        public Property CreateProperty(Property property)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Property result;
                    //property.CreatedDate = property.LastModifiedDate = DateTime.Now;
                    result = context.Property.Add(property).Entity;
                    context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while creating a property", ex);
            }
            return null;
        }

        public Property GetProperty(Guid propertyId)
        {
            try
            {

                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.Property.Where(e => e.Id == propertyId)
                        .Include(p=>p.PropertyOptionList)
                               .FirstOrDefault();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Property by id", ex);
            }
            return null;
        }

        public List<Property> GetProperties()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.Property
                        .Include(p => p.PropertyOptionList)
                        .ToList();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Properties", ex);
            }
            return null;
        }

        public Property UpdateProperty(Property property)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Property result;
                    //property.LastModifiedDate = DateTime.Now;
                    result = context.Property.Attach(property).Entity;
                    context.Entry(property).State = EntityState.Modified;
                    context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating PropertyOptionList", ex);
            }
            return null;
        }
    }
}
