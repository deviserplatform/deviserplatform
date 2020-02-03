using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Deviser.Core.Library.TagHelpers
{
    public class DeviserTagHelper : TagHelper
    {
        //private readonly IScopeService _scopeService;
        
        public DeviserTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;
            //_scopeService = scopeService;
        }

        protected HttpContext HttpContext
        {
            get; set;
        }
    }
}
