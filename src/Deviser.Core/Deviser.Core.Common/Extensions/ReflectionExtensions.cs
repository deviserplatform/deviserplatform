using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Deviser.Core.Common.Extensions
{
    public static class ReflectionExtensions
    {
        public static List<TypeInfo> GetDerivedTypeInfos(this IEnumerable<Assembly> assemblies, Type baseType)
        {
            List<TypeInfo> derivedTypeList = new List<TypeInfo>();
            foreach (var assembly in assemblies)
            {
                List<TypeInfo> derivedTypes = assembly.GetDerivedTypeInfos(baseType);

                if (derivedTypes?.Count > 0)
                    derivedTypeList.AddRange(derivedTypes);
            }
            return derivedTypeList;
        }

        public static List<TypeInfo> GetDerivedTypeInfos(this Assembly assembly, Type baseType)
        {
            return assembly.DefinedTypes.Where((t) => (baseType.IsAssignableFrom(t) ||
                            (t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseType)))).ToList();
        }

        public static string GetMemberName(LambdaExpression fieldExpression)
        {
            if (fieldExpression == null)
                return null;

            MemberExpression memberExpression;

            if (fieldExpression.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = (UnaryExpression)(fieldExpression.Body);
                memberExpression = (MemberExpression)(unaryExpression.Operand);
            }
            else
            {
                memberExpression = (MemberExpression)(fieldExpression.Body);
            }


            var fieldName = ((PropertyInfo)memberExpression.Member).Name;
            return fieldName;
        }

        public static bool IsCollectionType(this Type type)
        {
            if (type == null) return false;
            return type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>)) ||
                   type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>);
        }
    }
}
