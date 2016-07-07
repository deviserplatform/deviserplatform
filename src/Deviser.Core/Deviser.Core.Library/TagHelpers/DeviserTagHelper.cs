using Deviser.Core.Library.Extensions;
using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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

        protected AppContext AppContext
        {
            get
            {
                AppContext returnValue = scopeService.AppContext;
                return returnValue;
            }
        }

        protected HttpContext HttpContext
        {
            get; set;
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
                var requestCulture = requestCultureFeature.RequestCulture.UICulture;
                return requestCulture;
            }
        }
    }
}
