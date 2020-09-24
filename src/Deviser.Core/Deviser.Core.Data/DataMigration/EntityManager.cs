using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.DataMigration
{
    public class EntityManager
    {
        private readonly DbContext _dbContext;
        public EntityManager(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Entity> ExportData()
        {
            var entities = GetOrderedEntities();
            FetchDataForEntities(entities);
            return entities;
        }

        public void ImportData(string json)
        {
            var etypeWithData = JsonConvert.DeserializeObject<List<Entity>>(json);
            var orderedEntityTypes = GetOrderedEntities();

            foreach (var etype in orderedEntityTypes)
            {
                var dataEntity = etypeWithData.FirstOrDefault(e => e.EntityTypeName == etype.EntityTypeName);

                if (dataEntity != null && dataEntity.AllRecords != null)
                {
                    var entityClrType = etype.EntityType.ClrType;
                    var genericMethodInfo = typeof(DbContext).GetMethods().First(m => m.Name == "Add" && m.IsGenericMethod);
                    var addGenericMethod = genericMethodInfo.MakeGenericMethod(entityClrType);

                    foreach (var item in dataEntity.AllRecords)
                    {
                        var targetObj = ((JObject)item).ToObject(entityClrType);
                        addGenericMethod.Invoke(_dbContext, new object[] { targetObj });
                    }
                }
            }

            _dbContext.SaveChanges();
        }

        public string ExportDataAsJson()
        {
            var entities = ExportData();
            return JsonConvert.SerializeObject(entities, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        private void FetchDataForEntities(List<Entity> orderedEntityTypes)
        {
            foreach (var etype in orderedEntityTypes)
            {
                var entityClrType = etype.EntityType.ClrType;
                var genericMethodInfo = typeof(DbContext).GetMethod("Set");
                var setGenericMethod = genericMethodInfo.MakeGenericMethod(entityClrType);
                var dbSet = setGenericMethod.Invoke(_dbContext, null);

                var toListMethodInfo = typeof(Enumerable).GetMethod("ToList");
                var toListGenericMethod = toListMethodInfo.MakeGenericMethod(entityClrType);
                etype.AllRecords = toListGenericMethod.Invoke(null, new object[] { dbSet });
            }


            foreach (var etype in orderedEntityTypes)
            {
                foreach (var item in etype.AllRecords)
                {
                    var propInfos = ((object)item).GetType().GetProperties().ToList();

                    //Skipping navigation properties
                    foreach (var prop in propInfos)
                    {
                        if ((prop.PropertyType.IsGenericType && Nullable.GetUnderlyingType(prop.PropertyType) == null) ||
                            orderedEntityTypes.Any(et => et.EntityType.ClrType == prop.PropertyType))
                        {
                            prop.SetValue(item, null, null);
                        }
                    }
                }
            }
        }
        private List<Entity> GetOrderedEntities()
        {
            var entityTypes = _dbContext.Model.GetEntityTypes();

            var entitesToSorted = new List<Entity>();

            //Add all entity types
            foreach (var entityType in entityTypes)
            {
                entitesToSorted.Add(new Entity
                {
                    EntityType = entityType,
                    SortOrder = 0,
                });
            }

            //Sort entity
            foreach (var entityType in entityTypes)
            {
                var primaryKey = entityType.FindPrimaryKey();
                if (primaryKey != null)
                {
                    var internalPk = ((Microsoft.EntityFrameworkCore.Metadata.Internal.Key)primaryKey);
                    if (internalPk.ReferencingForeignKeys != null && internalPk.ReferencingForeignKeys.Count > 0)
                    {
                        //This key is referenced on other entities
                        foreach (var fk in internalPk.ReferencingForeignKeys)
                        {
                            var fkEntityType = entitesToSorted.Find(e => e.EntityType == fk.DeclaringEntityType);
                            fkEntityType.SortOrder++;
                        }
                    }
                }

                //var primaryKey = keys!=null && keys.Count()>0 ? keys.Where(k=>((Microsoft.EntityFrameworkCore.Metadata.Internal.Key)k).).ToList()

            }



            var orderedEntityTypes = entitesToSorted.OrderBy(e => e.SortOrder).ToList();
            return orderedEntityTypes;
        }
    }
}
