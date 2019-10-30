using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Common.Module
{
    public class ModuleManifest : IModuleManifest
    {
        public ModuleMetaInfo ModuleMetaInfo { get; }

        public ModuleManifest()
        {
            this.ModuleMetaInfo = new ModuleMetaInfo();
        }

    }
}
