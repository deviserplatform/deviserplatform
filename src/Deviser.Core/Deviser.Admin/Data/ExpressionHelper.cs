using System;
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
            ParameterExpression paramterExpression = Expression.Parameter(type);
            MemberExpression memberExpression = Expression.Property(paramterExpression, propertyName);
            LambdaExpression propertyExpression = Expression.Lambda(memberExpression, paramterExpression);
            return propertyExpression;
        }

        public static LambdaExpression GetPropertyExpression(Type type, PropertyInfo propertyInfo)
        {
            ParameterExpression paramterExpression = Expression.Parameter(type);
            MemberExpression propertyExpression = Expression.Property(paramterExpression, propertyInfo);
            LambdaExpression lambdaExpression = Expression.Lambda(propertyExpression, paramterExpression);
            return lambdaExpression;
        }

        public static PropertyInfo GetPropertyInfo(LambdaExpression lambdaExpression)
        {
            return (lambdaExpression?.Body as MemberExpression)?.Member as PropertyInfo;
        }

        public static MethodCallExpression GetOrderByExpression(string orderByProp, Type eType, Expression parentExpression, string orderByMethod)
        {
            ParameterExpression paramterExpression = Expression.Parameter(eType);
            MemberExpression memberExpression = Expression.Property(paramterExpression, orderByProp);
            LambdaExpression propertyExpression = Expression.Lambda(memberExpression, paramterExpression);

            MethodCallExpression orderByCallExpression = Expression.Call(
                typeof(Queryable),
                orderByMethod,
                new Type[] { eType, memberExpression.Type },
                parentExpression,
                propertyExpression);
            return orderByCallExpression;
        }

        public static MethodCallExpression GetWhereExpression(Type eType, Expression parentExpression, LambdaExpression filterExpression)
        {
            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                new Type[] { eType },
                parentExpression,
                filterExpression);
            return whereCallExpression;
        }

        public static MethodCallExpression GetSelectExpression(Type eType, Expression parentExpression, LambdaExpression selectExpression)
        {
            MethodCallExpression selectCallExpression = Expression.Call(
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
    }
}
