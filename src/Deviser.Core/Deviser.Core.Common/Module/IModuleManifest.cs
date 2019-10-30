using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.Module
{
    public interface IModuleManifest
    {
        ModuleMetaInfo ModuleMetaInfo { get; }
    }
}
