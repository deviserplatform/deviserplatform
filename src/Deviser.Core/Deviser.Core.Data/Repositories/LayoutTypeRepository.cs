using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface ILayoutTypeRepository
    {
        LayoutType CreateLayoutType(LayoutType dbLayoutType);
        List<LayoutType> GetLayoutTypes();
        LayoutType GetLayoutType(Guid layoutTypeId);
        LayoutType GetLayoutType(string layoutTypeName);
        LayoutType UpdateLayoutType(LayoutType dbLayoutType);

    }
    public class LayoutTypeRepository : RepositoryBase, ILayoutTypeRepository
    {
        //Logger
        private readonly ILogger<LayoutTypeRepository> _logger;

        //Constructor
        public LayoutTypeRepository(ILifetimeScope container)
            : base(container)
        {
            _logger = container.Resolve<ILogger<LayoutTypeRepository>>();
        }

        public LayoutType CreateLayoutType(LayoutType layoutType)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbLayoutType = Mapper.Map<Entities.LayoutType>(layoutType);
                    dbLayoutType.CreatedDate = dbLayoutType.LastModifiedDate = DateTime.Now;
                    var result = context.LayoutType.Add(dbLayoutType).Entity;
                    context.SaveChanges();
                    return Mapper.Map<LayoutType>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating LayoutType", ex);
            }
            return null;
        }

        public LayoutType GetLayoutType(Guid layoutTypeId)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.LayoutType
                        .Include(lt => lt.LayoutTypeProperties)
                        .FirstOrDefault(e => e.Id == layoutTypeId);

                    return Mapper.Map<LayoutType>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting LayoutType by id", ex);
            }
            return null;
        }

        public LayoutType GetLayoutType(string layoutTypeName)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.LayoutType
                        .Include(lt => lt.LayoutTypeProperties)
                        .FirstOrDefault(e => e.Name.ToLower() == layoutTypeName.ToLower());

                    return Mapper.Map<LayoutType>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting LayoutType by id", ex);
            }
            return null;
        }

        public List<LayoutType> GetLayoutTypes()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.LayoutType
                        .Include(c => c.LayoutTypeProperties).ThenInclude(cp => cp.Property).ThenInclude(p => p.OptionList)
                        .ToList();

                    return Mapper.Map<List<LayoutType>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all LayoutTypes", ex);
            }
            return null;
        }

        public LayoutType UpdateLayoutType(LayoutType layoutType)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbLayoutType = Mapper.Map<Entities.LayoutType>(layoutType);

                    if (dbLayoutType.LayoutTypeProperties != null && dbLayoutType.LayoutTypeProperties.Count > 0)
                    {

                        var toRemoveFromClient = dbLayoutType.LayoutTypeProperties.Where(clientProp => context.LayoutTypeProperty.Any(dbProp =>
                         clientProp.LayoutTypeId == dbProp.LayoutTypeId && clientProp.PropertyId == dbProp.PropertyId)).ToList();

                        var layoutTypeProperties = context.LayoutTypeProperty.Where(ctp => ctp.LayoutTypeId == dbLayoutType.Id).ToList();

                        List<Entities.LayoutTypeProperty> newLayoutTypeProperities = dbLayoutType.LayoutTypeProperties.Where(newProp => context.LayoutTypeProperty.Any(ctp => ctp.LayoutTypeId == newProp.LayoutTypeId && ctp.PropertyId != newProp.PropertyId)).ToList();

                        List<Entities.LayoutTypeProperty> toRemoveFromDb = null;

                        if (layoutTypeProperties != null && layoutTypeProperties.Count > 0)
                        {
                            toRemoveFromDb = layoutTypeProperties.Where(dbProp => !dbLayoutType.LayoutTypeProperties.Any(clientProp => dbProp.PropertyId == clientProp.PropertyId)).ToList();
                        }

                        if (toRemoveFromClient != null && toRemoveFromClient.Count > 0)
                        {
                            foreach (var layoutTypeProp in toRemoveFromClient)
                            {
                                //ContentTypeProperty exist in db, therefore remove it from contentType (client source)
                                dbLayoutType.LayoutTypeProperties.Remove(layoutTypeProp);
                            }
                        }

                        if (toRemoveFromDb != null && toRemoveFromDb.Count > 0)
                        {
                            //ContentTypeProperty is not exist in contentType (client source), because client has been removed it. Therefor, remove it from db.
                            context.LayoutTypeProperty.RemoveRange(toRemoveFromDb);
                        }
                        if (newLayoutTypeProperities != null && newLayoutTypeProperities.Count > 0)
                        {
                            context.LayoutTypeProperty.AddRange(newLayoutTypeProperities);
                        }
                    }

                    dbLayoutType.LastModifiedDate = DateTime.Now;
                    var result = context.LayoutType.Attach(dbLayoutType).Entity;
                    context.Entry(dbLayoutType).State = EntityState.Modified;
                    context.SaveChanges();
                    return Mapper.Map<LayoutType>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating LayoutType", ex);
            }
            return null;
        }
    }
}
