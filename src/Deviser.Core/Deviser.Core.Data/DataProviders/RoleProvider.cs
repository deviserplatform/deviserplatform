using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Deviser.Core.Data.DataProviders
{

    public interface IRoleProvider
    {
        List<Role> GetRoles();
        Role GetRole(string roleId);
        Role GetRoleByName(string roleName);
        Role CreateRole(Role role);
        Role UpdateRole(Role role);
        Role DeleteRole(string roleId);

    }

    public class RoleProvider : DataProviderBase, IRoleProvider
    {
        ///Logger
        private readonly ILogger<LayoutProvider> logger;

        //Constructor
        public RoleProvider(ILifetimeScope container)
            :base(container)
        {
            logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        //Custom Field Declaration
        public List<Role> GetRoles()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<Role> returnData = context.Roles.ToList();
                    return new List<Role>(returnData); 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetRoles", ex);
            }
            return null;
        }

        public Role GetRole(string roleId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Role returnData = context.Roles
                              .Where(e => e.Id == roleId)
                              .FirstOrDefault();

                    return returnData; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling GetRole", ex);
            }
            return null;
        }

        public Role GetRoleByName(string roleName)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Role returnData = context.Roles
                              .Where(e => e.Name == roleName)
                              .FirstOrDefault();
                    return returnData; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling GetRoleByName", ex);
            }
            return null;
        }
        public Role CreateRole(Role role)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    RoleManager<Role> rm = container.Resolve<RoleManager<Role>>();
                    var result = rm.CreateAsync(role).Result;

                    //Role resultRole;
                    //role.Id = Guid.NewGuid().ToString();
                    //resultRole = context.Roles.Add(role).Entity;
                    //context.SaveChanges();
                    return role; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling CreateRole", ex);
            }
            return null;
        }
        public Role UpdateRole(Role role)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    RoleManager<Role> rm = container.Resolve<RoleManager<Role>>();
                    var result = rm.UpdateAsync(role).Result;

                    //Role resultRole;
                    //resultRole = context.Roles.Attach(role).Entity;
                    //context.Entry(role).State = EntityState.Modified;

                    //context.SaveChanges();
                    return role; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdateRole", ex);
            }
            return null;
        }
        public Role DeleteRole(string roleId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    RoleManager<Role> rm = container.Resolve<RoleManager<Role>>();
                    var role = rm.Roles.Where(e => e.Id == roleId)
                        .FirstOrDefault();
                    var result = rm.DeleteAsync(role).Result;


                    //Role resultRole;
                    //var deleteObj = context.Roles
                    //.Where(e => e.Id == roleId)
                    //    .FirstOrDefault();

                    //resultRole = context.Roles.Remove(deleteObj).Entity;
                    //context.SaveChanges();
                    return role; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling DeleteRole", ex);
            }
            return null;
        }

    }

}//End namespace
