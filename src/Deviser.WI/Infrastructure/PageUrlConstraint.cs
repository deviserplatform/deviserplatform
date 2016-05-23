using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Library;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;

namespace Deviser.WI.Infrastructure
{
    public class PageUrlConstraint : IRouteConstraint
    {
        IPageProvider pageProvider;

        public PageUrlConstraint(IPageProvider pageProvider)
        {
            this.pageProvider = pageProvider;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string requestCulture = "";
            string cultureKey = "culture";
            var requestCultureFeature = httpContext.Features.Get<IRequestCultureFeature>();
            if (values.ContainsKey(cultureKey) && !string.IsNullOrEmpty(values[cultureKey].ToString()))
            {
                requestCulture = values[cultureKey].ToString();
            }
            else
            {
                requestCulture = requestCultureFeature.RequestCulture.Culture.ToString();
            }

            var pages = pageProvider.GetPageTranslations(requestCulture);
            if (values[routeKey] != null && pages != null && pages.Count > 0)
            {
                var permalink = requestCulture + "/" + values[routeKey];
                var result = pages.Any(p => (p != null && p.URL.ToLower() == permalink.ToLower()));
                return result;
            }
            return false;
        }
    }
}
