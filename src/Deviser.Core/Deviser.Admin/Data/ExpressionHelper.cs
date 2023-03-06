using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Deviser.Admin.Data
{
    public static class ExpressionHelper
    {

       // private static readonly MethodInfo OrderByMethod =
       // typeof(Queryable).GetMethods().Single(method =>
       //method.Name == "OrderBy" && method.GetParameters().Length == 2);

       // private static readonly MethodInfo OrderByDescendingMethod =
       //     typeof(Queryable).GetMethods().Single(method =>
       //    method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

        public static bool PropertyExists<T>(string propertyName)
        {
            return typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                                                       BindingFlags.Public | BindingFlags.Instance) != null;
        }

        public static LambdaExpression GetPropertyExpression(Type type, string propertyName)
        {
            var paramterExpression = Expression.Parameter(type);
            var memberExpression = Expression.Property(paramterExpression, propertyName);
            var propertyExpression = Expression.Lambda(memberExpression, paramterExpression);
            return propertyExpression;
        }

        public static LambdaExpression GetPropertyExpression(Type type, PropertyInfo propertyInfo)
        {
            var paramterExpression = Expression.Parameter(type);
            var propertyExpression = Expression.Property(paramterExpression, propertyInfo);
            var lambdaExpression = Expression.Lambda(propertyExpression, paramterExpression);
            return lambdaExpression;
        }

        public static PropertyInfo GetPropertyInfo(LambdaExpression lambdaExpression)
        {
            return (lambdaExpression?.Body as MemberExpression)?.Member as PropertyInfo;
        }

        public static MethodCallExpression GetOrderByExpression(string orderByProp, Type eType, Expression parentExpression, string orderByMethod)
        {
            var paramterExpression = Expression.Parameter(eType);
            var memberExpression = Expression.Property(paramterExpression, orderByProp);
            var propertyExpression = Expression.Lambda(memberExpression, paramterExpression);

            var orderByCallExpression = Expression.Call(
                typeof(Queryable),
                orderByMethod,
                new Type[] { eType, memberExpression.Type },
                parentExpression,
                propertyExpression);
            return orderByCallExpression;
        }

        public static MethodCallExpression GetWhereExpression(Type eType, Expression parentExpression, LambdaExpression filterExpression)
        {
            var whereCallExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                new Type[] { eType },
                parentExpression,
                filterExpression);
            return whereCallExpression;
        }

        public static MethodCallExpression GetSelectExpression(Type eType, Expression parentExpression, LambdaExpression selectExpression)
        {
            var selectCallExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Select),
                new Type[] { eType },
                parentExpression,
                selectExpression);
            return selectCallExpression;
        }

        //public static IQueryable<T> OrderByProperty<T>(
        //   this IQueryable<T> source, string propertyName)
        //{
        //    if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
        //        BindingFlags.Public | BindingFlags.Instance) == null)
        //    {
        //        return null;
        //    }
        //    ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
        //    Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
        //    LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
        //    MethodInfo genericMethod =
        //      OrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        //    object ret = genericMethod.Invoke(null, new object[] { source, lambda });
        //    return (IQueryable<T>)ret;
        //}

        //public static IQueryable<T> OrderByPropertyDescending<T>(
        //    this IQueryable<T> source, string propertyName)
        //{
        //    if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
        //        BindingFlags.Public | BindingFlags.Instance) == null)
        //    {
        //        return null;
        //    }
        //    ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
        //    Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
        //    LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
        //    MethodInfo genericMethod =
        //      OrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        //    object ret = genericMethod.Invoke(null, new object[] { source, lambda });
        //    return (IQueryable<T>)ret;
        //}
        public static List<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ToList();
        }
    }
}
