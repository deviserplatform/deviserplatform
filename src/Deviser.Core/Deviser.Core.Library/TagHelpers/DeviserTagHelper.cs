using Deviser.Core.Library.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Library.Services;

namespace Deviser.Core.Library.TagHelpers
{
    public class DeviserTagHelper : TagHelper
    {
        IScopeService scopeService;
        
        public DeviserTagHelper(IHttpContextAccessor httpContextAccessor, IScopeService scopeService)
        {
            HttpContext = httpContextAccessor.HttpContext;
            this.scopeService = scopeService;
        }

        protected HttpContext HttpContext
        {
            get; set;
        }
    }
}
