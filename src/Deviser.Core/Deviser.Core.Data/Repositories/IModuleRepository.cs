using System;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface IModuleRepository //: IRepositoryBase
    {
        List<Module> GetModules();
        Module GetModule(Guid moduleId);
        ModuleView GetModuleView(Guid moduleViewId);
        List<ModuleView> GetModuleViews();
        List<ModuleViewType> GetModuleViewType();
        List<ModuleView> GetEditModuleViews(Guid moduleId);
        Module GetModule(string moduleName);
        Module GetModuleByPageModuleId(Guid pageModuleId);
        Module Create(Module dbModule);
        Module UpdateModule(Module dbModule);
        ModuleView CreateModuleView(ModuleView moduleView);
        ModuleView UpdateModuleView(ModuleView moduleView);
    }
}