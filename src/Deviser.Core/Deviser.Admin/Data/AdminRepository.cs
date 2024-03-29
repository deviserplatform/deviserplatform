﻿using AutoMapper;
using Deviser.Admin.Config;
using Deviser.Core.Common.Extensions;
using Deviser.Detached;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Extensions;
using Deviser.Admin.Internal;

namespace Deviser.Admin.Data
{
    public interface IAdminRepository

    {
        Task<PagedResult<TModel>> GetAllFor<TModel>(int pageNo, int pageSize, string orderByProperties, FilterNode filters = null) where TModel : class;
        Task<TModel> GetItemFor<TModel>(string itemId) where TModel : class;
        Task<TModel> CreateItemFor<TModel>(TModel item) where TModel : class;
        Task<TModel> UpdateItemFor<TModel>(TModel item) where TModel : class;
        Task<TModel> DeleteItemFor<TModel>(string itemId) where TModel : class;
        Task<PagedResult<TModel>> SortItemsFor<TModel>(int pageNo, int pageSize, IList<TModel> items) where TModel : class;
    }

    public class AdminRepository : IAdminRepository
    {
        //Logger
        private readonly ILogger<AdminRepository> _logger;

        private readonly DbContext _dbContext;
        private readonly IAdminSite _adminSite;
        private readonly Type _adminConfiguratorType;
        private readonly IAdminSiteProvider _adminSiteProvider;
        private readonly EntityManager _entityManager;
        JsonSerializer _serializer = new JsonSerializer();



        public AdminRepository(IAdminSite adminSite, IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<AdminRepository>>();
            _adminSite = adminSite;

            if (_adminSite == null)
                throw new InvalidOperationException($"Admin site is not found for type {_adminConfiguratorType}");

            _dbContext = (DbContext)serviceProvider.GetService(_adminSite.DbContextType);
            _entityManager = new EntityManager(_dbContext);
            _serializer.Converters.Add(new Core.Common.Json.GuidConverter());
        }

        public async Task<PagedResult<TModel>> GetAllFor<TModel>(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
            where TModel : class
        {
            var modelType = typeof(TModel);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;

            var result = await CallGenericMethod<PagedResult<TModel>>(nameof(GetAll), new Type[] { modelType, entityClrType }, new object[] { pageNo, pageSize, orderByProperties, filter });
            return result;
        }

        public async Task<TModel> GetItemFor<TModel>(string itemId)
            where TModel : class
        {
            var modelType = typeof(TModel);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;

            var result = await CallGenericMethod<TModel>(nameof(GetItem), new Type[] { modelType, entityClrType }, new object[] { itemId });
            return result;
        }

        public async Task<TModel> CreateItemFor<TModel>(TModel item)
            where TModel : class
        {
            var modelType = typeof(TModel);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;

            var result = await CallGenericMethod<TModel>(nameof(CreateItem), new Type[] { modelType, entityClrType }, new object[] { item });
            return result;
        }

        public async Task<TModel> UpdateItemFor<TModel>(TModel item)
            where TModel : class
        {
            var modelType = typeof(TModel);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;

            var result = await CallGenericMethod<TModel>(nameof(UpdateItem), new Type[] { modelType, entityClrType }, new object[] { item });
            return result;
        }

        public async Task<TModel> DeleteItemFor<TModel>(string itemId)
            where TModel : class
        {
            var modelType = typeof(TModel);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;

            var result = await CallGenericMethod<TModel>(nameof(DeleteItem), new Type[] { modelType, entityClrType }, new object[] { itemId });
            return result;
        }

        public async Task<PagedResult<TModel>> SortItemsFor<TModel>(int pageNo, int pageSize, IList<TModel> items) where TModel : class
        {
            var modelType = typeof(TModel);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;

            var result = await CallGenericMethod<PagedResult<TModel>>(nameof(SortItems), new Type[] { modelType, entityClrType }, new object[] { pageNo, pageSize, items });
            return result;
        }

        private async Task<TResult> CallGenericMethod<TResult>(string methodName, Type[] genericTypes, object[] parmeters)
            where TResult : class
        {
            var getItemMethodInfo = typeof(AdminRepository).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericTypes);
            var result = (Task<TResult>)getItemMethod.Invoke(this, parmeters);
            return await result;
            //return await CallGenericMethodOf<TResult>(typeof(AdminRepository), this, methodName, genericTypes, parmeters);
        }

        private async Task CallGenericMethod(string methodName, Type[] genericTypes, object[] parmeters)
        {
            var getItemMethodInfo = typeof(AdminRepository).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericTypes);
            var task = (Task)getItemMethod.Invoke(this, parmeters);
            await task;
        }

        //private async Task<TResult> CallGenericMethodOf<TResult>(Type classType, object classObject, string methodName, Type[] genericTypes, object[] parmeters)
        //    where TResult : class
        //{
        //    var getItemMethodInfo = classType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        //    var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericTypes);
        //    var result = (Task<TResult>)getItemMethod.Invoke(classObject, parmeters);
        //    return await result;
        //}


        private async Task<PagedResult<TModel>> GetAll<TModel, TEntity>(int pageNo, int pageSize, string orderByProperties, FilterNode filter)
            where TEntity : class
            where TModel : class
        {
            var modelType = typeof(TModel);
            var eType = typeof(TEntity);
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.AsQueryable();
            var adminConfig = GetAdminConfig(modelType);

            // Determine the number of records to skip
            var skip = (pageNo - 1) * pageSize;

            // Get total number of records
            var total = dbSet.Count();

            IQueryable<TEntity> query = dbSet;

            query = AddIncludes(adminConfig, query);

            query = query.Skip(skip).Take(pageSize);

            var dbResult = await query.ToListAsync();
            var result = _adminSite.Mapper.Map<List<TModel>>(dbResult);

            //Filter and Sorting are performed in-memory since EF could not translate all expressions to SQL
            if (!string.IsNullOrEmpty(orderByProperties))
            {
                result = result.SortBy(orderByProperties).ToList();
            }

            if (filter != null && filter.ChildNodes.Count > 0)
            {
                result = result.ApplyFilter(filter).ToList();
            }

            return new PagedResult<TModel>(result, pageNo, pageSize, total);
        }

        private async Task<TModel> GetItem<TModel, TEntity>(string itemId)
            where TEntity : class
            where TModel : class
        {
            var dbResult = await GetDbItem<TModel, TEntity>(itemId);
            var result = _adminSite.Mapper.Map<TModel>(dbResult);
            return result;
        }

        private async Task<TModel> CreateItem<TModel, TEntity>(TModel item)
            where TEntity : class
            where TModel : class
        {
            var modelType = typeof(TModel);
            var adminConfig = GetAdminConfig(modelType);


            var modelToAdd = item;//((JObject)item).ToObject<TModel>(_serializer);
            var itemToAdd = _adminSite.Mapper.Map<TEntity>(modelToAdd);

            var m2ofields = GetManyToOneFields(adminConfig);

            SetManyToOneFields(adminConfig, itemToAdd, m2ofields);

            //ManyToMany Includes
            var m2mFields = GetManyToManyFields(adminConfig);

            await SetM2mReferences(itemToAdd, m2mFields);

            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.Add(itemToAdd);
            await _dbContext.SaveChangesAsync();

            var result = _adminSite.Mapper.Map<TModel>(itemToAdd);
            return result;
        }

        private async Task<TModel> UpdateItem<TModel, TEntity>(TModel item)
            where TEntity : class
            where TModel : class
        {
            var modelType = typeof(TModel);
            var entityClrType = typeof(TEntity);
            var adminConfig = GetAdminConfig(modelType);

            var modelToUpdate = item;//((JObject)item).ToObject<TModel>(_serializer);
            var itemToUpdate = _adminSite.Mapper.Map<TEntity>(modelToUpdate);

            var dbSet = _dbContext.Set<TEntity>();

            var graphConfigs = new List<GraphConfig>();

            var navigationFields = GetFieldsFor(adminConfig, f => f.FieldOption.RelationType == RelationType.ManyToMany || f.FieldOption.RelationType == RelationType.ManyToOne);
            if (navigationFields != null && navigationFields.Count > 0)
            {
                var releatedGraphConfigs = GetGraphConfigsForReleation(entityClrType, navigationFields);
                graphConfigs.AddRange(releatedGraphConfigs);
            }


            var childConfigFields = GetChildEntityFields(adminConfig);
            if (childConfigFields != null && childConfigFields.Count > 0)
            {
                var childGraphConfigs = GetGraphConfigsForChildEntities(entityClrType, childConfigFields);
                graphConfigs.AddRange(childGraphConfigs);
                //TODO: include releated graph configs for each childConfigField. This needs changes in Deviser.Detached as well
            }

            if (graphConfigs.Count > 0)
            {
                //entity have releations and/or child configs
                var queryableData = _dbContext.UpdateGraph(itemToUpdate, graphConfigs);
            }
            else
            {
                //entity does not have any releations and / or child configs
                dbSet.Update(itemToUpdate);
            }

            await _dbContext.SaveChangesAsync();
            var itemId = _entityManager.GetPrimaryKeyValues(itemToUpdate).First().ToString();
            var updatedItem = await GetItem<TModel, TEntity>(itemId);
            var result = _adminSite.Mapper.Map<TModel>(updatedItem);
            return result;

        }

        private async Task<TModel> DeleteItem<TModel, TEntity>(string itemId)
            where TEntity : class
            where TModel : class
        {
            var itemToDelete = await GetDbItem<TModel, TEntity>(itemId);
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.Remove(itemToDelete);
            await _dbContext.SaveChangesAsync();
            var result = _adminSite.Mapper.Map<TModel>(itemToDelete);
            return result;
        }

        private async Task<TEntity> GetDbItem<TModel, TEntity>(string itemId) where TEntity : class
        {
            var modelType = typeof(TModel);
            var entityClrType = typeof(TEntity);
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.AsQueryable();
            var adminConfig = GetAdminConfig(modelType);

            var filterExpression = CreatePrimaryKeyFilter(adminConfig.EntityConfig.PrimaryKey, new List<string> { itemId });

            var whereCallExpression = ExpressionHelper.GetWhereExpression(entityClrType, queryableData.Expression, filterExpression);

            var query = queryableData.Provider.CreateQuery<TEntity>(whereCallExpression);

            query = AddIncludes(adminConfig, query);

            var dbResult = await query.FirstOrDefaultAsync();
            return dbResult;
        }

        private void SetManyToOneFields<TEntity>(IAdminConfig adminConfig, TEntity itemToAdd, List<Field> m2ofields) where TEntity : class
        {
            var entityClrType = typeof(TEntity);
            foreach (var m2oField in m2ofields)
            {
                var entityFieldExpression = GetEntityFieldExpressionFor(entityClrType, m2oField.FieldClrType);
                var fieldPropInfo = ExpressionHelper.GetPropertyInfo(entityFieldExpression);
                var field = fieldPropInfo.GetValue(itemToAdd, null); //item.Category

                if (field == null)
                {
                    continue;
                }

                var fieldType = fieldPropInfo.PropertyType;
                var fieldEntityType = _dbContext.Model.FindEntityType(fieldType);
                var fieldPK = fieldEntityType.FindPrimaryKey();

                var pkPropInfo = fieldPK.Properties.First().PropertyInfo; //Assuming the entity has only one primary key
                var id = pkPropInfo.GetValue(field, null); //item.Category.Id

                var targetPropInfo = fieldEntityType.GetNavigations().First().ForeignKey.Properties.First().PropertyInfo;

                targetPropInfo.SetValue(itemToAdd, id, null); //item.CategoryId = item.Category.Id

                fieldPropInfo.SetValue(itemToAdd, null, null); //item.Category = null;
            }
        }

        private async Task SetM2mReferences<TEntity>(TEntity itemToAdd, List<Field> m2mFields)
        {
            foreach (var m2MField in m2mFields)
            {
                var relatedModelType = m2MField.FieldOption.LookupModelType;
                var propEntityClrType = _adminSite.GetEntityClrTypeFor(relatedModelType);
                await CallGenericMethod(nameof(SetM2mReferencesFor), new Type[] { typeof(TEntity), propEntityClrType }, new object[] { itemToAdd, m2MField });
            }

        }

        private async Task SetM2mReferencesFor<TEntity, TPropEntity>(TEntity itemToAdd, Field m2mField)
            where TEntity : class
            where TPropEntity : class
        {
            var primaryKeyExpressions = AdminSite.GetPrimaryKeyStringExpressions<TPropEntity>(_dbContext);
            var dictionary = await GetItemsAsDict(primaryKeyExpressions);

            var propInfo = typeof(TEntity).GetProperties().First(p => p.Name == m2mField.FieldName);

            if (propInfo?.GetValue(itemToAdd) is IList<TPropEntity> propCollection)
            {
                for (var i = 0; i < propCollection.Count; i++)
                {
                    var propEntity = propCollection[i];
                    var key = GetPrimaryKeyValue(primaryKeyExpressions, propEntity);
                    propCollection[i] = dictionary[key];
                }
            }

            //var dbTags = await _dbContext.Tags.ToDictionaryAsync(t => t.Id, t => t);
            //var tags = entity.Tags as List<Models.Tag>;
            //for (int i = 0; i < tags.Count; i++)
            //{
            //    tags[i] = dbTags[tags[i].Id];
            //}
        }

        private async Task<IDictionary<string, TEntity>> GetItemsAsDict<TEntity>(List<Expression<Func<TEntity, string>>> pkExpressions)
            where TEntity : class
        {
            var result = await _dbContext.Set<TEntity>().ToDictionaryAsync(e => GetPrimaryKeyValue(pkExpressions, e));
            return result;
        }

        private static string GetPrimaryKeyValue<TEntity>(List<Expression<Func<TEntity, string>>> pkExpressions, TEntity e) where TEntity : class
        {
            var keys = new List<string>();
            foreach (var pkExpression in pkExpressions)
            {
                var memberName = ((pkExpression.Body as MethodCallExpression).Object as MemberExpression).Member.Name;
                var del = pkExpression.Compile();
                keys.Add($"{memberName}:{del(e)}");
            }

            return string.Join('|', keys);
        }

        private async Task<PagedResult<TModel>> SortItems<TModel, TEntity>(int pageNo, int pageSize, IList<TModel> items)
            where TEntity : class
            where TModel : class
        {
            var modelType = typeof(TModel);
            var eType = typeof(TEntity);
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.AsQueryable();
            var adminConfig = GetAdminConfig(modelType);

            // Determine the number of records to skip
            var skip = (pageNo - 1) * pageSize;

            // Get total number of records
            var total = dbSet.Count();

            IQueryable<TEntity> query = dbSet;

            query = AddIncludes(adminConfig, query);

            query = query.Skip(skip).Take(pageSize);


            var uiSorting = items.ToDictionary(k => modelType.GetProperty(adminConfig.ModelConfig.KeyField.FieldName).GetValue(k).ToString(),
                v => (int)modelType.GetProperty(adminConfig.ModelConfig.GridConfig.SortField.FieldName).GetValue(v));
            var dbResult = await query.ToListAsync();

            foreach (var item in dbResult)
            {
                var sortOrder = uiSorting[modelType.GetProperty(adminConfig.ModelConfig.KeyField.FieldName).GetValue(item).ToString()];
                modelType.GetProperty(adminConfig.ModelConfig.GridConfig.SortField.FieldName).SetValue(item, sortOrder);
            }

            _dbContext.SaveChanges();

            var result = _adminSite.Mapper.Map<List<TModel>>(dbResult);
            return new PagedResult<TModel>(result, pageNo, pageSize, total);
        }

        private IQueryable<TEntity> AddIncludes<TEntity>(IAdminConfig adminConfig, IQueryable<TEntity> query) where TEntity : class
        {
            //ManyToMany Includes
            var m2mFields = GetManyToManyFields(adminConfig);
            var typeMap = _adminSite.GetTypeMapFor(typeof(TEntity));
            foreach (var m2mField in m2mFields)
            {
                var includeString = GetIncludeString(typeMap, m2mField.FieldOption.LookupModelType);
                query = query.Include(includeString);
            }

            //OneToMany Includes
            var m2oFields = GetManyToOneFields(adminConfig);
            foreach (var m2oField in m2oFields)
            {
                var includeString = GetIncludeString(typeMap, m2oField.FieldOption.LookupModelType);
                query = query.Include(includeString);
            }

            //Child Includes            
            //var childFields = GetChildEntityFields<TEntity>();
            foreach (var childConfig in adminConfig.ChildConfigs)
            {
                var childFieldName = GetIncludeString(typeMap, childConfig.Field.FieldClrType); //childConfig.Field.FieldName;

                var cM2mFields = childConfig.ModelConfig.FormConfig.AllFields.Where(f => f.FieldOption.RelationType == RelationType.ManyToMany).ToList();
                var cM2oFields = childConfig.ModelConfig.FormConfig.AllFields.Where(f => f.FieldOption.RelationType == RelationType.ManyToOne).ToList();

                query = query.Include(childFieldName);

                //Child ManyToMany Includes
                foreach (var m2mField in cM2mFields)
                {
                    var includeString = $"{childFieldName}.{GetIncludeString(typeMap, m2mField.FieldOption.LookupModelType)}";
                    query = query.Include(includeString);
                }

                //Child OneToMany Includes                
                foreach (var m2oField in cM2oFields)
                {
                    var includeString = $"{childFieldName}.{GetIncludeString(typeMap, m2oField.FieldOption.LookupModelType)}";
                    query = query.Include(includeString);
                }
            }

            return query;
        }

        private string GetIncludeString(TypeMap typeMap, Type relatedEntityType)
        {
            var propMap = GetPropertyMapFor(typeMap, relatedEntityType);

            if (propMap.CustomMapExpression == null)
            {
                return propMap.SourceMember.Name;
            }

            var srcExpressions = (propMap.CustomMapExpression.Body as MethodCallExpression)?.Arguments;
            if (srcExpressions != null)
            {
                var includeMembers = new List<string>();
                foreach (var expr in srcExpressions)
                {
                    if (expr is MemberExpression me)
                    {
                        includeMembers.Add(me.Member.Name);
                    }
                    else if (expr is LambdaExpression lambdaExpression && lambdaExpression.Body is MemberExpression leMe)
                    {
                        includeMembers.Add(leMe.Member.Name);
                    }
                }
                var includeString = string.Join(".", includeMembers);
                return includeString;
            }

            return string.Empty;
        }

        private static PropertyMap GetPropertyMapFor(TypeMap typeMap, Type destinationPropType)
        {
            return typeMap.PropertyMaps.FirstOrDefault(pm => pm.DestinationType == destinationPropType || (pm.DestinationType.IsGenericType && pm.DestinationType.GenericTypeArguments.First() == destinationPropType));
        }

        private List<GraphConfig> GetGraphConfigsForReleation(Type entityClrType, IEnumerable<Field> fields)
        {
            var graphConfig = new List<GraphConfig>();
            foreach (var field in fields)
            {
                var entityFieldExpression = GetEntityFieldExpressionFor(entityClrType, field.FieldClrType);
                if (entityFieldExpression != null)
                {
                    if (entityFieldExpression.Body.Type.IsCollectionType())
                    {
                        var isManyToMany = field.FieldOption.RelationType == RelationType.ManyToMany;
                        graphConfig.Add(new GraphConfig
                        {
                            FieldExpression = entityFieldExpression.Body as MemberExpression,
                            GraphConfigType = GraphConfigType.AssociatedCollection
                        });
                    }
                    else
                    {
                        graphConfig.Add(new GraphConfig
                        {
                            FieldExpression = entityFieldExpression.Body as MemberExpression,
                            GraphConfigType = GraphConfigType.AssociatedEntity
                        });
                    }
                }


            }
            return graphConfig;
        }

        private LambdaExpression GetEntityFieldExpressionFor(Type entityClrType, Type fieldClrType)
        {
            var typeMap = _adminSite.GetTypeMapFor(entityClrType);
            var propertyMap = GetPropertyMapFor(typeMap, fieldClrType);
            LambdaExpression entityFieldExpression = null; //ExpressionHelper.GetPropertyExpression(entityClrType, fieldTypeMap.DestinationType.Name);

            if (propertyMap.CustomMapExpression == null)
            {
                entityFieldExpression = ExpressionHelper.GetPropertyExpression(entityClrType, propertyMap.SourceMember.Name);
            }
            else
            {
                var srcExpression = (propertyMap.CustomMapExpression.Body as MethodCallExpression)?.Arguments.FirstOrDefault(expr => expr is MemberExpression) as MemberExpression;
                entityFieldExpression = ExpressionHelper.GetPropertyExpression(entityClrType, srcExpression.Member.Name);
            }

            return entityFieldExpression;
        }

        private List<GraphConfig> GetGraphConfigsForChildEntities(Type entityClrType, IEnumerable<Field> fields)
        {
            var graphConfig = new List<GraphConfig>();
            foreach (var field in fields)
            {
                var fieldTypeMap = _adminSite.GetTypeMapFor(field.FieldClrType);
                if (fieldTypeMap != null)
                {
                    var entityFieldExpression = ExpressionHelper.GetPropertyExpression(entityClrType, fieldTypeMap.DestinationType.Name);

                    if (entityFieldExpression.Body.Type.IsCollectionType())
                    {
                        graphConfig.Add(new GraphConfig
                        {
                            FieldExpression = entityFieldExpression.Body as MemberExpression,
                            GraphConfigType = GraphConfigType.OwnedCollection
                        });
                    }
                    else
                    {
                        graphConfig.Add(new GraphConfig
                        {
                            FieldExpression = entityFieldExpression.Body as MemberExpression,
                            GraphConfigType = GraphConfigType.OwnedEntity
                        });
                    }
                }


            }
            return graphConfig;
        }

        private IAdminConfig GetAdminConfig(Type eType)
        {
            IAdminConfig adminConfig;
            if (_adminSite.AdminConfigs.TryGetValue(eType, out adminConfig))
            {
                return adminConfig;
            }
            return null;
        }

        private List<Field> GetManyToManyFields(IAdminConfig adminConfig)
        {
            return GetFieldsFor(adminConfig, f => f.FieldOption.RelationType == RelationType.ManyToMany);
        }

        private List<Field> GetManyToOneFields(IAdminConfig adminConfig)
        {
            return GetFieldsFor(adminConfig, f => f.FieldOption.RelationType == RelationType.ManyToOne);
        }

        private List<Field> GetFieldsFor(IAdminConfig adminConfig, Func<Field, bool> predicate)
        {
            //ManyToMany Includes
            var m2mFields = adminConfig.ModelConfig.FormConfig.AllFields.Where(predicate).ToList();
            return m2mFields;
        }

        private List<Field> GetChildEntityFields(IAdminConfig adminConfig)
        {
            //ManyToMany Includes
            var m2mFields = adminConfig.ChildConfigs.Select(cc => cc.Field).ToList();
            return m2mFields;
        }

        public static LambdaExpression CreatePrimaryKeyFilter(IKey primaryKey, List<string> keyValues)
        {
            if (primaryKey == null)
            {
                return null;
            }
            //return CreateFilter(key.Properties, keyValues, key.DeclaringEntityType.ClrType);
            return BuildObjectLambda(primaryKey.Properties, keyValues, primaryKey.DeclaringEntityType.ClrType);
        }

        private static LambdaExpression BuildObjectLambda(IReadOnlyList<IProperty> keyProperties, List<string> keyValues, Type entityClrType)
        {
            var entityParameter = Expression.Parameter(entityClrType, "e");

            return Expression.Lambda(
                BuildPredicate(keyProperties, keyValues, entityParameter), entityParameter);
        }

        private static BinaryExpression BuildPredicate(
           IReadOnlyList<IProperty> keyProperties,
           List<string> keyValues,
           ParameterExpression entityParameter)
        {
            BinaryExpression predicate = null;
            for (var i = 0; i < keyProperties.Count; i++)
            {
                var keyProperty = keyProperties[i];
                var strKeyValue = keyValues[i];
                var keyValue = TypeDescriptor.GetConverter(keyProperty.ClrType).ConvertFromInvariantString(strKeyValue);

                var left = Expression.Property(entityParameter, keyProperty.PropertyInfo);

                var right = Expression.Constant(keyValue);

                var equalsExpression = Expression.Equal(left, right);

                predicate = predicate == null ? equalsExpression : Expression.AndAlso(predicate, equalsExpression);
            }
            return predicate;
        }
    }
}
