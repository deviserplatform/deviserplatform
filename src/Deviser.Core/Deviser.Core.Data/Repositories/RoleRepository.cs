using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        ///Logger
        private readonly ILogger<RoleRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;


        //Constructor
        public RoleRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<RoleRepository> logger,
            IMapper mapper,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        //Custom Field Declaration
        public List<Role> GetRoles()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Roles
                .OrderBy(r => r.Name)
                .ToList();
            return _mapper.Map<List<Role>>(result);
        }

        public List<Role> GetRoles(string userName)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var user = context.Users.FirstOrDefault(u => u.UserName.ToLower() == userName);
            
            if (user == null) throw new InvalidOperationException($"User not found {userName}");

            var result = context.Roles
                .Include(r => r.UserRoles)
                .Where(r => r.UserRoles.Any(u => u.UserId == user.Id))
                .OrderBy(r => r.Name)
                .ToList();
            return _mapper.Map<List<Role>>(result);
        }

        public Role GetRole(Guid roleId)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Roles
                .FirstOrDefault(e => e.Id == roleId);

            return _mapper.Map<Role>(result);
        }

        public Role GetRoleByName(string roleName)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Roles
                .FirstOrDefault(e => e.Name == roleName);

            return _mapper.Map<Role>(result);
        }

        public Role CreateRole(Role role)
        {
            var dbRole = _mapper.Map<Entities.Role>(role);
            var rm = _serviceProvider.GetService<RoleManager<Entities.Role>>();
            var result = rm.CreateAsync(dbRole).GetAwaiter().GetResult();
            if (!result.Succeeded) throw new InvalidOperationException($"Unable to create new role, {result.Errors}");
            return role;
        }
        public Role UpdateRole(Role role)
        {
            var rm = _serviceProvider.GetService<RoleManager<Entities.Role>>();
            var dbRole = rm.Roles.FirstOrDefault(r => r.Id == role.Id);

            if (dbRole == null) return null;

            dbRole.Name = role.Name;
            var result = rm.UpdateAsync(dbRole).GetAwaiter().GetResult();
            if (!result.Succeeded) throw new InvalidOperationException($"Unable to update role, {result.Errors}");
            return role;
        }
        public Role DeleteRole(Guid roleId)
        {
            var rm = _serviceProvider.GetService<RoleManager<Entities.Role>>();
            var role = rm.Roles
                .FirstOrDefault(e => e.Id == roleId);
            var result = rm.DeleteAsync(role).Result;
            return _mapper.Map<Role>(role);
        }

    }

}//End namespace
