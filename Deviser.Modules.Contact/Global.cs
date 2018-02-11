using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Deviser.Modules.ContactForm
{
    public class Global
    {
        public static readonly ModuleMetaInfo ModuleMetaInfo = new ModuleMetaInfo()
        {
            ModuleAssembly = typeof(Global).GetTypeInfo().Assembly.FullName,
            ModuleName = "Contact",
            ModuleVersion = typeof(Global).GetTypeInfo().Assembly.GetName().Version.ToString()
        };
    }
}
