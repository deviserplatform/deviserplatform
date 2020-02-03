using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Common.Module
{
    public interface IModuleRegistry
    {
        ModuleMetaInfo GetModuleMetaInfoByAssembly(string assemblyName);
        ModuleMetaInfo GetModuleMetaInfoByModuleName(string moduleName);
        bool TryRegisterModule(ModuleMetaInfo moduleMetaInfo);
    }
}
