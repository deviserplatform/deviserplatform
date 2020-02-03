﻿using Deviser.Core.Data.Repositories;
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
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IInstallationProvider _installationProvider;
        private readonly  bool _isEverythingInstalled;
        
        public PageUrlConstraint(IInstallationProvider installationProvider,
            IPageRepository pageRepository,
            ILanguageRepository languageRepository)
        {
            _installationProvider = installationProvider;
            _isEverythingInstalled = _installationProvider.IsPlatformInstalled && _installationProvider.IsDatabaseExist;
            if (_isEverythingInstalled)
            {
                _pageRepository = pageRepository;
                _languageRepository = languageRepository;
            }
            
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!_isEverythingInstalled)
                return false;

            if (values[routeKey] != null)
            {
                var permalink = values[routeKey].ToString();
                var currentCulture = GetCurrentCulture(httpContext, permalink); // Have  to use this since it will be clled before InitPageContext()
                var pageTranslations = _pageRepository.GetPageTranslations(currentCulture.ToString());
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
            var isMultilingual = _languageRepository.IsMultilingual();
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
