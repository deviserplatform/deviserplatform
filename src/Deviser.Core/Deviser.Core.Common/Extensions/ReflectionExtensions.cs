using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
