using Microsoft.AspNetCore.Builder;
using System;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Library.Middleware
{
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds the <see cref="PageContextMiddleware"/> to the specified <see cref="IApplicationBuilder"/>, which initialize <see cref="PageContext"/>.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePageContext(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<PageContextMiddleware>(/*routes.Build()*/);
        }

        /// <summary>
        /// Adds the <see cref="PageAuthorizationMiddleware"/> to the specified <see cref="IApplicationBuilder"/>, which initialize page permissions in <see cref="PageContext"/>.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePageAuthorization(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<PageAuthorizationMiddleware>();
        }
    }
}
