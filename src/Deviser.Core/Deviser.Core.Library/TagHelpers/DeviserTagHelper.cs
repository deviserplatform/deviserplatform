using Deviser.Core.Library.Extensions;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Localization;
using Microsoft.AspNet.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.TagHelpers
{
    public class DeviserTagHelper : TagHelper
    {
        protected AppContext AppContext
        {
            get
            {
                AppContext returnValue = HttpContext.Session.GetObjectFromJson<AppContext>("AppContext");
                if (returnValue == null)
                {
                    returnValue = new AppContext();
                    HttpContext.Session.SetObjectAsJson("AppContext", returnValue);
                }
                returnValue.CurrentCulture = CurrentCulture;
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

        public DeviserTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;
        }
    }
}
