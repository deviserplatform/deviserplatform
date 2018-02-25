using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Library.Modules
{
    public class ModuleLocationRemapper : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            // Swap /Modules/ for /Areas/
            return viewLocations.Select(f => f.Replace("/Areas/", "/Modules/"));
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // do nothing.. not entirely needed for this 
        }
    }
}
