using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Common.Module
{
    public interface IModuleManifest
    {
        ModuleMetaInfo ModuleMetaInfo { get; }
    }
}
