using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var props = orderByProperties.Split(',');

            foreach (var prop in props)
            {
                var orderByProp = prop.Replace("-", "");
                var propertyExpression = GetPropertyExpression<TSource>(sourceType, orderByProp);
                if (baseQuery is IOrderedQueryable<TSource> baseOrderedQuery)
                {
                    baseQuery = prop.StartsWith("-") ? baseOrderedQuery.ThenByDescending(propertyExpression) : baseOrderedQuery.ThenBy(propertyExpression);
                }
                else
                {
                    baseQuery = prop.StartsWith("-") ? baseQuery.OrderByDescending(propertyExpression) : baseQuery.OrderBy(propertyExpression);
                }
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
            var props = orderByProperties.Split(',');

            foreach (var prop in props)
            {
                var orderByProp = prop.Replace("-", "");
                var propertyExpression = GetPropertyExpression<TSource>(sourceType, orderByProp);
                var del = propertyExpression.Compile();
                if (baseQuery is IOrderedEnumerable<TSource> baseOrderedQuery)
                {
                    baseQuery = prop.StartsWith("-") ? baseOrderedQuery.ThenByDescending(del) : baseOrderedQuery.ThenBy(del);
                }
                else
                {
                    baseQuery = prop.StartsWith("-") ? baseQuery.OrderByDescending(del) : baseQuery.OrderBy(del);
                }
            }

            return baseQuery;
        }

        private static Expression<Func<TSource, object>> GetPropertyExpression<TSource>(Type sourceType, string orderByProp)
        {
            var parameterExpression = Expression.Parameter(sourceType);
            var memberExpression = Expression.Property(parameterExpression, orderByProp);
            var objectMemberExpr = Expression.Convert(memberExpression, typeof(object)); //Convert Value/Reference type to object using boxing/lifting
            var propertyExpression = Expression.Lambda<Func<TSource, object>>(objectMemberExpr, parameterExpression);
            return propertyExpression;
        }
    }
}
