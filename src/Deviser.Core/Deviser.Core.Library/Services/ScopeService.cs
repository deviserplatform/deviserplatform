using System;
using Microsoft.AspNetCore.Http;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Library.Services
{
    public class ScopeService : IScopeService
    {
        public PageContext PageContext { get; }
        public ModuleContext ModuleContext { get; }

        public Guid ScopeId { get; }

        public ScopeService(IHttpContextAccessor httpContextAccessor)
        {
            ScopeId = Guid.NewGuid();

            object pageContext;
            object moduleContext;
            if (httpContextAccessor.HttpContext.Items.TryGetValue("PageContext", out pageContext))
            {
                PageContext = pageContext as PageContext;
            }
            else
            {
                PageContext = new PageContext();
            }

            if (httpContextAccessor.HttpContext.Items.TryGetValue("ModuleContext", out moduleContext))
            {
                ModuleContext = moduleContext as ModuleContext;
            }
            else
            {
                ModuleContext = new ModuleContext();
            }
        }

    }
}
