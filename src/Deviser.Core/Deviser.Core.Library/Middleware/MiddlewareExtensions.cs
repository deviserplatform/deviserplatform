using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UsePageContext(this IApplicationBuilder builder, Action<IRouteBuilder> configureRoutes)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configureRoutes == null)
            {
                throw new ArgumentNullException(nameof(configureRoutes));
            }

            // Verify if AddMvc was done before calling UseMvc
            // We use the MvcMarkerService to make sure if all the services were added.
            if (builder.ApplicationServices.GetService(typeof(MvcMarkerService)) == null)
            {
                throw new InvalidOperationException("FormatUnableToFindServices" +
                    "AddMvc" +
                    "ConfigureServices(...)");
            }

            var routes = new RouteBuilder(builder)
            {
                DefaultHandler = builder.ApplicationServices.GetRequiredService<MvcRouteHandler>(),
            };

            configureRoutes(routes);

            routes.Routes.Insert(0, AttributeRouting.CreateAttributeMegaRoute(builder.ApplicationServices));

            return builder.UseMiddleware<PageContextMiddleware>(routes.Build());
        }

        //public static IApplicationBuilder UsePageContext(
        //    this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<PageContextMiddleware>();
        //}

        //public static IApplicationBuilder UseRouteMiddleware(this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<RouteMiddleware>();
        //}
    }
}
