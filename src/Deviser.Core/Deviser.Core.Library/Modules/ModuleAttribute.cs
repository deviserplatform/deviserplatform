﻿using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace Deviser.Core.Library.Modules
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ModuleAttribute : RouteValueAttribute
    {
        public ModuleAttribute(string moduleName)
            : base("area", moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                throw new ArgumentException("Module name must not be empty", nameof(moduleName));
            }
        }
    }
}
