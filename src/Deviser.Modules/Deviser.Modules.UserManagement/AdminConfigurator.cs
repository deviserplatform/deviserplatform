﻿using Deviser.Admin;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using FieldType = Deviser.Admin.Config.FieldType;

namespace Deviser.Modules.UserManagement
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<User, UserAdminService>(modelBuilder =>
            {
                modelBuilder.GridBuilder.Title = "Users";
                modelBuilder.FormBuilder.Title = "User";

                modelBuilder.GridBuilder
                    .AddField(u => u.FirstName)
                    .AddField(u => u.LastName)
                    .AddField(u => u.Email);

                modelBuilder.FormBuilder
                    .AddKeyField(u => u.Id)
                    //.AddField(u => u.UserName, option => option.ValidationType = ValidationType.UserExist)
                    .AddField(u => u.FirstName)
                    .AddField(u => u.LastName)
                    .AddField(u => u.Password, option =>
                    {
                        option.FieldType = FieldType.Password;
                        option.ShowIn = FormMode.Create;
                        option.ValidationType = ValidationType.Password;
                    })
                    //.AddField(u => u.PasswordConfirm, option => option.AddIn = AddInMode.Add)
                    .AddField(u => u.Email, option =>
                    {
                        option.ShowIn = FormMode.Create;
                        option.ValidationType = ValidationType.UserExistByEmail;
                    })
                    .AddMultiselectField(u => u.Roles);

                modelBuilder.FormBuilder.AddFormAction("UnlockUser", "Unlock User",
                    (sp, user) => sp.GetService<UserAdminService>().UnlockUserAccount(user));

                modelBuilder.FormBuilder.AddFormAction("LockUser", "Lock User",
                    (sp, user) => sp.GetService<UserAdminService>().LockUserAccount(user));

                modelBuilder.FormBuilder.Property(u => u.Roles).HasLookup(sp => sp.GetService<IRoleRepository>().GetRoles(),
                    ke => ke.Id,
                    de => de.Name);

                modelBuilder.AddCustomForm<PasswordReset>("PasswordReset", formBuilder =>
                {
                    formBuilder.Title = "Password Reset";
                    formBuilder
                        .AddKeyField(f => f.Id)
                        .AddField(f => f.CurrentPassword, option =>
                        {
                            option.FieldType = FieldType.Password;
                            option.DisplayName = "Current Password";
                        })
                        .AddField(f => f.NewPassword, option =>
                        {
                            option.FieldType = FieldType.Password;
                            option.DisplayName = "New Password";
                            option.ValidationType = ValidationType.Password;
                        });

                    formBuilder.SetFormOption(formOption =>
                    {
                        formOption.SaveButtonText = "Reset Password";
                    });

                    formBuilder.OnSubmit((sp, form) => sp.GetService<UserAdminService>().ResetPassword(form));
                });
            });
        }
    }
}
