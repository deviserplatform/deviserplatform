// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.AspNet.Mvc.Routing;
using Microsoft.AspNet.Mvc.ViewEngines;
using Microsoft.Extensions.OptionsModel;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc;

namespace Deviser.Core.Library.ViewFreatures
{
    /// <summary>
    /// Default implementation of <see cref="IRazorViewEngine"/>.
    /// </summary>
    /// <remarks>
    /// For <c>ViewResults</c> returned from controllers, views should be located in <see cref="ViewLocationFormats"/>
    /// by default. For the controllers in an area, views should exist in <see cref="AreaViewLocationFormats"/>.
    /// </remarks>
    public class DeviserViewEngine : RazorViewEngine
    {
        private const string ViewExtension = ".cshtml";
        internal const string ControllerKey = "controller";
        internal const string AreaKey = "area";
        //SKYDev changes 
        internal const string ModuleKey = "module";

        private static readonly IEnumerable<string> _viewLocationFormats = new[]
        {
            "/Views/{1}/{0}" + ViewExtension,
            "/Views/Shared/{0}" + ViewExtension,
        };

        private static readonly IEnumerable<string> _areaViewLocationFormats = new[]
        {
            "/Areas/{2}/Views/{1}/{0}" + ViewExtension,
            "/Areas/{2}/Views/Shared/{0}" + ViewExtension,
            "/Views/Shared/{0}" + ViewExtension,
        };

        private readonly IRazorPageFactory _pageFactory;
        private readonly IRazorViewFactory _viewFactory;
        private readonly IList<IViewLocationExpander> _viewLocationExpanders;
        private readonly IViewLocationCache _viewLocationCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="RazorViewEngine" /> class.
        /// </summary>
        /// <param name="pageFactory">The page factory used for creating <see cref="IRazorPage"/> instances.</param>
        public DeviserViewEngine(
            IRazorPageFactory pageFactory,
            IRazorViewFactory viewFactory,
            IOptions<RazorViewEngineOptions> optionsAccessor,
            IViewLocationCache viewLocationCache)
            : base(pageFactory, viewFactory, optionsAccessor, viewLocationCache)
        {
            _pageFactory = pageFactory;
            _viewFactory = viewFactory;
            _viewLocationExpanders = optionsAccessor.Value.ViewLocationExpanders;
            _viewLocationCache = viewLocationCache;
        }

        /// <inheritdoc />
        public new ViewEngineResult FindView(
            ActionContext context,
            string viewName)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("Argument Cannot be Null Or Empty", nameof(viewName));
            }

            var pageResult = GetRazorPageResult(context, viewName, isPartial: false);
            return CreateViewEngineResult(pageResult, _viewFactory, isPartial: false);
        }

        /// <inheritdoc />
        public new ViewEngineResult FindPartialView(
            ActionContext context,
            string partialViewName)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("Argument Cannot be Null Or Empty", nameof(partialViewName));
            }

            var pageResult = GetRazorPageResult(context, partialViewName, isPartial: true);
            return CreateViewEngineResult(pageResult, _viewFactory, isPartial: true);
        }

        /// <inheritdoc />
        public new RazorPageResult FindPage(
            ActionContext context,
            string pageName)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrEmpty(pageName))
            {
                throw new ArgumentException("Argument Cannot be Null Or Empty", nameof(pageName));
            }

            return GetRazorPageResult(context, pageName, isPartial: true);
        }


        private RazorPageResult GetRazorPageResult(
            ActionContext context,
            string pageName,
            bool isPartial)
        {
            if (IsApplicationRelativePath(pageName))
            {
                var applicationRelativePath = pageName;
                if (!pageName.EndsWith(ViewExtension, StringComparison.OrdinalIgnoreCase))
                {
                    applicationRelativePath += ViewExtension;
                }

                var page = _pageFactory.CreateInstance(applicationRelativePath);
                if (page != null)
                {
                    return new RazorPageResult(pageName, page);
                }

                return new RazorPageResult(pageName, new[] { pageName });
            }
            else
            {
                return LocatePageFromViewLocations(context, pageName, isPartial);
            }
        }

        private RazorPageResult LocatePageFromViewLocations(
            ActionContext context,
            string pageName,
            bool isPartial)
        {
            // Initialize the dictionary for the typical case of having controller and action tokens.
            var areaName = GetNormalizedRouteValue(context, AreaKey);

            //SKYDev chanages
            if (string.IsNullOrEmpty(areaName))
            {
                areaName = GetNormalizedRouteValue(context, ModuleKey);
            }

            // Only use the area view location formats if we have an area token.
            var viewLocations = !string.IsNullOrEmpty(areaName) ? AreaViewLocationFormats :
                                                                  ViewLocationFormats;

            var expanderContext = new ViewLocationExpanderContext(context, pageName, isPartial);
            if (_viewLocationExpanders.Count > 0)
            {
                expanderContext.Values = new Dictionary<string, string>(StringComparer.Ordinal);

                // 1. Populate values from viewLocationExpanders.
                // Perf: Avoid allocations
                for (var i = 0; i < _viewLocationExpanders.Count; i++)
                {
                    _viewLocationExpanders[i].PopulateValues(expanderContext);
                }
            }

            // 2. With the values that we've accumumlated so far, check if we have a cached result.
            IEnumerable<string> locationsToSearch = null;
            var cachedResult = _viewLocationCache.Get(expanderContext);
            if (!cachedResult.Equals(ViewLocationCacheResult.None))
            {
                if (cachedResult.IsFoundResult)
                {
                    var page = _pageFactory.CreateInstance(cachedResult.ViewLocation);

                    if (page != null)
                    {
                        // 2a We have a cache entry where a view was previously found.
                        return new RazorPageResult(pageName, page);
                    }
                }
                else
                {
                    locationsToSearch = cachedResult.SearchedLocations;
                }
            }

            if (locationsToSearch == null)
            {
                // 2b. We did not find a cached location or did not find a IRazorPage at the cached location.
                // The cached value has expired and we need to look up the page.
                foreach (var expander in _viewLocationExpanders)
                {
                    viewLocations = expander.ExpandViewLocations(expanderContext, viewLocations);
                }

                var controllerName = GetNormalizedRouteValue(context, ControllerKey);

                locationsToSearch = viewLocations.Select(
                    location => string.Format(
                        CultureInfo.InvariantCulture,
                        location,
                        pageName,
                        controllerName,
                        areaName
                    ));
            }

            // 3. Use the expanded locations to look up a page.
            var searchedLocations = new List<string>();
            foreach (var path in locationsToSearch)
            {
                var page = _pageFactory.CreateInstance(path);
                if (page != null)
                {
                    // 3a. We found a page. Cache the set of values that produced it and return a found result.
                    _viewLocationCache.Set(expanderContext, new ViewLocationCacheResult(path, searchedLocations));
                    return new RazorPageResult(pageName, page);
                }

                searchedLocations.Add(path);
            }

            // 3b. We did not find a page for any of the paths.
            _viewLocationCache.Set(expanderContext, new ViewLocationCacheResult(searchedLocations));
            return new RazorPageResult(pageName, searchedLocations);
        }

        private ViewEngineResult CreateViewEngineResult(
            RazorPageResult result,
            IRazorViewFactory razorViewFactory,
            bool isPartial)
        {
            if (result.SearchedLocations != null)
            {
                return ViewEngineResult.NotFound(result.Name, result.SearchedLocations);
            }

            var view = razorViewFactory.GetView(this, result.Page, isPartial);
            return ViewEngineResult.Found(result.Name, view);
        }

        private static bool IsApplicationRelativePath(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            return name[0] == '~' || name[0] == '/';
        }
    }
}
