using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class ModuleMetaInfo
    {
        public string ModuleName { get; set; }

        public string ModuleAssembly { get; set; }

        public string ModuleVersion { get; set; }

        public TypeInfo AdminConfiguratorTypeInfo { get; set; }
    }
}
