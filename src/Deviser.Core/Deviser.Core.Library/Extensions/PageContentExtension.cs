using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Extensions
{
    public static class PageContentExtension
    {
        public static Property Get(this List<Property> properties, string propertyName)
        {
            if (properties != null && properties.Count > 0)
            {
                return properties.FirstOrDefault(p => p.Name.ToLower() == propertyName.ToLower());
            }
            return null;
        }
    }
}
