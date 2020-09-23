using Deviser.Core.Common;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Routing;

namespace Deviser.Core.Library.Security
{
    public class PermissionHandler : IAuthorizationHandler
    {
        //private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IPermissionRepository _permissionRepository;
        //private readonly UserManager<Core.Data.Entities.User> _userManager;
        //private readonly IUserRepository _userRepository;

        public PermissionHandler(/*IActionContextAccessor actionContextAccessor, */
            IHttpContextAccessor httpContextAccessor
            /*IPermissionRepository permissionRepository,
            UserManager<Core.Data.Entities.User> userManager,
            IUserRepository userRepository*/)
        {
            //_actionContextAccessor = actionContextAccessor;
            _httpContextAccessor = httpContextAccessor;

        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();
            var httpContext = _httpContextAccessor.HttpContext;
            var scopeService = httpContext.RequestServices.GetService<IScopeService>();
            var userRepository = httpContext.RequestServices.GetService<IUserRepository>();
            var permissionRepository = httpContext.RequestServices.GetService<IPermissionRepository>();
            var languageRepository = httpContext.RequestServices.GetService<ILanguageRepository>();
            var permissions = permissionRepository.GetPagePermissions();
            var isUserAuthenticated = context.User.Identity.IsAuthenticated;
            var userRoles = new Dictionary<Guid, Role>();

            if (isUserAuthenticated)
            {
                var user = userRepository.GetUser(context.User.Identity.Name);
                userRoles = user.Roles.ToDictionary(k => k.Id, v => v);
            }

            //var user = await _userManager.GetUserAsync(context.User);
            //var userRoles = await _userManager.GetRolesAsync(user);




            var routeData = _httpContextAccessor.HttpContext.GetRouteData();
            var activeLanguages = languageRepository.GetActiveLanguages();

            var currentPage = scopeService.PageContext.CurrentPage;

            foreach (var requirement in pendingRequirements)
            {
                if (!(requirement is PermissionRequirement permissionRequirement)) continue;

                var requiredPermission = permissions.Single(p =>
                    string.Equals(p.Entity, permissionRequirement.Entity,
                        StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(p.Name, permissionRequirement.Permission,
                        StringComparison.InvariantCultureIgnoreCase));

                if (!isUserAuthenticated && requiredPermission.Name == "VIEW" && currentPage.PagePermissions.Any(p => p.RoleId == Globals.AllUsersRoleId))
                {
                    //PagePermission VIEW is assigned to AllUsers (AllowAnonymous)
                    context.Succeed(requirement);
                }

                if (HasPagePermission(currentPage, userRoles, requiredPermission))
                {
                    context.Succeed(requirement);
                }
            }

            //TODO: Use the following if targeting a version of
            //.NET Framework older than 4.6:
            //      return Task.FromResult(0);
            return Task.CompletedTask;
        }

        private static bool HasPagePermission(Page currentPage, Dictionary<Guid, Role> userRoles, Permission requiredPermission)
        {
            var pagePermission = currentPage.PagePermissions.Any(p => userRoles.ContainsKey(p.RoleId) && p.PermissionId == requiredPermission.Id);
            return pagePermission;
        }
    }
}
