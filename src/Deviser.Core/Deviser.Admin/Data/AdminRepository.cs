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

namespace Deviser.Admin.Data
{
    public interface IAdminRepository<TAdminConfigurator>
        where TAdminConfigurator : IAdminConfigurator
    {
        IAdminSite AdminConfig { get; }

        IAdminConfig GetAdminConfig(string entityType);
        object GetAllFor(string entityType);
        object GetItemFor(string entityType, string itemId);
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



        public object GetAllFor(string entityType)
        {
            var eType = GetEntityType(entityType);
            var genericMethodInfo = typeof(DbContext).GetMethod("Set");
            var setGenericMethod = genericMethodInfo.MakeGenericMethod(eType);
            var dbSet = setGenericMethod.Invoke(_dbContext, null);

            var toListMethodInfo = typeof(Enumerable).GetMethod("ToList");
            var toListGenericMethod = toListMethodInfo.MakeGenericMethod(eType);
            var result = toListGenericMethod.Invoke(null, new object[] { dbSet });
            return result;
        }


        public object GetItemFor(string entityType, string itemId)
        {
            var eType = GetEntityType(entityType);

            //Creates lambda expression (e => e.id == id)
            Expression keyFilter = CreateFilter(new List<string> { itemId }, eType);


            //var entity = await DbContext.Set<TEntity>().Where(keyFilter)
            //    .SingleOrDefaultAsync();
                        
            var genericMethodInfo = typeof(DbContext).GetMethod("Set");
            var setGenericMethod = genericMethodInfo.MakeGenericMethod(eType);
            var dbSet = setGenericMethod.Invoke(_dbContext, null);

            var wm = GetWhereMethod();
            var whereMethod = wm.MakeGenericMethod(eType);
            var whereResult = whereMethod.Invoke(dbSet, new object[] { dbSet,  keyFilter });
            var fOd = typeof(Queryable).GetMethods().Where(m => m.Name == "FirstOrDefault" && m.GetParameters().Length==1).FirstOrDefault();
            var firstOrDefault = fOd.MakeGenericMethod(eType);
            var result = firstOrDefault.Invoke(whereResult, new object[] { whereResult });

            return result;
        }

        private MethodInfo GetWhereMethod()
        {
            //typeof(Queryable).GetMethods()[66].GetParameters()[1].ParameterType.GenericTypeArguments[0].GenericTypeArguments.Count()
            var methods = typeof(Queryable).GetMethods().Where(m=>m.Name=="Where");            
            foreach(var method in methods)
            {
                var parameters = method?.GetParameters();
                if (parameters.Length > 1 && parameters[1]?.ParameterType.GenericTypeArguments[0]?.GenericTypeArguments.Count() == 2)
                {
                    return method;
                }
            }
            return null;
        }

        private MethodInfo GetFirstOrDefaultMethod()
        {
            //typeof(Queryable).GetMethods()[66].GetParameters()[1].ParameterType.GenericTypeArguments[0].GenericTypeArguments.Count()
            var methods = typeof(Queryable).GetMethods().Where(m => m.Name == "FirstOrDefault");
            foreach (var method in methods)
            {
                var parameters = method?.GetParameters();
                if (parameters.Length>1 && parameters[1]?.ParameterType.GenericTypeArguments[0]?.GenericTypeArguments.Count() == 2)
                {
                    return method;
                }
            }
            return null;
        }

        private Type GetEntityType(string entityType)
        {
            return _adminSite.AdminConfigs.Keys.FirstOrDefault(t => t.Name == entityType);
        }

        //private Expression CreateFindExpr(object value, Type entityType)
        //{
        //    IAdminConfig adminConfig;
        //    if(_adminSite.AdminConfigs.TryGetValue(entityType, out adminConfig))
        //    {                
        //        var key = adminConfig.EntityConfig.PrimaryKey;
        //        IProperty keyProperty = key.Properties[0];
        //        ParameterExpression param = Expression.Parameter(key.DeclaringEntityType.ClrType);
        //        var equalExpr = Expression.Equal(Expression.Property(param, keyProperty.PropertyInfo),
        //                                        Expression.Constant(value));
        //        return Expression.Lambda<Func<TEntity, bool>>(equalExpr, param);
        //    }

        //}

        private LambdaExpression CreateFilter(List<string> keyValues, Type entityType)
        {

            IAdminConfig adminConfig;
            if (_adminSite.AdminConfigs.TryGetValue(entityType, out adminConfig))
            {
                var key = adminConfig.EntityConfig.PrimaryKey;
                Expression result = null;
                ParameterExpression param = Expression.Parameter(key.DeclaringEntityType.ClrType);

                foreach (string keyVal in keyValues)
                {
                    Func<int, Expression> buildCompare = i =>
                    {
                        string strKeyVal = keyVal;
                        object keyValue = keyVal;
                        IProperty keyProperty = key.Properties[i];
                        if (keyValue.GetType() != keyProperty.ClrType)
                        {
                            //keyValue = Convert.ChangeType(keyValue, keyProperty.ClrType);
                            keyValue = TypeDescriptor.GetConverter(keyProperty.ClrType).ConvertFromInvariantString(strKeyVal);
                        }

                        return Expression.Equal(Expression.Property(param, keyProperty.PropertyInfo),
                                                Expression.Constant(keyValue));
                    };

                    Expression findExpr = buildCompare(0);
                    for (int i = 1; i < key.Properties.Count; i++)
                    {
                        findExpr = Expression.AndAlso(findExpr, buildCompare(i));
                    }
                    if (result == null)
                        result = findExpr;
                    else
                        result = Expression.OrElse(result, findExpr);
                }

                return Expression.Lambda(result, param);
            }

            return null;
        }
    }
}
