using System;
using Deviser.Core.Common.DomainTypes;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Library.Extensions
{
    public static class PageContentExtension
    {
        public static Property Get(this ICollection<Property> properties, string propertyName)
        {
            if (properties != null && properties.Count > 0)
            {
                return properties.FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));
            }
            return null;
        }
    }
}
