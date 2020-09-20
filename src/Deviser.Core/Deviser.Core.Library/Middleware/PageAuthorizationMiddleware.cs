using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Deviser.Core.Common;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Core.Library.Middleware
{
    /// <summary>
    /// Deviser Platform specific middleware which initializes the page edit and view permissions for the requested page.
    /// </summary>
    public class PageAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// Middleware specific constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public PageAuthorizationMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<PageContextMiddleware>();
        }

        /// <summary>
        /// Invokes the middleware
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var serviceProvider = httpContext.RequestServices;
            var pageManager = serviceProvider.GetService<IPageManager>();
            var scopeService = serviceProvider.GetService<IScopeService>();
            var pageContext = scopeService.PageContext;

            if (pageContext != null)
            {
                var currentPage = pageContext.CurrentPage;

                pageContext.HasPageViewPermission = pageManager.HasViewPermission(currentPage);
                pageContext.HasPageEditPermission = pageManager.HasEditPermission(currentPage);

                if (!httpContext.Items.ContainsKey("PageContext"))
                {
                    httpContext.Items.Add("PageContext", pageContext);
                }
                else
                {
                    httpContext.Items["PageContext"] = pageContext;
                }
            }

            // Call the next delegate/middleware in the pipeline
            await _next(httpContext);
        }
    }
}
