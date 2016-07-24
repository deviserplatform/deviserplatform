using Deviser.Core.Common;
using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Extensions
{
    public static class PageExtension
    {
        public static PageTranslation Get(this ICollection<PageTranslation> pageTranslations)
        {
            if(pageTranslations!=null && pageTranslations.Count > 0)
            {
                var currentCulture = Globals.CurrentCulture;
                return pageTranslations.FirstOrDefault(t => t.Locale.ToLower() == currentCulture.Name.ToLower());
            }
            return null;
        }

        public static PageTranslation Get(this ICollection<PageTranslation> pageTranslations, string locale)
        {
            if (pageTranslations != null && pageTranslations.Count > 0)
            {
                var currentCulture = Globals.CurrentCulture;
                return pageTranslations.FirstOrDefault(t => t.Locale.ToLower() == locale.ToLower());
            }
            return null;
        }
    }
}
