using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Deviser.Core.Library.Sites;

namespace Deviser.Core.Library.Services
{
    public class ScopeService : IScopeService
    {
        public ScopeService(IHttpContextAccessor httpContextAccessor)
        {
            object pageContext;
            if(httpContextAccessor.HttpContext.Items.TryGetValue("PageContext", out pageContext))
            {
                PageContext = pageContext as PageContext;
            }
            else
            {
                PageContext = new PageContext();
            }
        }
    }
}
