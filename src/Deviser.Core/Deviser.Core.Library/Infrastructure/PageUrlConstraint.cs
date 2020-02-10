using System;
using Deviser.Core.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Deviser.Core.Library.Infrastructure
{
    public class PageUrlConstraint : IRouteConstraint
    {
        private readonly bool _isEverythingInstalled;
        
        public PageUrlConstraint(IServiceProvider serviceProvider)
        {
            var installationProvider = serviceProvider.GetService<IInstallationProvider>(); //installationProvider;
            _isEverythingInstalled = installationProvider.IsPlatformInstalled && installationProvider.IsDatabaseExist;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!_isEverythingInstalled)
                return false;

            if (values[routeKey] != null)
            {
                var permalink = values[routeKey].ToString();
                var currentCulture = GetCurrentCulture(httpContext, permalink); // Have  to use this since it will be clled before InitPageContext()
                var pageRepository = httpContext.RequestServices.GetService<IPageRepository>();
                var pageTranslations = pageRepository.GetPageTranslations(currentCulture.ToString());
                if (pageTranslations != null && pageTranslations.Count > 0)
                {
                    var result = pageTranslations.Any(p => (p != null && p.URL.ToLower() == permalink.ToLower()));
                    return result;
                }
            }
            return false;
        }

        private CultureInfo GetCurrentCulture(HttpContext httpContext, string permalink)
        {
            var requestCultureFeature = httpContext.Features.Get<IRequestCultureFeature>();
            var languageRepository = httpContext.RequestServices.GetService<ILanguageRepository>();
            var isMultilingual = languageRepository.IsMultilingual();
            CultureInfo requestCulture = null;

            if (isMultilingual)
            {
                var match = Regex.Match(permalink, @"[a-z]{2}-[a-z]{2}", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    requestCulture = new CultureInfo(match.Value);
                }
                else
                {
                    requestCulture = requestCultureFeature.RequestCulture.UICulture;
                }
            }
            else
            {
                //TODO: Assign default language from sitesettings
                requestCulture = requestCultureFeature.RequestCulture.UICulture; //Remove it
            }
            return requestCulture;
        }
    }
}
