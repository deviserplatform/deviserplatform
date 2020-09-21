using System;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Library.Extensions
{
    public static class PageExtension
    {
        public static PageTranslation Get(this ICollection<PageTranslation> pageTranslations)
        {
            if(pageTranslations!=null && pageTranslations.Count > 0)
            {
                var currentCulture = Globals.CurrentCulture;
                return pageTranslations.FirstOrDefault(t => string.Equals(t.Locale, currentCulture.Name, StringComparison.CurrentCultureIgnoreCase));
            }
            return null;
        }

        public static PageTranslation Get(this ICollection<PageTranslation> pageTranslations, string locale)
        {
            if (pageTranslations != null && pageTranslations.Count > 0)
            {
                return pageTranslations.FirstOrDefault(t => string.Equals(t.Locale, locale, StringComparison.CurrentCultureIgnoreCase));
            }
            return null;
        }
    }
}
