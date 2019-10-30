using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.Module
{
    public class ModuleRegistry : IModuleRegistry
    {
        private readonly ConcurrentDictionary<string, string> _moduleNameAssemblyMapping;
        private readonly ConcurrentDictionary<string, ModuleMetaInfo> _moduleAssemblyMetaInfo;

        public ModuleRegistry()
        {
            _moduleNameAssemblyMapping = new ConcurrentDictionary<string, string>();
            _moduleAssemblyMetaInfo = new ConcurrentDictionary<string, ModuleMetaInfo>();
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

            if (_moduleAssemblyMetaInfo.ContainsKey(moduleMetaInfo.ModuleAssembly))
            {
                return false;
            }

            _moduleNameAssemblyMapping.TryAdd(moduleMetaInfo.ModuleName, moduleMetaInfo.ModuleAssembly);
            _moduleAssemblyMetaInfo.TryAdd(moduleMetaInfo.ModuleAssembly, moduleMetaInfo);
            return true;
        }

        public ModuleMetaInfo GetModuleMetaInfoByAssembly(string assemblyName)
        {
            _moduleAssemblyMetaInfo.TryGetValue(assemblyName, out ModuleMetaInfo moduleMetaInfo);
            return moduleMetaInfo;
        }

        public ModuleMetaInfo GetModuleMetaInfoByModuleName(string moduleName)
        {
            if (_moduleNameAssemblyMapping.TryGetValue(moduleName, out string assemblyName))
            {
                _moduleAssemblyMetaInfo.TryGetValue(moduleName, out ModuleMetaInfo moduleMetaInfo);
                return moduleMetaInfo;
            }
            return null;
        }
    }
}
