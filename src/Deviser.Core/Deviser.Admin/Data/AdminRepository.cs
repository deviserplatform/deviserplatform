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

namespace Deviser.Admin.Data
{
    public interface IAdminRepository<TAdminConfigurator>
        where TAdminConfigurator : IAdminConfigurator
    {
        IAdminSite AdminConfig { get; }

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
        JsonSerializer _serializer = new JsonSerializer();



        public AdminRepository(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<AdminRepository<TAdminConfigurator>>>();
            _adminSiteProvider = serviceProvider.GetService<IAdminSiteProvider>();

            _adminConfiguratorType = typeof(TAdminConfigurator);
            _adminSite = _adminSiteProvider.GetAdminConfig(_adminConfiguratorType);
            _dbContext = (DbContext)serviceProvider.GetService(_adminSite.DbContextType);
            _serializer.Converters.Add(new Core.Common.Json.GuidConverter());

            if (_adminSite == null)
                throw new InvalidOperationException($"Admin site is not found for type {_adminConfiguratorType}");
        }

        public IAdminSite AdminConfig => _adminSite;

        public IAdminConfig GetAdminConfig(string entityType)
        {
            var eType = GetEntityType(entityType);
            return GetAdminConfig(eType);
        }

        public object GetAllFor(string entityType, int pageNo = 1, int pageSize = Globals.AdminDefaultPageCount, string orderBy = null)
        {
            var eType = GetEntityType(entityType);

            //var genericMethodInfo = typeof(DbContext).GetMethod("Set");
            //var setGenericMethod = genericMethodInfo.MakeGenericMethod(eType);
            //var dbSet = setGenericMethod.Invoke(_dbContext, null);

            //var toListMethodInfo = typeof(Enumerable).GetMethod("ToList");
            //var toListGenericMethod = toListMethodInfo.MakeGenericMethod(eType);
            //var result = toListGenericMethod.Invoke(null, new object[] { dbSet });


            var result = CallGenericMethod(nameof(GetAll), eType, new object[] { pageNo, pageSize, orderBy });

            return result;
        }

        public object GetItemFor(string entityType, string itemId)
        {
            var eType = GetEntityType(entityType);
            var result = CallGenericMethod(nameof(GetItem), eType, new object[] { itemId });
            return result;
        }

        public object CreateItemFor(string entityType, object entity)
        {
            var eType = GetEntityType(entityType);
            var result = CallGenericMethod(nameof(CreateItem), eType, new object[] { entity });
            return result;
        }

        public object UpdateItemFor(string entityType, object entity)
        {
            var eType = GetEntityType(entityType);
            var result = CallGenericMethod(nameof(UpdateItem), eType, new object[] { entity });
            return result;
        }

        public object DeleteItemFor(string entityType, string itemId)
        {
            var eType = GetEntityType(entityType);
            var result = CallGenericMethod(nameof(DeleteItem), eType, new object[] { itemId });
            return result;
        }

        private object CallGenericMethod(string methodName, Type genericType, object[] parmeters)
        {
            var getItemMethodInfo = typeof(AdminRepository<TAdminConfigurator>).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericType);
            var result = getItemMethod.Invoke(this, parmeters);
            return result;
        }


        private PagedResult<TEntity> GetAll<TEntity>(int pageNo, int pageSize, string orderByProperties)
            where TEntity : class
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

            var result = query.ToList();

            return new PagedResult<TEntity>(result, pageNo, pageSize, total);
        }

        private TEntity GetItem<TEntity>(string itemId)
            where TEntity : class
        {
            var eType = typeof(TEntity);

            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.AsQueryable();

            LambdaExpression filterExpression = CreatePrimaryKeyFilter(new List<string> { itemId }, eType);

            MethodCallExpression whereCallExpression = ExpressionHelper.GetWhereExpression(eType, queryableData.Expression, filterExpression);

            var query = queryableData.Provider.CreateQuery<TEntity>(whereCallExpression);

            query = AddIncludes(query);

            var result = query.FirstOrDefault();
            return result;
        }

        private IQueryable<TEntity> AddIncludes<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            //ManyToMany Includes
            var m2mFields = GetManyToManyFields<TEntity>();
            foreach (var m2mField in m2mFields)
            {
                var includeString = $"{m2mField.FieldName}.{m2mField.FieldOption.ReleatedEntityType.Name}";
                query = query.Include(includeString);
            }

            //OneToMany Includes
            var m2oFields = GetManyToOneFields<TEntity>();
            foreach (var m2oField in m2oFields)
            {
                query = query.Include(m2oField.FieldName);
            }

            //Child Includes
            var eType = typeof(TEntity);
            var adminConfig = GetAdminConfig(eType);

            //var childFields = GetChildEntityFields<TEntity>();
            foreach (var childConfig in adminConfig.ChildConfigs)
            {
                var childFieldName = childConfig.Field.FieldName;//$"{m2mField.FieldName}.{m2mField.FieldOption.ReleatedEntityType.Name}";

                var cM2mFields = childConfig.FormConfig.AllFormFields.Where(f => f.FieldOption.RelationType == RelationType.ManyToMany).ToList();
                var cM2oFields = childConfig.FormConfig.AllFormFields.Where(f => f.FieldOption.RelationType == RelationType.ManyToOne).ToList();

                query = query.Include(childFieldName);

                //Child ManyToMany Includes
                foreach (var m2mField in cM2mFields)
                {
                    var includeString = $"{childFieldName}.{m2mField.FieldName}.{m2mField.FieldOption.ReleatedEntityType.Name}";
                    query = query.Include(includeString);
                }

                //Child OneToMany Includes                
                foreach (var m2oField in cM2oFields)
                {
                    var includeString = $"{childFieldName}.{m2oField.FieldName}";
                    query = query.Include(includeString);
                }
            }

            return query;
        }

        private TEntity CreateItem<TEntity>(object item)
            where TEntity : class
        {
            var eType = typeof(TEntity);

            TEntity itemToAdd = ((JObject)item).ToObject<TEntity>(_serializer);

            var m2ofields = GetManyToOneFields<TEntity>();

            SetManyToOneFields(itemToAdd, m2ofields);

            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.Add(itemToAdd);
            _dbContext.SaveChanges();
            return itemToAdd;
        }

        private void SetManyToOneFields<TEntity>(TEntity itemToAdd, List<Field> m2ofields) where TEntity : class
        {
            foreach (var m2oField in m2ofields)
            {
                var fieldPropInfo = ExpressionHelper.GetPropertyInfo(m2oField.FieldExpression);
                var field = fieldPropInfo.GetValue(itemToAdd, null); //item.Category
                var fieldType = fieldPropInfo.PropertyType;
                var fieldEntityType = _dbContext.Model.FindEntityType(fieldType);
                var fieldPK = fieldEntityType.FindPrimaryKey();

                var pkPropInfo = fieldPK.Properties.First().PropertyInfo; //Assuming the entity has only one primary key
                var id = pkPropInfo.GetValue(field, null); //item.Category.Id

                var targetPropInfo = ExpressionHelper.GetPropertyInfo(m2oField.FieldOption.ReleatedFields.First().FieldExpression);

                targetPropInfo.SetValue(itemToAdd, id, null); //item.CategoryId = item.Category.Id

                fieldPropInfo.SetValue(itemToAdd, null, null); //item.Category = null;
            }
        }

        private TEntity UpdateItem<TEntity>(object item)
            where TEntity : class
        {
            var eType = typeof(TEntity);
            TEntity itemToUpdate = ((JObject)item).ToObject<TEntity>(_serializer);
            var dbSet = _dbContext.Set<TEntity>();

            List<GraphConfig> graphConfigs = new List<GraphConfig>();

            var navigationFields = GetFieldsFor<TEntity>(f => f.FieldOption.RelationType == RelationType.ManyToMany || f.FieldOption.RelationType == RelationType.ManyToOne);
            if (navigationFields != null && navigationFields.Count > 0)
            {
                var releatedGraphConfigs = GetGraphConfigsForReleation(navigationFields);
                graphConfigs.AddRange(releatedGraphConfigs);
            }


            var childConfigFields = GetChildEntityFields<TEntity>();
            if (childConfigFields != null && childConfigFields.Count > 0)
            {
                var childGraphConfigs = GetGraphConfigsForChildEntities(childConfigFields);
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
            return itemToUpdate;

        }

        private List<GraphConfig> GetGraphConfigsForReleation(IEnumerable<Field> fields)
        {
            var graphConfig = new List<GraphConfig>();
            foreach (var field in fields)
            {
                if (field.FieldExpression.Body.Type.IsCollectionType())
                {
                    bool isManyToMany = field.FieldOption.RelationType == RelationType.ManyToMany;
                    graphConfig.Add(new GraphConfig
                    {
                        FieldExpression = field.FieldExpression.Body as MemberExpression,
                        GraphConfigType = isManyToMany ? GraphConfigType.OwnedCollection : GraphConfigType.AssociatedCollection
                    });
                }
                else
                {
                    graphConfig.Add(new GraphConfig
                    {
                        FieldExpression = field.FieldExpression.Body as MemberExpression,
                        GraphConfigType = GraphConfigType.AssociatedEntity
                    });
                }
            }
            return graphConfig;
        }

        private List<GraphConfig> GetGraphConfigsForChildEntities(IEnumerable<Field> fields)
        {
            var graphConfig = new List<GraphConfig>();
            foreach (var field in fields)
            {
                if (field.FieldExpression.Body.Type.IsCollectionType())
                {
                    graphConfig.Add(new GraphConfig
                    {
                        FieldExpression = field.FieldExpression.Body as MemberExpression,
                        GraphConfigType = GraphConfigType.OwnedCollection
                    });
                }
                else
                {
                    graphConfig.Add(new GraphConfig
                    {
                        FieldExpression = field.FieldExpression.Body as MemberExpression,
                        GraphConfigType = GraphConfigType.OwnedEntity
                    });
                }
            }
            return graphConfig;
        }

        private TEntity DeleteItem<TEntity>(string itemId)
            where TEntity : class
        {
            var eType = typeof(TEntity);
            TEntity itemToDelete = GetItem<TEntity>(itemId);
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.Remove(itemToDelete);
            _dbContext.SaveChanges();
            return itemToDelete;

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

        private List<Field> GetManyToManyFields<TEntity>() where TEntity : class
        {
            return GetFieldsFor<TEntity>(f => f.FieldOption.RelationType == RelationType.ManyToMany);
        }

        private List<Field> GetManyToOneFields<TEntity>() where TEntity : class
        {
            return GetFieldsFor<TEntity>(f => f.FieldOption.RelationType == RelationType.ManyToOne);
        }

        private List<Field> GetFieldsFor<TEntity>(Func<Field, bool> predicate) where TEntity : class
        {
            var eType = typeof(TEntity);
            var adminConfig = GetAdminConfig(eType);
            //ManyToMany Includes
            var m2mFields = adminConfig.FormConfig.AllFormFields.Where(predicate).ToList();
            return m2mFields;
        }

        private List<Field> GetChildEntityFields<TEntity>() where TEntity : class
        {
            var eType = typeof(TEntity);
            var adminConfig = GetAdminConfig(eType);
            //ManyToMany Includes
            var m2mFields = adminConfig.ChildConfigs.Select(cc => cc.Field).ToList();
            return m2mFields;
        }

        private Type GetEntityType(string entityType)
        {
            return _adminSite.AdminConfigs.Keys.FirstOrDefault(t => t.Name == entityType);
        }

        private LambdaExpression CreatePrimaryKeyFilter(List<string> keyValues, Type entityType)
        {

            IAdminConfig adminConfig;
            if (_adminSite.AdminConfigs.TryGetValue(entityType, out adminConfig))
            {
                var key = adminConfig.EntityConfig.PrimaryKey;
                //return CreateFilter(key.Properties, keyValues, key.DeclaringEntityType.ClrType);
                return BuildObjectLambda(key.Properties, keyValues, key.DeclaringEntityType.ClrType);
            }

            return null;
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
