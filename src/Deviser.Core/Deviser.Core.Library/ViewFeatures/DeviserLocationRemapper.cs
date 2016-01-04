using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.ViewFreatures
{
    public class DeviserLocationRemapper : IViewLocationExpander
    {

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            // Swap /Shared/ for /_Shared/
            return viewLocations.Select(f => f.Replace("/Areas/", "/Modules/"));
            //return viewLocations.;


        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // do nothing.. not entirely needed for this 
        }

    }
}
