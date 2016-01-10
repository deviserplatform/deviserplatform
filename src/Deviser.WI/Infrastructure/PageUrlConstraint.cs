using Microsoft.AspNet.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Library;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Localization;

namespace Deviser.WI.Infrastructure
{
    public class PageUrlConstraint : IRouteConstraint
    {
        IPageProvider pageProvider;

        public PageUrlConstraint(IPageProvider pageProvider)
        {
            this.pageProvider = pageProvider;
        }
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, IDictionary<string, object> values, RouteDirection routeDirection)
        {
            var requestCultureFeature = httpContext.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature.RequestCulture;
            var pages = pageProvider.GetPageTranslations(requestCulture.Culture.ToString());
            if (values[routeKey] != null && pages != null && pages.Count > 0)
            {
                var permalink = "/" + values[routeKey].ToString();
                var result = pages.Any(p => (p != null && p.URL.ToLower() == permalink.ToLower()));
                return result;
            }
            return false;
        }
    }
}
