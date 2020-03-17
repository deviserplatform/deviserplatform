using Deviser.Core.Common.DomainTypes;
using System;

namespace Deviser.Core.Library.Services
{
    public interface IScopeService
    {
        PageContext PageContext { get; set; }

        ModuleContext ModuleContext { get; set; }

        Guid ScopeId { get; }
    }
}
