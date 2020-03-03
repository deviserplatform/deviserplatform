using Deviser.Admin;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes;
using Deviser.Modules.ContentManagement.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Modules.ContentManagement
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<ContentType, ContentTypeAdminService>(modelBuilder =>
            {
                modelBuilder.AdminTitle = "Content Type";

                modelBuilder.GridBuilder
                    .AddField(c => c.Label)
                    .AddField(c => c.Name)
                    .AddField(c => c.IsActiveText, option => option.DisplayName = "Is Active");

                modelBuilder.GridBuilder.DisplayFieldAs(c => c.Label, LabelType.Icon, c => c.IconClass);
                modelBuilder.GridBuilder.DisplayFieldAs(c => c.IsActiveText, LabelType.Badge, c => c.IsActiveBadgeClass);

                modelBuilder.FormBuilder
                    .AddKeyField(c => c.Id)
                    .AddField(c => c.Label)
                    .AddField(c => c.Name, option =>
                    {
                        option.EnableIn = FormMode.Create;
                    })
                    .AddField(c => c.IconClass)
                    .AddField(c => c.IsActive)
                    .AddMultiselectField(c => c.Properties, c => $"{c.Label} ({c.Name})");



                modelBuilder.FormBuilder.Property(c => c.Properties).HasLookup(sp => sp.GetService<ContentTypeAdminService>().GetProperties(),
                    ke => ke.Id,
                    de => $"{de.Label} ({de.Name})");

                modelBuilder.FormBuilder.SetCustomValidationFor(c => c.Name,
                    (sp, contentTypeName) =>
                        sp.GetService<ContentTypeAdminService>().ValidateContentTypeName(contentTypeName));
            });

            adminBuilder.Register<LayoutType, LayoutTypeAdminService>(modelBuilder =>
            {
                modelBuilder.AdminTitle = "Layout Type";

                modelBuilder.GridBuilder
                    .AddField(c => c.Label)
                    .AddField(c => c.Name)
                    .AddField(c => c.IsActiveText, option => option.DisplayName = "Is Active");

                modelBuilder.GridBuilder.DisplayFieldAs(c => c.Label, LabelType.Icon, c => c.IconClass);
                modelBuilder.GridBuilder.DisplayFieldAs(c => c.IsActiveText, LabelType.Badge, c => c.IsActiveBadgeClass);

                modelBuilder.FormBuilder
                    .AddKeyField(c => c.Id)
                    .AddField(c => c.Label)
                    .AddField(c => c.Name, option =>
                    {
                        option.EnableIn = FormMode.Create;
                    })
                    .AddField(c => c.IconClass)
                    .AddField(c => c.IsActive)
                    .AddMultiselectField(c=>c.AllowedLayoutTypes)
                    .AddMultiselectField(c => c.Properties, c => $"{c.Label} ({c.Name})");


                modelBuilder.FormBuilder.Property(c => c.AllowedLayoutTypes).HasLookup(sp => sp.GetService<LayoutTypeAdminService>().GetLayoutTypes(),
                    ke => ke.Id,
                    de => $"{de.Label} ({de.Name})");

                modelBuilder.FormBuilder.Property(c => c.Properties).HasLookup(sp => sp.GetService<LayoutTypeAdminService>().GetProperties(),
                    ke => ke.Id,
                    de => $"{de.Label} ({de.Name})");

                modelBuilder.FormBuilder.SetCustomValidationFor(c => c.Name,
                    (sp, contentTypeName) =>
                        sp.GetService<LayoutTypeAdminService>().ValidateLayoutTypeName(contentTypeName));
            });


        }
    }
}
