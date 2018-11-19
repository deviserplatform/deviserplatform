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


namespace Deviser.Admin.Data
{
    public interface IAdminRepository<TAdminConfigurator>
        where TAdminConfigurator : IAdminConfigurator
    {
        IAdminSite AdminConfig { get; }

        IAdminConfig GetAdminConfig(string entityType);
        object GetAllFor(string entityType, int pageNo = 1, int pageSize = 50, string orderBy = null);
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


        public AdminRepository(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<AdminRepository<TAdminConfigurator>>>();
            _adminSiteProvider = serviceProvider.GetService<IAdminSiteProvider>();

            _adminConfiguratorType = typeof(TAdminConfigurator);
            _adminSite = _adminSiteProvider.GetAdminConfig(_adminConfiguratorType);
            _dbContext = (DbContext)serviceProvider.GetService(_adminSite.DbContextType);

            if (_adminSite == null)
                throw new InvalidOperationException($"Admin site is not found for type {_adminConfiguratorType}");
        }

        public IAdminSite AdminConfig => _adminSite;

        public IAdminConfig GetAdminConfig(string entityType)
        {
            var eType = GetEntityType(entityType);
            IAdminConfig adminConfig;
            if (_adminSite.AdminConfigs.TryGetValue(eType, out adminConfig))
            {
                return adminConfig;
            }
            return null;
        }

        public object GetAllFor(string entityType, int pageNo = 1, int pageSize = 50, string orderBy = null)
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

            query.Skip(skip)
                .Take(pageSize);

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
            var result = query.FirstOrDefault();
            return result;
        }

        private TEntity CreateItem<TEntity>(object item)
            where TEntity : class
        {
            var eType = typeof(TEntity);
            TEntity itemToAdd = ((Newtonsoft.Json.Linq.JObject)item).ToObject<TEntity>();
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.Add(itemToAdd);
            _dbContext.SaveChanges();
            return itemToAdd;
        }

        private TEntity UpdateItem<TEntity>(object item)
            where TEntity : class
        {
            var eType = typeof(TEntity);
            TEntity itemToAdd = ((Newtonsoft.Json.Linq.JObject)item).ToObject<TEntity>();
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.Update(itemToAdd);
            _dbContext.SaveChanges();
            return itemToAdd;

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
