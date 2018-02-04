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
using Deviser.Core.Common.DomainTypes;
using System.Globalization;
using System.Text.RegularExpressions;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Common;
using Autofac;

namespace Deviser.Core.Library.Infrastructure
{
    public class PageUrlConstraint : IRouteConstraint
    {
        protected IPageProvider pageProvider;
        protected ILanguageProvider languageProvider;
        protected ILifetimeScope container;
        protected IInstallationProvider installationProvider;
        private bool isEverythingInstalled;

        //public PageUrlConstraint(IPageProvider pageProvider, ILanguageProvider languageProvider)
        //{
        //    this.pageProvider = pageProvider;
        //    this.languageProvider = languageProvider;
        //}

        public PageUrlConstraint(ILifetimeScope container)
        {
            this.container = container;
            installationProvider = container.Resolve<IInstallationProvider>();
            isEverythingInstalled = installationProvider.IsPlatformInstalled && installationProvider.IsDatabaseExist;
            if (isEverythingInstalled)
            {
                pageProvider = container.Resolve<IPageProvider>();
                languageProvider = container.Resolve<ILanguageProvider>();
            }
            
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!isEverythingInstalled)
                return false;

            if (values[routeKey] != null)
            {
                var permalink = values[routeKey].ToString();
                var currentCulture = GetCurrentCulture(httpContext, permalink); // Have  to use this since it will be clled before InitPageContext()
                var pages = pageProvider.GetPageTranslations(currentCulture.ToString());
                if (pages != null && pages.Count > 0)
                {
                    var result = pages.Any(p => (p != null && p.URL.ToLower() == permalink.ToLower()));
                    return result;
                }
            }
            return false;
        }

        private CultureInfo GetCurrentCulture(HttpContext httpContext, string permalink)
        {
            var requestCultureFeature = httpContext.Features.Get<IRequestCultureFeature>();
            var isMultilingual = languageProvider.IsMultilingual();
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
