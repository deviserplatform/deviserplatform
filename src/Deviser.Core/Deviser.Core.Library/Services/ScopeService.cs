using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Http;
using System;

namespace Deviser.Core.Library.Services
{
    public class ScopeService : IScopeService
    {
        public PageContext PageContext { get; set; }
        public ModuleContext ModuleContext { get; set; }

        public Guid ScopeId { get; }

        public ScopeService(IHttpContextAccessor httpContextAccessor)
        {
            ScopeId = Guid.NewGuid();

            //object pageContext;
            //object moduleContext;
            //if (httpContextAccessor.HttpContext.Items.TryGetValue("PageContext", out pageContext))
            //{
            //    PageContext = pageContext as PageContext;
            //}
            //else
            //{
            //    PageContext = new PageContext();
            //}

            //if (httpContextAccessor.HttpContext.Items.TryGetValue("ModuleContext", out moduleContext))
            //{
            //    ModuleContext = moduleContext as ModuleContext;
            //}
            //else
            //{
            //    ModuleContext = new ModuleContext();
            //}
        }

    }
}
