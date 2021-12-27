using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Admin;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes;
using Deviser.Modules.SecurityRoles.Services;

namespace Deviser.Modules.SecurityRoles
{ 
    public class AdminConfigurator:IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<Role,SecurityRoleAdminService>(modelBuilderAction: builder =>
                {
                    var gridBuilder = builder.GridBuilder;
                    var formBuilder = builder.FormBuilder;

                    gridBuilder.Title = "Security Roles";
                    formBuilder.Title = "Role";

                    gridBuilder
                        .AddKeyField(r => r.Id)
                        .AddField(r => r.Name, option => option.DisplayName = "Role Name");


                    formBuilder
                        .AddField(l => l.Name, option =>
                        {
                            option.DisplayName = "Role Name";
                        });
                });
        }
    }
}
