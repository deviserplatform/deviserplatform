using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Internal
{
    public class DeviserRouteHandler : IRouter
    {
        private readonly IActionSelector _actionSelector;

        public DeviserRouteHandler(
            IActionSelector actionSelector)
        {
            _actionSelector = actionSelector;
        }


        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // We return null here because we're not responsible for generating the url, the route is.
            return null;
        }

        public Task RouteAsync(RouteContext context)
        {  
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //Calling this method to populate route data in PageContextMiddleware
            var candidates = _actionSelector.SelectCandidates(context);
            if (candidates == null || candidates.Count == 0)
            {
                //_logger.NoActionsMatched(context.RouteData.Values);
                return Task.CompletedTask;
            }

            context.Handler = (c) =>
            {
                var routeData = c.GetRouteData();

                return Task.CompletedTask;
            };

            return Task.CompletedTask;
        }
    }
}
