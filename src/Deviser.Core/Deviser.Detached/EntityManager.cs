using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Deviser.Detached
{
    public class EntityManager : IEntityManager
    {
        DbContext _dbContext;

        public EntityManager(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<INavigation> GetRequiredNavigationPropertiesForType(Type clrType)
        {
            if (clrType.GetTypeInfo().IsGenericType)
            {
                var entityType = GetEntityType(GetCollectionElementType(clrType));
                return entityType.GetNavigations();
            }
            else
            {
                var entityType = GetEntityType(clrType);
                return entityType.GetNavigations();
            }

        }

        public IEntityType GetEntityType(Type clrType)
        {
            return _dbContext.Model.FindEntityType(clrType);
        }

        public IKey GetPrimaryKey<TEntity>()
        {
            var clrType = typeof(TEntity);
            if (clrType.GetTypeInfo().IsGenericType)
            {
                return GetPrimaryKey(GetCollectionElementType(clrType));
            }
            return GetPrimaryKey(clrType);
        }

        public IKey GetPrimaryKey(object entity)
        {
            var clrType = entity.GetType();
            return GetPrimaryKey(clrType);
        }

        public IKey GetPrimaryKey(Type clrType)
        {
            var entityType = _dbContext.Model.FindEntityType(clrType);
            var key = entityType.FindPrimaryKey();
            return key;
        }

        public EntityKey GetEntityKey(object instance)
        {
            var key = GetPrimaryKey(instance);
            var entityKey = new EntityKey();
            var properties = key.Properties.Where(p => p.IsPrimaryKey()).ToList();
            //List<object> primaryKeyValues = new List<object>();
            foreach (var property in properties)
            {
                var getter = property.GetGetter();
                var value = getter.GetClrValue(instance);
                entityKey.AddKeyValue(property.Name, value);
            }
            return entityKey;
        }

        public bool AreKeysIdentical(object newValue, object dbValue)
        {
            if (newValue == null || dbValue == null)
            {
                return false;
            }

            var newValKeys = GetEntityKey(newValue);
            var dbValKeys = GetEntityKey(dbValue);
            var areEqual = newValKeys == dbValKeys;
            return areEqual;
        }

        public object CreateEmptyEntityWithKey(object entity)
        {
            var instance = Activator.CreateInstance(entity.GetType(), true);
            CopyPrimaryKeyFields(entity, instance);
            return instance;
        }

        public IEnumerable<PropertyInfo> GetPrimaryKeyFieldsFor(object entity)
        {
            var key = GetPrimaryKey(entity);
            var properties = key.Properties
                .Where(p => p.IsPrimaryKey())
                .Select(p => p.PropertyInfo)
                .ToList();
            return properties;
        }

        public TEntity LoadPersisted<TEntity>(TEntity entity, List<string> includeStringPaths)
            where TEntity : class
        {
            var keyValues = GetPrimaryKeyValues(entity);
            return LoadPersisted<TEntity>(keyValues, includeStringPaths).FirstOrDefault();
        }

        public List<object> GetPrimaryKeyValues(object entity)
        {
            var entityKey = GetEntityKey(entity);
            var keyValues = entityKey.GetAllValues();
            return keyValues;
        }

        private IList<TEntity> LoadPersisted<TEntity>(List<object> keyValues, List<string> includeStringPaths)
            where TEntity : class
        {
            var keyFilter = CreateFilter<TEntity>(keyValues);

            var query = GetBaseQuery<TEntity>(includeStringPaths).AsTracking().Where(keyFilter);

            IList<TEntity> persisted = query.ToListAsync().Result;

            return persisted;
        }

        private IQueryable<TEntity> GetBaseQuery<TEntity>(List<string> includeStringPaths)
            where TEntity : class
        {
            var entityType = _dbContext.Model.FindEntityType(typeof(TEntity));

            // get base query.
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            //// include all paths.
            foreach (var path in includeStringPaths)
            {
                query = query.Include(path);
            }

            return query;
        }

        private Expression<Func<TEntity, bool>> CreateFilter<TEntity>(List<object> keyValues)
        {
            Expression result = null;
            var key = GetPrimaryKey<TEntity>();
            var param = Expression.Parameter(key.DeclaringEntityType.ClrType);


            foreach (var keyVal in keyValues)
            {
                Func<int, Expression> buildCompare = i =>
                {
                    var keyValue = keyVal;
                    var keyProperty = key.Properties[i];
                    if (keyValue.GetType() != keyProperty.ClrType)
                    {
                        keyValue = Convert.ChangeType(keyValue, keyProperty.ClrType);
                    }

                    return Expression.Equal(Expression.Property(param, keyProperty.PropertyInfo),
                                            Expression.Constant(keyValue));
                };

                var findExpr = buildCompare(0);
                for (var i = 1; i < key.Properties.Count; i++)
                {
                    findExpr = Expression.AndAlso(findExpr, buildCompare(i));
                }
                if (result == null)
                    result = findExpr;
                else
                    result = Expression.OrElse(result, findExpr);
            }

            return Expression.Lambda<Func<TEntity, bool>>(result, param);
        }
        
        private void CopyPrimaryKeyFields(object from, object to)
        {
            var keyProperties = GetPrimaryKeyFieldsFor(from);
            foreach (var keyProperty in keyProperties)
            {
                keyProperty.SetValue(to, keyProperty.GetValue(from, null), null);
            }
        }

        private Type GetCollectionElementType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (type.GetTypeInfo().IsGenericType)
            {
                return type.GetTypeInfo().GetGenericArguments()[0];
            }

            throw new InvalidOperationException("GraphDiff requires the collection to be either IEnumerable<T> or T[]");
        }
    }
}
