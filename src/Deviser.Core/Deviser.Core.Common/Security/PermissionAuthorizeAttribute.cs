using Microsoft.AspNetCore.Authorization;

namespace Deviser.Core.Common.Security
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        private string _entity;
        private string _permission;
        public PermissionAuthorizeAttribute(string entity, string permission)
        {
            Entity = entity;
            Permission = permission;
            Policy = $"{Globals.POLICY_PREFIX}_{entity}_{permission}";
        }
        public string Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                Policy = $"{Globals.POLICY_PREFIX}_{_entity}_{_permission}";
            }
        }
        public string Permission
        {
            get => _permission;
            set
            {
                _permission = value;
                Policy = $"{Globals.POLICY_PREFIX}_{_entity}_{_permission}";
            }
        }
    }
}
