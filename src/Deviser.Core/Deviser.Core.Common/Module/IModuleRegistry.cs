using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.Module
{
    public interface IModuleRegistry
    {
        ModuleMetaInfo GetModuleMetaInfoByAssembly(string assemblyName);
        ModuleMetaInfo GetModuleMetaInfoByModuleName(string moduleName);
        bool TryRegisterModule(ModuleMetaInfo moduleMetaInfo);
    }
}
