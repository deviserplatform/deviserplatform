using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Admin;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes;
using Deviser.Modules.ModuleManagement.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Modules.ModuleManagement
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<Module, ModuleAdminService>(modelBuilder =>
            {
                modelBuilder.AdminTitle = "Content Type";

                modelBuilder.GridBuilder
                    .AddField(c => c.Label)
                    .AddField(c => c.Name)
                    .AddField(c => c.Version)
                    .AddField(c => c.IsActiveText, option => option.DisplayName = "Is Active");
                
                modelBuilder.GridBuilder.DisplayFieldAs(c => c.IsActiveText, LabelType.Badge, c => c.IsActiveBadgeClass);

                modelBuilder.FormBuilder
                    .AddKeyField(c => c.Id)
                    .AddField(c => c.Label)
                    .AddField(c => c.Name, option => { option.EnableIn = FormMode.Create; })
                    .AddField(c => c.IsActive)
                    .AddField(c => c.Description)
                    .AddField(c => c.Version);

                modelBuilder.AddChildConfig(s => s.ModuleAction, (childForm) =>
                {
                    childForm.FormBuilder
                        .AddKeyField(c => c.Id)
                        .AddField(c => c.DisplayName, option=> option.EnableIn = FormMode.Create)
                        .AddSelectField(c => c.ModuleActionType, de=>de.ControlType)
                        .AddField(c => c.ControllerName)
                        .AddField(c => c.ControllerNamespace)
                        .AddField(c => c.IconClass)
                        .AddField(c => c.IconImage)
                        .AddField(c => c.IsDefault)
                        .AddMultiselectField(c => c.Properties, c => $"{c.Label} ({c.Name})");

                    childForm.FormBuilder.Property(c => c.Properties).HasLookup(sp => sp.GetService<ModuleAdminService>().GetProperties(),
                        ke => ke.Id,
                        de => $"{de.Label} ({de.Name})");
                });

                modelBuilder.FormBuilder.SetCustomValidationFor(c => c.Name,
                    (sp, moduleName) =>
                        sp.GetService<ModuleAdminService>().ValidateModuleName(moduleName));
            });
        }
    }
}
