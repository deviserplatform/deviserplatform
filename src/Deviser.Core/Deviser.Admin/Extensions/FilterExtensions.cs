using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Deviser.Admin.Config.Filters;

namespace Deviser.Admin.Extensions
{
    public static class FilterExtensions
    {
        public static IQueryable<TSource> ApplyFilter<TSource>(this IQueryable<TSource> baseQuery, FilterNode rootFilterNode)
        {
            var predicate = ConvertToPredicate<TSource>(rootFilterNode);
            baseQuery = baseQuery.Where(predicate);
            return baseQuery;
        }

        public static IEnumerable<TSource> ApplyFilter<TSource>(this IEnumerable<TSource> baseQuery, FilterNode rootFilterNode)
        {
            var predicate = ConvertToPredicate<TSource>(rootFilterNode);
            var func = predicate.Compile();
            baseQuery = baseQuery.Where(func);
            //baseQuery = baseQuery.Where(predicate);
            return baseQuery;
        }

        public static Expression<Func<TSource, bool>> ConvertToPredicate<TSource>(FilterNode rootFilterNode)
        {
            var sourceType = typeof(TSource);
            var nodeExpression = ConvertToPredicateRecursive<TSource>(rootFilterNode);
            var paramExpr = Expression.Parameter(sourceType, "p");
            var resultExpression = Expression.Lambda<Func<TSource, bool>>(nodeExpression, paramExpr);
            return resultExpression;
        }

        private static Expression ConvertToPredicateRecursive<TSource>(FilterNode rootFilterNode)
        {
            Expression nodeExpression = null;
            foreach (var childNode in rootFilterNode.ChildNodes)
            {
                Expression childPredicate = null;
                switch (childNode.Filter)
                {
                    case BooleanFilter booleanFilter:
                        childPredicate = GetBooleanExpression<TSource>(booleanFilter);
                        break;
                    case DateFilter dateFilter:
                        childPredicate = GetDateExpression<TSource>(dateFilter);
                        break;
                    case NumberFilter numberFilter:
                        childPredicate = GetNumberExpression<TSource>(numberFilter);
                        break;
                    case TextFilter textFilter:
                        childPredicate = GetTextExpression<TSource>(textFilter);
                        break;
                    case SelectFilter selectFilter:
                        childPredicate = GetSelectExpression<TSource>(selectFilter);
                        break;
                    default:
                        continue;
                }

                if (nodeExpression == null)
                {
                    nodeExpression = childPredicate;
                }
                else
                {
                    nodeExpression = rootFilterNode.ChildOperator == LogicalOperator.AND 
                        ? Expression.AndAlso(nodeExpression, childPredicate)
                        : Expression.OrElse(nodeExpression, childPredicate);
                }

                if (childNode.ChildNodes == null || childNode.ChildNodes.Count <= 0) continue;

                var childGroupPredicate = ConvertToPredicateRecursive<TSource>(childNode);
                nodeExpression = childNode.RootOperator == LogicalOperator.AND
                    ? Expression.AndAlso(nodeExpression, childGroupPredicate)
                    : Expression.OrElse(nodeExpression, childGroupPredicate);
            }

            if (nodeExpression == null)
            {
                throw new InvalidOperationException(
                    $"Unable to find filter type for field {rootFilterNode.Filter.FieldName}");
            }

            return nodeExpression;
        }

        private static BinaryExpression GetBooleanExpression<TSource>(BooleanFilter booleanFilter)
        {
            var sourceType = typeof(TSource);
            var paramExpr = Expression.Parameter(sourceType, "p");

            var propertyInfo = sourceType.GetProperty(booleanFilter.FieldName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Filter property {booleanFilter.FieldName} not found");

            //BinaryExpression binaryExpression = null;

            //foreach (var strValue in selectFilter.FilterKeyValues)
            //{
            //    var keyValue = TypeDescriptor.GetConverter(keyPropInfo.PropertyType).ConvertFromInvariantString(strValue);
            //    var valueExpr = Expression.Constant(keyValue);
            //    var equalsExpression = Expression.Equal(keyPropExpr, valueExpr);

            //    binaryExpression = binaryExpression == null ? equalsExpression : Expression.AndAlso(binaryExpression, equalsExpression);
            //}


            var fieldExpr = Expression.Property(paramExpr, propertyInfo);
            List<BinaryExpression> binaryExpressions = new List<BinaryExpression>();
            
            if (booleanFilter.IsTrue)
            {
                var valExpr = Expression.Constant(booleanFilter.IsTrue);
                var be = Expression.Equal(fieldExpr, valExpr);
                binaryExpressions.Add(be);
            }

            if (booleanFilter.IsFalse)
            {
                var valExpr = Expression.Constant(booleanFilter.IsFalse);
                var be = Expression.Equal(fieldExpr, valExpr);
                binaryExpressions.Add(be);
            }

            BinaryExpression binaryExpression = null;
            foreach (var equalsExpression in binaryExpressions)
            {
                binaryExpression = binaryExpression == null ? equalsExpression : Expression.AndAlso(binaryExpression, equalsExpression);
            }

            return binaryExpression;
        }

        private static BinaryExpression GetDateExpression<TSource>(DateFilter dateFilter)
        {
            var sourceType = typeof(TSource);
            var paramExpr = Expression.Parameter(sourceType, "p");

            var propertyInfo = sourceType.GetProperty(dateFilter.FieldName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Filter property {dateFilter.FieldName} not found");

            var fieldExpr = Expression.Property(paramExpr, propertyInfo);

            var valExpr = Expression.Constant(dateFilter.Value);

            BinaryExpression binaryExpression = null;
            switch (dateFilter.Operator)
            {
                case DateTimeOperator.Equal:
                    binaryExpression = Expression.Equal(fieldExpr, valExpr);
                    break;
                case DateTimeOperator.After:
                    binaryExpression = Expression.GreaterThan(fieldExpr, valExpr);
                    break;
                case DateTimeOperator.AfterAnd:
                    binaryExpression = Expression.GreaterThanOrEqual(fieldExpr, valExpr);
                    break;
                case DateTimeOperator.InRange:
                    var fromExpr = Expression.GreaterThanOrEqual(fieldExpr, Expression.Constant(dateFilter.From));
                    var toExpr = Expression.LessThanOrEqual(fieldExpr, Expression.Constant(dateFilter.From));
                    binaryExpression = Expression.AndAlso(fromExpr, toExpr);
                    break;
                case DateTimeOperator.Before:
                    binaryExpression = Expression.LessThan(fieldExpr, valExpr);
                    break;
                case DateTimeOperator.BeforeAnd:
                    binaryExpression = Expression.LessThanOrEqual(fieldExpr, valExpr);
                    break;
                case DateTimeOperator.NotEqual:
                    binaryExpression = Expression.NotEqual(fieldExpr, valExpr);
                    break;
                default:
                    throw new InvalidOperationException($"Operator cannot be found for field {dateFilter.FieldName}");
            }

            return binaryExpression;
        }

        private static BinaryExpression GetNumberExpression<TSource>(NumberFilter numberFilter)
        {
            var sourceType = typeof(TSource);
            var paramExpr = Expression.Parameter(sourceType, "p");

            var propertyInfo = sourceType.GetProperty(numberFilter.FieldName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Filter property {numberFilter.FieldName} not found");

            var fieldExpr = Expression.Property(paramExpr, propertyInfo);

            var valExpr = Expression.Constant(numberFilter.Value);

            BinaryExpression binaryExpression = null;
            switch (numberFilter.Operator)
            {
                case NumberOperator.Equal:
                    binaryExpression = Expression.Equal(fieldExpr, valExpr);
                    break;
                case NumberOperator.GreaterThan:
                    binaryExpression = Expression.GreaterThan(fieldExpr, valExpr);
                    break;
                case NumberOperator.GreaterThanOrEqual:
                    binaryExpression = Expression.GreaterThanOrEqual(fieldExpr, valExpr);
                    break;
                case NumberOperator.InRange:
                    var fromExpr = Expression.GreaterThanOrEqual(fieldExpr, Expression.Constant(numberFilter.From));
                    var toExpr = Expression.LessThanOrEqual(fieldExpr, Expression.Constant(numberFilter.From));
                    binaryExpression = Expression.AndAlso(fromExpr, toExpr);
                    break;
                case NumberOperator.LessThan:
                    binaryExpression = Expression.LessThan(fieldExpr, valExpr);
                    break;
                case NumberOperator.LessThanOrEqual:
                    binaryExpression = Expression.LessThanOrEqual(fieldExpr, valExpr);
                    break;
                case NumberOperator.NotEqual:
                    binaryExpression = Expression.NotEqual(fieldExpr, valExpr);
                    break;
                default:
                    throw new InvalidOperationException($"Operator cannot be found for field {numberFilter.FieldName}");
            }

            return binaryExpression;
        }

        private static Expression GetTextExpression<TSource>(TextFilter textFilter)
        {
            var sourceType = typeof(TSource);
            var paramExpr = Expression.Parameter(sourceType, "p");

            var propertyInfo = sourceType.GetProperty(textFilter.FieldName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Filter property {textFilter.FieldName} not found");

            var fieldExpr = Expression.Property(paramExpr, propertyInfo);

            var valExpr = Expression.Constant(textFilter.Value);

            Expression predicateExpr = null;
            switch (textFilter.Operator)
            {
                case TextOperator.Equal:
                    predicateExpr = Expression.Equal(fieldExpr, valExpr);
                    break;
                case TextOperator.Contains:
                    MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    predicateExpr = Expression.Call(fieldExpr, containsMethod, valExpr);
                    break;
                case TextOperator.StartsWith:
                    MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    predicateExpr = Expression.Call(fieldExpr, startsWithMethod, valExpr);
                    break;
                case TextOperator.EndsWith:
                    MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    predicateExpr = Expression.Call(fieldExpr, endsWithMethod, valExpr);
                    break;
                case TextOperator.NotEqual:
                    predicateExpr = Expression.NotEqual(fieldExpr, valExpr);
                    break;
                default:
                    throw new InvalidOperationException($"Operator cannot be found for field {textFilter.FieldName}");
            }
            return predicateExpr;
        }

        private static BinaryExpression GetSelectExpression<TSource>(SelectFilter selectFilter)
        {
            var sourceType = typeof(TSource);
            var paramExpr = Expression.Parameter(sourceType, "p");

            var propertyInfo = sourceType.GetProperty(selectFilter.FieldName);
            if (propertyInfo == null)
                throw new InvalidOperationException($"Filter property {selectFilter.FieldName} not found");

            var keyPropInfo = propertyInfo.PropertyType.GetProperty(selectFilter.KeyFieldName);

            if (keyPropInfo == null)
                throw new InvalidOperationException($"Key Field {selectFilter.KeyFieldName} of Filter property {selectFilter.FieldName} not found");

            var fieldExpr = Expression.Property(paramExpr, propertyInfo);
            var keyPropExpr = Expression.Property(fieldExpr, keyPropInfo);
            BinaryExpression binaryExpression = null;

            foreach (var strValue in selectFilter.FilterKeyValues)
            {
                var keyValue = TypeDescriptor.GetConverter(keyPropInfo.PropertyType).ConvertFromInvariantString(strValue);
                var valueExpr = Expression.Constant(keyValue);
                var equalsExpression = Expression.Equal(keyPropExpr, valueExpr);

                binaryExpression = binaryExpression == null ? equalsExpression : Expression.AndAlso(binaryExpression, equalsExpression);
            }

            return binaryExpression;
        }
    }
}
