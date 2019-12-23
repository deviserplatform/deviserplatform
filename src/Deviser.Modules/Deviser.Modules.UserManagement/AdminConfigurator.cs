using Deviser.Admin;
using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Core.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using FieldType = Deviser.Core.Common.DomainTypes.Admin.FieldType;

namespace Deviser.Modules.UserManagement
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<User, UserAdminService>(modelBuilder =>
            {
                modelBuilder.FormBuilder
                    .AddKeyField(u => u.Id)
                    .AddField(u => u.UserName)
                    .AddField(u => u.FirstName)
                    .AddField(u => u.LastName)
                    .AddField(u => u.Email)
                    .AddField(u => u.Password)
                    .AddField(u => u.PasswordConfirm)
                    .AddMultiselectField(u => u.Roles);

                modelBuilder.Property(u => u.Roles).HasLookup(sp => sp.GetService<IRoleRepository>().GetRoles(),
                    ke => ke.Id,
                    de => de.Name);

                modelBuilder.AddCustomForm<PasswordReset>("PasswordReset", formBuilder =>
                {
                    formBuilder.AddField(f => f.CurrentPassword, option =>
                    {
                        option.FieldType = FieldType.Password;
                        option.DisplayName = "Current Password";
                    });
                    formBuilder.AddField(f => f.NewPassword, option =>
                    {
                        option.FieldType = FieldType.Password;
                        option.DisplayName = "New Password";
                    });

                    formBuilder.SetFormOption(formOption =>
                    {
                        formOption.EditButtonText = "Reset Password";
                        formOption.FormTitle = "Password Reset";
                        formOption.SaveButtonText = "Reset Password";
                    });

                    formBuilder.SetFormSubmitAction((sp, form) => sp.GetService<UserAdminService>().ResetPassword(form));
                });
            });
        }
    }
}
