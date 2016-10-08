using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.ClientDependency
{
    public static class DependencyManager
    {
        public static DependencyLoader GetLoader(HttpContext httpContext)
        {
            if (httpContext.Items.ContainsKey("Deviser.ClientDependency.DependencyLoader"))
            {
                return (DependencyLoader)httpContext.Items["Deviser.ClientDependency.DependencyLoader"];
            }
            else
            {
                var loader = new DependencyLoader();
                httpContext.Items["Deviser.ClientDependency.DependencyLoader"] = loader;
                return loader;
            }
        }
    }
}
