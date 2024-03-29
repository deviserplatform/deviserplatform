﻿using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Extensions;
using Deviser.Admin.Services;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Deviser.Modules.SecurityRoles.Services
{
    public class SecurityRoleAdminService : IAdminService<Role>
    {
        private readonly ILogger<SecurityRoleAdminService> _logger;
        private readonly IRoleRepository _roleRepository;
        private readonly IPropertyRepository _propertyRepository;

        public SecurityRoleAdminService(ILogger<SecurityRoleAdminService> logger,
            IRoleRepository roleRepository,
            IPropertyRepository propertyRepository)
        {
            _logger = logger;
            _roleRepository = roleRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<PagedResult<Role>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            var Roles = _roleRepository.GetRoles();

            if (filter != null)
            {
                Roles = Roles.ApplyFilter(filter).ToList();
            }
            var result = new PagedResult<Role>(Roles, pageNo, pageSize);
            return await Task.FromResult(result);
        }

        public async Task<Role> GetItem(string roleId)
        {
            var result = _roleRepository.GetRole(Guid.Parse(roleId));
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Role>> CreateItem(Role role)
        {
            role = _roleRepository.CreateRole(role);

            if (role == null)
                return new FormResult<Role>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to create the Role"
                };

            var result = new FormResult<Role>(role)
            {
                IsSucceeded = true,
                SuccessMessage = "Role has been created"
            };
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Role>> UpdateItem(Role role)
        {
            role = _roleRepository.UpdateRole(role);
            if (role == null)
                return new FormResult<Role>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to update the Role"
                };

            var result = new FormResult<Role>(role)
            {
                IsSucceeded = true,
                SuccessMessage = "Role has been updated"
            };
            return await Task.FromResult(result);
        }

        public async Task<IAdminResult<Role>> DeleteItem(string RoleId)
        {
            var role = _roleRepository.DeleteRole(Guid.Parse(RoleId));

            if (role == null)
                return new FormResult<Role>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to delete the Role"
                };

            var result = new AdminResult<Role>(role)
            {
                IsSucceeded = true,
                SuccessMessage = "Role has been deleted"
            };
            return await Task.FromResult(result);
        }

        //public async Task<ValidationResult> ValidateRoleName(string RoleName)
        //{
        //    var result = _roleRepository.GetRole(RoleName) != null ? ValidationResult.Failed(new ValidationError() { Code = "Role available!", Description = "LayoutType already exist" }) : ValidationResult.Success;
        //    return await Task.FromResult(result);
        //}

        //public List<Property> GetProperties()
        //{
        //    var result = _propertyRepository.GetProperties();
        //    return result;
        //}
    }
}




