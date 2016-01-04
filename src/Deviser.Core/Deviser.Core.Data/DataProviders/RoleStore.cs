/*using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Data.Entity;

namespace Deviser.Core.Data.DataProviders
{
    public class RoleStore : IQueryableRoleStore<Role, string>
    {
        private readonly DeviserContext db;

        public RoleStore(DeviserContext db)
        {
            this.db = db;
        }

        //// IQueryableRoleStore<UserRole, TKey>

        public IQueryable<Role> Roles
        {
            get { return this.db.Roles; }
        }

        //// IRoleStore<UserRole, TKey>

        public virtual Task CreateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            try
            {
                role.Id = Guid.NewGuid().ToString();
                this.db.Roles.Add(role);
                return this.db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task DeleteAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.Roles.Remove(role);
            return this.db.SaveChangesAsync();
        }

        public Task<Role> FindByIdAsync(string roleId)
        {
            return this.db.Roles.FindAsync(new[] { roleId });
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return this.db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public Task UpdateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.Entry(role).State = EntityState.Modified;
            return this.db.SaveChangesAsync();
        }

        //// IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.db != null)
            {
                this.db.Dispose();
            }
        }
    }
}
*/