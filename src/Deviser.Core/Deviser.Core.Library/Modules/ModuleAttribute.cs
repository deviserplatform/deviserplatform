using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Modules
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ModuleAttribute : RouteConstraintAttribute
    {
        public ModuleAttribute(string moduleName)
            : base("area", moduleName, blockNonAttributedActions: true)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                throw new ArgumentException("Area name must not be empty", nameof(moduleName));
            }
        }
    }
}
