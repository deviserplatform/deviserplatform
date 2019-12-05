using Deviser.Admin;
using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Core.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Modules.UserManagement
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<User, AdminService>(form =>
            {
                form.Fields
                    .AddKeyField(u => u.Id)
                    .AddField(u => u.UserName)
                    .AddField(u => u.FirstName)
                    .AddField(u => u.LastName)
                    .AddField(u => u.Email)
                    .AddField(u => u.Password)
                    .AddField(u => u.PasswordConfirm)
                    .AddMultiselectField(u => u.Roles);

                form.Property(u => u.Roles).HasLookup(sp => sp.GetService<IRoleRepository>().GetRoles(),
                    ke => ke.Id,
                    de => de.Name);
            });
        }
    }
}
