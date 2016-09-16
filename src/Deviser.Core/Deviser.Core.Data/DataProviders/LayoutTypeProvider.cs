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
        LayoutType CreateLayoutType(LayoutType layoutType);
        List<LayoutType> GetLayoutTypes();
        LayoutType GetLayoutType(Guid layoutTypeId);
        LayoutType GetLayoutType(string layoutTypeName);        
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
                        .Include(lt=>lt.LayoutTypeProperties)
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
                        .Include(lt => lt.LayoutTypeProperties)
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
                        .Include(c => c.LayoutTypeProperties).ThenInclude(cp => cp.Property).ThenInclude(p=>p.PropertyOptionList)
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
                    if (layoutType.LayoutTypeProperties != null && layoutType.LayoutTypeProperties.Count > 0)
                    {

                        var toRemoveFromClient = layoutType.LayoutTypeProperties.Where(clientProp => context.LayoutTypeProperty.Any(dbProp =>
                         clientProp.LayoutTypeId == dbProp.LayoutTypeId && clientProp.PropertyId == dbProp.PropertyId)).ToList();

                        var layoutTypeProperties = context.LayoutTypeProperty.Where(ctp => ctp.LayoutTypeId == layoutType.Id).ToList();

                        List<LayoutTypeProperty> toRemoveFromDb = null;

                        if (layoutTypeProperties != null && layoutTypeProperties.Count > 0)
                        {
                            toRemoveFromDb = layoutTypeProperties.Where(dbProp => !layoutType.LayoutTypeProperties.Any(clientProp => dbProp.PropertyId == clientProp.PropertyId)).ToList();
                        }

                        if (toRemoveFromClient != null && toRemoveFromClient.Count > 0)
                        {
                            foreach (var layoutTypeProp in toRemoveFromClient)
                            {
                                //ContentTypeProperty exist in db, therefore remove it from contentType (client source)
                                layoutType.LayoutTypeProperties.Remove(layoutTypeProp);
                            }
                        }

                        if (toRemoveFromDb != null && toRemoveFromDb.Count > 0)
                        {
                            //ContentTypeProperty is not exist in contentType (client source), because client has been removed it. Therefor, remove it from db.
                            context.LayoutTypeProperty.RemoveRange(toRemoveFromDb);
                        }
                    }

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
