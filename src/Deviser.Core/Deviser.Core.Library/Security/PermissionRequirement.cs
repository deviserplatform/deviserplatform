using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Deviser.Core.Library.Security
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Entity { get; set; }
        public string Permission { get; set; }

        public PermissionRequirement(string entity, string permission)
        {
            Entity = entity;
            Permission = permission;
        }
    }
}
