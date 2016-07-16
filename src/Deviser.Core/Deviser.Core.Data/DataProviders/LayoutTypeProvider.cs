using Autofac;
using Deviser.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.DataProviders
{
    public interface ILayoutTypeProvider
    {
        List<LayoutType> GetLayoutTypes();
        LayoutType GetLayoutType(Guid layoutTypeId);
        LayoutType GetLayoutType(string layoutTypeName);
        LayoutType CreateLayoutType(LayoutType layoutType);
        LayoutType UpdateLayoutType(LayoutType layoutType);

    }
    public class LayoutTypeProvider : DataProviderBase, ILayoutTypeProvider
    {
        //Logger
        private readonly ILogger<LayoutTypeProvider> logger;

        //Constructor
        public LayoutTypeProvider(ILifetimeScope container)
            :base(container)
        {
            logger = container.Resolve<ILogger<LayoutTypeProvider>>();
        }

        public LayoutType CreateLayoutType(LayoutType layoutType)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    LayoutType result;
                    layoutType.CreatedDate = layoutType.LastModifiedDate = DateTime.Now;
                    result = context.LayoutType.Add(layoutType).Entity;
                    context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while creating LayoutType", ex);
            }
            return null;
        }

        public LayoutType GetLayoutType(Guid layoutTypeId)
        {
            try
            {

                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.LayoutType
                               .Where(e => e.Id == layoutTypeId)
                               .FirstOrDefault();

                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting LayoutType by id", ex);
            }
            return null;
        }

        public LayoutType GetLayoutType(string layoutTypeName)
        {
            try
            {

                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.LayoutType
                               .Where(e => e.Name.ToLower() == layoutTypeName.ToLower())
                               .FirstOrDefault();

                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting LayoutType by id", ex);
            }
            return null;
        }

        public List<LayoutType> GetLayoutTypes()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.LayoutType
                        .Include(c => c.LayoutTypeProperties).ThenInclude(cp => cp.Property)                        
                        .ToList();
                    return new List<LayoutType>(returnData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting all LayoutTypes", ex);
            }
            return null;
        }

        public LayoutType UpdateLayoutType(LayoutType layoutType)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    LayoutType result;
                    layoutType.LastModifiedDate = DateTime.Now;
                    result = context.LayoutType.Attach(layoutType).Entity;
                    context.Entry(layoutType).State = EntityState.Modified;
                    context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating LayoutType", ex);
            }
            return null;
        }
    }
}
