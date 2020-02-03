using Deviser.Core.Common.DomainTypes;
using System;

namespace Deviser.Core.Library.Services
{
    public interface IScopeService
    {
        PageContext PageContext { get; }

        ModuleContext ModuleContext { get;}

        Guid ScopeId { get; }
    }
}
