using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Deviser.Core.Common.Extensions
{
    public static class ReflectionExtensions
    {
        public static List<TypeInfo> GetDerivedTypeInfos(this IEnumerable<Assembly> assemblies, Type baseType)
        {
            List<TypeInfo> derivedTypes = new List<TypeInfo>();
            foreach (var assembly in assemblies)
            {
                var moduleDbContextTypes = assembly.DefinedTypes.Where((t) => (baseType.IsAssignableFrom(t) ||
                (t.GetInterfaces().Any(i=>i.IsGenericType && i.GetGenericTypeDefinition()==baseType)))).ToList();

                if (moduleDbContextTypes?.Count > 0)
                    derivedTypes.AddRange(moduleDbContextTypes);
            }
            return derivedTypes;
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
            return type?.GetInterfaces()?.Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>)) ?? false;
        }
    }
}
