using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Deviser.Admin.Data;

namespace Deviser.Admin.Extensions
{
    public static class SortExtensions
    {
        /// <summary>
        /// Adds OrderBy or OrderByDescending chain for provided properties in comma separated
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="baseQuery"></param>
        /// <param name="orderByProperties">Comma separated property string. Hyphen (-) prefix to order by descending. E.g. PropertyName to sort by ascending -PropertyName to sort by descending.</param>
        /// <returns></returns>
        public static IQueryable<TSource> SortBy<TSource>(this IQueryable<TSource> baseQuery, string orderByProperties)
        {
            if (string.IsNullOrEmpty(orderByProperties))
            {
                return baseQuery;
            }

            var sourceType = typeof(TSource);
            //var queryableSourceType = typeof(IQueryable<TSource>);
            var props = orderByProperties.Split(',');

            foreach (var prop in props)
            {
                var orderByProp = prop.Replace("-", "");
                var parameterExpression = Expression.Parameter(sourceType);
                var memberExpression = Expression.Property(parameterExpression, orderByProp);

                var memberType = (memberExpression.Member as PropertyInfo)?.PropertyType;

                baseQuery = CallGenericMethod<IQueryable<TSource>>(nameof(BuildSortExpressionForQueryable), new Type[] { sourceType, memberType }, new object[] { baseQuery, parameterExpression, memberExpression, !prop.StartsWith("-") });
            }

            return baseQuery;
        }

        /// <summary>
        /// Adds OrderBy or OrderByDescending chain for provided properties in comma separated
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="baseQuery"></param>
        /// <param name="orderByProperties">Comma separated property string. Hyphen (-) prefix to order by descending. E.g. PropertyName to sort by ascending -PropertyName to sort by descending.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> SortBy<TSource>(this IEnumerable<TSource> baseQuery, string orderByProperties)
        {
            if (string.IsNullOrEmpty(orderByProperties))
            {
                return baseQuery;
            }

            var sourceType = typeof(TSource);
            //var enumerableSourceType = typeof(IEnumerable<TSource>);
            var props = orderByProperties.Split(',');

            foreach (var prop in props)
            {
                var orderByProp = prop.Replace("-", "");
                var parameterExpression = Expression.Parameter(sourceType);
                var memberExpression = Expression.Property(parameterExpression, orderByProp);

                var memberType = (memberExpression.Member as PropertyInfo)?.PropertyType;

                baseQuery = CallGenericMethod<IEnumerable<TSource>>(nameof(BuildSortExpressionForEnumerable), new Type[] { sourceType, memberType }, new object[] { baseQuery, parameterExpression, memberExpression, !prop.StartsWith("-") });
                //baseQuery = BuildSortExpressionForEnumerable<TSource>(baseQuery, parameterExpression, memberExpression, !prop.StartsWith("-"));
            }

            return baseQuery;
        }

        private static IQueryable<TSource> BuildSortExpressionForQueryable<TSource, TKey>(IQueryable<TSource> baseQuery,
            ParameterExpression parameterExpression,
            MemberExpression memberExpression,
            bool isAscendingOrder)
        {
            var propertyExpression = GetPropertyExpression<TSource, TKey>(parameterExpression, memberExpression);

            if (baseQuery is IOrderedQueryable<TSource> baseOrderedQuery)
            {
                baseQuery = isAscendingOrder ? baseOrderedQuery.ThenByDescending(propertyExpression) : baseOrderedQuery.ThenBy(propertyExpression);
            }
            else
            {
                baseQuery = isAscendingOrder ? baseQuery.OrderByDescending(propertyExpression) : baseQuery.OrderBy(propertyExpression);
            }

            return baseQuery;
        }

        private static IEnumerable<TSource> BuildSortExpressionForEnumerable<TSource, TKey>(IEnumerable<TSource> baseQuery,
            ParameterExpression parameterExpression,
            MemberExpression memberExpression,
            bool isAscendingOrder)
        {
            var propertyExpression = GetPropertyExpression<TSource, TKey>(parameterExpression, memberExpression);

            var del = propertyExpression.Compile();
            if (baseQuery is IOrderedEnumerable<TSource> baseOrderedQuery)
            {
                baseQuery = isAscendingOrder ? baseOrderedQuery.ThenByDescending(del) : baseOrderedQuery.ThenBy(del);
            }
            else
            {
                baseQuery = isAscendingOrder ? baseQuery.OrderByDescending(del) : baseQuery.OrderBy(del);
            }

            return baseQuery;
        }

        private static Expression<Func<TSource, TKey>> GetPropertyExpression<TSource, TKey>(ParameterExpression parameterExpression, MemberExpression memberExpression)
        {
            //var objectMemberExpr = Expression.Convert(memberExpression, typeof(object)); //Convert Value/Reference type to object using boxing/lifting
            //var propertyExpression = Expression.Lambda<Func<TSource, object>>(objectMemberExpr, parameterExpression);
            var propertyExpression = Expression.Lambda<Func<TSource, TKey>>(memberExpression, parameterExpression);
            return propertyExpression;
        }

        private static TResult CallGenericMethod<TResult>(string methodName, Type[] genericTypes, object[] parmeters)
            where TResult : class
        {
            var getItemMethodInfo = typeof(SortExtensions).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericTypes);
            var result = getItemMethod.Invoke(null, parmeters);
            return result as TResult;
        }
    }
}
