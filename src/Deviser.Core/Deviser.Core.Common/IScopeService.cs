using System;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Common
{
    public interface IScopeService
    {
        PageContext PageContext { get; set; }

        ModuleContext ModuleContext { get; set; }

        Guid ScopeId { get; }
    }
}
