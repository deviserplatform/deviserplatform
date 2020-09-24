using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{
    public class LayoutTypeRepository : ILayoutTypeRepository
    {
        //Logger
        private readonly ILogger<LayoutTypeRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public LayoutTypeRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<LayoutTypeRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        public LayoutType CreateLayoutType(LayoutType layoutType)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbLayoutType = _mapper.Map<Entities.LayoutType>(layoutType);
            dbLayoutType.CreatedDate = dbLayoutType.LastModifiedDate = DateTime.Now;
            var result = context.LayoutType.Add(dbLayoutType).Entity;
            context.SaveChanges();
            return GetLayoutType(result.Id);
        }

        public LayoutType GetLayoutType(Guid layoutTypeId)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.LayoutType
                .Include(lt => lt.LayoutTypeProperties).ThenInclude(lp => lp.Property)
                .FirstOrDefault(e => e.Id == layoutTypeId);

            return _mapper.Map<LayoutType>(result);
        }

        public LayoutType GetLayoutType(string layoutTypeName)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.LayoutType
                .Include(lt => lt.LayoutTypeProperties)
                .FirstOrDefault(e => e.Name.ToLower() == layoutTypeName.ToLower());

            return _mapper.Map<LayoutType>(result);
        }

        public List<LayoutType> GetLayoutTypes()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.LayoutType
                .Include(c => c.LayoutTypeProperties).ThenInclude(cp => cp.Property).ThenInclude(p => p.OptionList)
                .ToList();

            return _mapper.Map<List<LayoutType>>(result);
        }

        public LayoutType UpdateLayoutType(LayoutType layoutType)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbLayoutType = _mapper.Map<Entities.LayoutType>(layoutType);

            if (dbLayoutType.LayoutTypeProperties != null && dbLayoutType.LayoutTypeProperties.Count > 0)
            {

                var toRemoveFromClient = dbLayoutType.LayoutTypeProperties.Where(clientProp => context.LayoutTypeProperty.Any(dbProp =>
                    clientProp.LayoutTypeId == dbProp.LayoutTypeId && clientProp.PropertyId == dbProp.PropertyId)).ToList();

                var layoutTypeProperties = context.LayoutTypeProperty.Where(ctp => ctp.LayoutTypeId == dbLayoutType.Id).ToList();

                //List<Entities.LayoutTypeProperty> newLayoutTypeProperities = dbLayoutType.LayoutTypeProperties.Where(newProp => context.LayoutTypeProperty.Any(ctp => ctp.LayoutTypeId == newProp.LayoutTypeId && ctp.PropertyId != newProp.PropertyId)).ToList();

                List<Entities.LayoutTypeProperty> toRemoveFromDb = null;

                if (layoutTypeProperties.Count > 0)
                {
                    toRemoveFromDb = layoutTypeProperties.Where(dbProp => !dbLayoutType.LayoutTypeProperties.Any(clientProp => dbProp.PropertyId == clientProp.PropertyId)).ToList();
                }

                if (toRemoveFromClient.Count > 0)
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
                if (dbLayoutType.LayoutTypeProperties != null && dbLayoutType.LayoutTypeProperties.Count > 0)
                {
                    context.LayoutTypeProperty.AddRange(dbLayoutType.LayoutTypeProperties);
                }
            }

            dbLayoutType.LastModifiedDate = DateTime.Now;
            var result = context.LayoutType.Update(dbLayoutType).Entity;
            context.SaveChanges();
            return GetLayoutType(result.Id);

        }
    }
}
