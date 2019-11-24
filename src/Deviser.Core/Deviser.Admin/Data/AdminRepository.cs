using Deviser.Admin.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;
using Deviser.Core.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Deviser.Core.Common.DomainTypes.Admin;
using Deviser.Detached;
using Deviser.Core.Common.Extensions;
using AutoMapper;

namespace Deviser.Admin.Data
{
    public interface IAdminRepository<TAdminConfigurator>
        where TAdminConfigurator : IAdminConfigurator
    {
        IAdminSite AdminSite { get; }

        IAdminConfig GetAdminConfig(string entityType);
        object GetAllFor(string entityType, int pageNo = 1, int pageSize = Globals.AdminDefaultPageCount, string orderBy = null);
        object GetItemFor(string entityType, string itemId);
        object CreateItemFor(string entityType, object entityObject);
        object UpdateItemFor(string entityType, object entity);
        object DeleteItemFor(string entityType, string itemId);
    }

    public class AdminRepository<TAdminConfigurator> : IAdminRepository<TAdminConfigurator>
        where TAdminConfigurator : IAdminConfigurator
    {
        //Logger
        private readonly ILogger<AdminRepository<TAdminConfigurator>> _logger;

        private readonly DbContext _dbContext;
        private readonly IAdminSite _adminSite;
        private readonly Type _adminConfiguratorType;
        private readonly IAdminSiteProvider _adminSiteProvider;
        private readonly EntityManager _entityManager;
        JsonSerializer _serializer = new JsonSerializer();



        public AdminRepository(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<AdminRepository<TAdminConfigurator>>>();
            _adminSiteProvider = serviceProvider.GetService<IAdminSiteProvider>();

            _adminConfiguratorType = typeof(TAdminConfigurator);
            _adminSite = _adminSiteProvider.GetAdminConfig(_adminConfiguratorType);
            _dbContext = (DbContext)serviceProvider.GetService(_adminSite.DbContextType);
            _entityManager = new EntityManager(_dbContext);
            _serializer.Converters.Add(new Core.Common.Json.GuidConverter());

            if (_adminSite == null)
                throw new InvalidOperationException($"Admin site is not found for type {_adminConfiguratorType}");
        }

        public IAdminSite AdminSite => _adminSite;

        public IAdminConfig GetAdminConfig(string strModelType)
        {
            var modelType = GetModelType(strModelType);
            return GetAdminConfig(modelType);
        }

        public object GetAllFor(string strModelType, int pageNo = 1, int pageSize = Globals.AdminDefaultPageCount, string orderBy = null)
        {
            var modelType = GetModelType(strModelType);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;
            //var genericMethodInfo = typeof(DbContext).GetMethod("Set");
            //var setGenericMethod = genericMethodInfo.MakeGenericMethod(eType);
            //var dbSet = setGenericMethod.Invoke(_dbContext, null);

            //var toListMethodInfo = typeof(Enumerable).GetMethod("ToList");
            //var toListGenericMethod = toListMethodInfo.MakeGenericMethod(eType);
            //var result = toListGenericMethod.Invoke(null, new object[] { dbSet });


            var result = CallGenericMethod(nameof(GetAll), new Type[] { modelType, entityClrType }, new object[] { pageNo, pageSize, orderBy });
            return result;
        }

        public object GetItemFor(string strModelType, string itemId)
        {
            var modelType = GetModelType(strModelType);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;

            var result = CallGenericMethod(nameof(GetItem), new Type[] { modelType, entityClrType }, new object[] { itemId });
            return result;
        }

        public object CreateItemFor(string strModelType, object entity)
        {
            var modelType = GetModelType(strModelType);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;

            var result = CallGenericMethod(nameof(CreateItem), new Type[] { modelType, entityClrType }, new object[] { entity });
            return result;
        }

        public object UpdateItemFor(string strModelType, object entity)
        {
            var modelType = GetModelType(strModelType);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;

            var result = CallGenericMethod(nameof(UpdateItem), new Type[] { modelType, entityClrType }, new object[] { entity });
            return result;
        }

        public object DeleteItemFor(string strModelType, string itemId)
        {
            var modelType = GetModelType(strModelType);
            var adminConfig = GetAdminConfig(modelType);
            var entityClrType = adminConfig.EntityConfig.EntityType.ClrType;

            var result = CallGenericMethod(nameof(DeleteItem), new Type[] { modelType, entityClrType }, new object[] { itemId });
            return result;
        }

        private object CallGenericMethod(string methodName, Type[] genericTypes, object[] parmeters)
        {
            var getItemMethodInfo = typeof(AdminRepository<TAdminConfigurator>).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericTypes);
            var result = getItemMethod.Invoke(this, parmeters);
            return result;
        }


        private PagedResult<TModel> GetAll<TModel, TEntity>(int pageNo, int pageSize, string orderByProperties)
            where TEntity : class
            where TModel : class
        {
            var eType = typeof(TEntity);
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.AsQueryable();

            // Determine the number of records to skip
            int skip = (pageNo - 1) * pageSize;

            // Get total number of records
            int total = dbSet.Count();

            IQueryable<TEntity> query = dbSet;

            //Creates OrderBy/OrderByDescending/ThenBy/ThenByDescending expression based on orderByProperties
            if (!string.IsNullOrEmpty(orderByProperties))
            {
                Expression orderByExpression = queryableData.Expression;
                var props = orderByProperties.Split(',');
                var orderByMethod = "";
                foreach (var prop in props)
                {
                    if (ExpressionHelper.PropertyExists<TEntity>(prop.Replace("-", "")))
                    {
                        string orderByProp = prop;
                        if (string.IsNullOrEmpty(orderByMethod))
                        {
                            if (orderByProp.StartsWith("-"))
                            {
                                orderByProp = orderByProp.Replace("-", "");
                                orderByMethod = nameof(Queryable.OrderByDescending);
                            }
                            else
                            {
                                orderByMethod = nameof(Queryable.OrderBy);
                            }
                        }
                        else
                        {
                            if (orderByProp.StartsWith("-"))
                            {
                                orderByProp = orderByProp.Replace("-", "");
                                orderByMethod = nameof(Queryable.ThenByDescending);
                            }
                            else
                            {
                                orderByMethod = nameof(Queryable.ThenBy);
                            }
                        }

                        orderByExpression = ExpressionHelper.GetOrderByExpression(orderByProp, eType, orderByExpression, orderByMethod);
                    }
                }
                query = queryableData.Provider.CreateQuery<TEntity>(orderByExpression);
            }

            query = query.Skip(skip).Take(pageSize);

            var dbResult = query.ToList();
            var result = _adminSite.Mapper.Map<List<TModel>>(dbResult);

            return new PagedResult<TModel>(result, pageNo, pageSize, total);
        }

        private TModel GetItem<TModel, TEntity>(string itemId)
            where TEntity : class
            where TModel : class
        {
            TEntity dbResult = GetDbItem<TModel, TEntity>(itemId);
            var result = _adminSite.Mapper.Map<TModel>(dbResult);
            return result;
        }

        private TEntity GetDbItem<TModel, TEntity>(string itemId) where TEntity : class
        {
            var modelType = typeof(TModel);
            var entityClrType = typeof(TEntity);
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.AsQueryable();
            var adminConfig = GetAdminConfig(modelType);

            LambdaExpression filterExpression = CreatePrimaryKeyFilter(adminConfig, new List<string> { itemId });

            MethodCallExpression whereCallExpression = ExpressionHelper.GetWhereExpression(entityClrType, queryableData.Expression, filterExpression);

            var query = queryableData.Provider.CreateQuery<TEntity>(whereCallExpression);

            query = AddIncludes(adminConfig, query);

            var dbResult = query.FirstOrDefault();
            return dbResult;
        }

        private IQueryable<TEntity> AddIncludes<TEntity>(IAdminConfig adminConfig, IQueryable<TEntity> query) where TEntity : class
        {
            //ManyToMany Includes
            var m2mFields = GetManyToManyFields(adminConfig);
            var typeMap = _adminSite.GetTypeMapFor(typeof(TEntity));
            foreach (var m2mField in m2mFields)
            {
                var includeString = GetIncludeString(typeMap, m2mField.FieldOption.ReleatedEntityType);
                query = query.Include(includeString);
            }

            //OneToMany Includes
            var m2oFields = GetManyToOneFields(adminConfig);
            foreach (var m2oField in m2oFields)
            {
                var includeString = GetIncludeString(typeMap, m2oField.FieldOption.ReleatedEntityType);
                query = query.Include(includeString);
            }

            //Child Includes            
            //var childFields = GetChildEntityFields<TEntity>();
            foreach (var childConfig in adminConfig.ChildConfigs)
            {
                var childFieldName = GetIncludeString(typeMap, childConfig.Field.FieldClrType); //childConfig.Field.FieldName;

                var cM2mFields = childConfig.FormConfig.AllFormFields.Where(f => f.FieldOption.RelationType == RelationType.ManyToMany).ToList();
                var cM2oFields = childConfig.FormConfig.AllFormFields.Where(f => f.FieldOption.RelationType == RelationType.ManyToOne).ToList();

                query = query.Include(childFieldName);

                //Child ManyToMany Includes
                foreach (var m2mField in cM2mFields)
                {
                    var includeString = $"{childFieldName}.{GetIncludeString(typeMap, m2mField.FieldOption.ReleatedEntityType)}";
                    query = query.Include(includeString);
                }

                //Child OneToMany Includes                
                foreach (var m2oField in cM2oFields)
                {
                    var includeString = $"{childFieldName}.{GetIncludeString(typeMap, m2oField.FieldOption.ReleatedEntityType)}";
                    query = query.Include(includeString);
                }
            }

            return query;
        }

        private string GetIncludeString(TypeMap typeMap, Type relatedEntityType)
        {
            PropertyMap propMap = GetPropertyMapFor(typeMap, relatedEntityType);

            if (propMap.CustomMapExpression == null)
            {
                return propMap.SourceMember.Name;
            }

            var srcExpressions = (propMap.CustomMapExpression.Body as MethodCallExpression)?.Arguments;
            if (srcExpressions != null)
            {
                List<string> includeMembers = new List<string>();
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

        private TModel CreateItem<TModel, TEntity>(object item)
            where TEntity : class
            where TModel : class
        {
            var modelType = typeof(TModel);
            var adminConfig = GetAdminConfig(modelType);


            TModel modelToAdd = ((JObject)item).ToObject<TModel>(_serializer);
            TEntity itemToAdd = _adminSite.Mapper.Map<TEntity>(modelToAdd);

            var m2ofields = GetManyToOneFields(adminConfig);

            SetManyToOneFields(adminConfig, itemToAdd, m2ofields);

            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.Add(itemToAdd);
            _dbContext.SaveChanges();

            var result = _adminSite.Mapper.Map<TModel>(itemToAdd);
            return result;
        }

        private void SetManyToOneFields<TEntity>(IAdminConfig adminConfig, TEntity itemToAdd, List<Field> m2ofields) where TEntity : class
        {
            var entityClrType = typeof(TEntity);
            foreach (var m2oField in m2ofields)
            {
                var entityFieldExpression = GetEntityFieldExpressionFor(entityClrType, m2oField.FieldClrType);
                var fieldPropInfo = ExpressionHelper.GetPropertyInfo(entityFieldExpression);
                var field = fieldPropInfo.GetValue(itemToAdd, null); //item.Category
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

        private TModel UpdateItem<TModel, TEntity>(object item)
            where TEntity : class
            where TModel : class
        {
            var modelType = typeof(TModel);
            var entityClrType = typeof(TEntity);
            var adminConfig = GetAdminConfig(modelType);

            TModel modelToUpdate = ((JObject)item).ToObject<TModel>(_serializer);
            TEntity itemToUpdate = _adminSite.Mapper.Map<TEntity>(modelToUpdate);

            var dbSet = _dbContext.Set<TEntity>();

            List<GraphConfig> graphConfigs = new List<GraphConfig>();

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
                //entity does not have any releations and/or child configs
                dbSet.Update(itemToUpdate);
            }

            _dbContext.SaveChanges();
            var itemId = _entityManager.GetPrimaryKeyValues(itemToUpdate).First().ToString();
            var updatedItem = GetItem<TModel, TEntity>(itemId);
            var result = _adminSite.Mapper.Map<TModel>(updatedItem);
            return result;

        }

        private List<GraphConfig> GetGraphConfigsForReleation(Type entityClrType, IEnumerable<Field> fields)
        {
            var graphConfig = new List<GraphConfig>();            
            foreach (var field in fields)
            {                
                LambdaExpression entityFieldExpression = GetEntityFieldExpressionFor(entityClrType, field.FieldClrType);
                if (entityFieldExpression != null)
                {
                    if (entityFieldExpression.Body.Type.IsCollectionType())
                    {
                        bool isManyToMany = field.FieldOption.RelationType == RelationType.ManyToMany;
                        graphConfig.Add(new GraphConfig
                        {
                            FieldExpression = entityFieldExpression.Body as MemberExpression,
                            GraphConfigType = isManyToMany ? GraphConfigType.OwnedCollection : GraphConfigType.AssociatedCollection
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

        private TModel DeleteItem<TModel, TEntity>(string itemId)
            where TEntity : class
            where TModel : class
        {
            TEntity itemToDelete = GetDbItem<TModel, TEntity>(itemId);
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.Remove(itemToDelete);
            _dbContext.SaveChanges();
            var result = _adminSite.Mapper.Map<TModel>(itemToDelete);
            return result;
        }

        //private MethodInfo GetWhereMethod()
        //{
        //    //typeof(Queryable).GetMethods()[66].GetParameters()[1].ParameterType.GenericTypeArguments[0].GenericTypeArguments.Count()
        //    var methods = typeof(Queryable).GetMethods().Where(m => m.Name == "Where");
        //    foreach (var method in methods)
        //    {
        //        var parameters = method?.GetParameters();
        //        if (parameters.Length > 1 && parameters[1]?.ParameterType.GenericTypeArguments[0]?.GenericTypeArguments.Count() == 2)
        //        {
        //            return method;
        //        }
        //    }
        //    return null;
        //}

        //private MethodInfo GetFirstOrDefaultMethod()
        //{
        //    //typeof(Queryable).GetMethods()[66].GetParameters()[1].ParameterType.GenericTypeArguments[0].GenericTypeArguments.Count()
        //    var methods = typeof(Queryable).GetMethods().Where(m => m.Name == "FirstOrDefault");
        //    foreach (var method in methods)
        //    {
        //        var parameters = method?.GetParameters();
        //        if (parameters.Length > 1 && parameters[1]?.ParameterType.GenericTypeArguments[0]?.GenericTypeArguments.Count() == 2)
        //        {
        //            return method;
        //        }
        //    }
        //    return null;
        //}

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
            var m2mFields = adminConfig.FormConfig.AllFormFields.Where(predicate).ToList();
            return m2mFields;
        }

        private List<Field> GetChildEntityFields(IAdminConfig adminConfig)
        {
            //ManyToMany Includes
            var m2mFields = adminConfig.ChildConfigs.Select(cc => cc.Field).ToList();
            return m2mFields;
        }

        private Type GetModelType(string strModelType)
        {
            return _adminSite.AdminConfigs.Keys.FirstOrDefault(t => t.Name == strModelType);
        }

        private LambdaExpression CreatePrimaryKeyFilter(IAdminConfig adminConfig, List<string> keyValues)
        {
            if (adminConfig == null)
            {
                return null;
            }

            var key = adminConfig.EntityConfig.PrimaryKey;
            //return CreateFilter(key.Properties, keyValues, key.DeclaringEntityType.ClrType);
            return BuildObjectLambda(key.Properties, keyValues, key.DeclaringEntityType.ClrType);
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

        private static Expression GetOrderByExpression(Type parameterType, string propertyName)
        {
            ParameterExpression paramterExpression = Expression.Parameter(parameterType);
            Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
            LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
            return lambda;
        }
    }
}
