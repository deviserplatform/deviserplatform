using Deviser.Admin;
using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Modules.UserManagement
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<User, AdminService>(form =>
           {
               form.FieldBuilder
               .AddField(u => u.Id)
               .AddField(u => u.UserName)
               .AddField(u => u.FirstName)
               .AddField(u => u.LastName);
           });
        }
    }
}
