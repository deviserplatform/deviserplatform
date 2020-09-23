using Deviser.Core.Common.DomainTypes;
using System.Collections.Concurrent;

namespace Deviser.Core.Common.Module
{
    public class ModuleRegistry : IModuleRegistry
    {
        private static readonly ConcurrentDictionary<string, string> _moduleNameAssemblyMapping = new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, ModuleMetaInfo> _moduleAssemblyMetaInfo = new ConcurrentDictionary<string, ModuleMetaInfo>();

        public ModuleRegistry()
        {

        }

        /// <summary>
        /// Registers new Module, moduleMetaInfo.ModuleName and moduleMetaInfo.ModuleAssembly should be unique
        /// </summary>
        /// <param name="moduleMetaInfo"></param>
        /// <returns></returns>
        public bool TryRegisterModule(ModuleMetaInfo moduleMetaInfo)
        {
            if (_moduleNameAssemblyMapping.ContainsKey(moduleMetaInfo.ModuleName))
            {
                return false;
            }

            if (_moduleAssemblyMetaInfo.ContainsKey(moduleMetaInfo.ModuleAssemblyFullName))
            {
                return false;
            }

            _moduleNameAssemblyMapping.TryAdd(moduleMetaInfo.ModuleName, moduleMetaInfo.ModuleAssemblyFullName);
            _moduleAssemblyMetaInfo.TryAdd(moduleMetaInfo.ModuleAssemblyFullName, moduleMetaInfo);
            return true;
        }

        public ModuleMetaInfo GetModuleMetaInfoByAssembly(string assemblyName)
        {
            _moduleAssemblyMetaInfo.TryGetValue(assemblyName, out var moduleMetaInfo);
            return moduleMetaInfo;
        }

        public ModuleMetaInfo GetModuleMetaInfoByModuleName(string moduleName)
        {
            if (_moduleNameAssemblyMapping.TryGetValue(moduleName, out var assemblyName))
            {
                _moduleAssemblyMetaInfo.TryGetValue(assemblyName, out var moduleMetaInfo);
                return moduleMetaInfo;
            }
            return null;
        }
    }
}
