using Deviser.Admin;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes;
using Deviser.Modules.ContentManagement.Services;
using Microsoft.Extensions.DependencyInjection;
using FieldType = Deviser.Admin.Config.FieldType;

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
                    .AddField(c => c.IsActive, option =>
                    {
                        option.DisplayName = "Is Active";
                        option.IsTrue = "Active";
                        option.IsFalse = "In Active";
                    });

                modelBuilder.GridBuilder.DisplayFieldAs(c => c.Label, LabelType.Icon, c => c.IconClass);
                modelBuilder.GridBuilder.DisplayFieldAs(c => c.IsActive, LabelType.Badge, c => c.IsActiveBadgeClass);

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
                    .AddField(l => l.Label)
                    .AddField(l => l.Name)
                    .AddField(l => l.IsActive, option =>
                    {
                        option.DisplayName = "Is Active";
                        option.IsTrue = "Active";
                        option.IsFalse = "In Active";
                    });

                modelBuilder.GridBuilder.DisplayFieldAs(l => l.Label, LabelType.Icon, l => l.IconClass);
                modelBuilder.GridBuilder.DisplayFieldAs(l => l.IsActive, LabelType.Badge, l => l.IsActiveBadgeClass);

                modelBuilder.FormBuilder
                    .AddKeyField(l => l.Id)
                    .AddField(l => l.Label)
                    .AddField(l => l.Name, option =>
                    {
                        option.EnableIn = FormMode.Create;
                    })
                    .AddField(l => l.IconClass)
                    .AddField(l => l.IsActive)
                    .AddMultiselectField(l => l.AllowedLayoutTypes)
                    .AddMultiselectField(l => l.Properties, c => $"{c.Label} ({c.Name})");


                modelBuilder.FormBuilder.Property(l => l.AllowedLayoutTypes).HasLookup(sp => sp.GetService<LayoutTypeAdminService>().GetLayoutTypes(),
                    ke => ke.Id,
                    de => $"{de.Label} ({de.Name})");

                modelBuilder.FormBuilder.Property(l => l.Properties).HasLookup(sp => sp.GetService<LayoutTypeAdminService>().GetProperties(),
                    ke => ke.Id,
                    de => $"{de.Label} ({de.Name})");

                modelBuilder.FormBuilder.SetCustomValidationFor(l => l.Name,
                    (sp, layoutTypeName) =>
                        sp.GetService<LayoutTypeAdminService>().ValidateLayoutTypeName(layoutTypeName));
            });

            adminBuilder.Register<Property, PropertyAdminService>(modelBuilder =>
            {
                modelBuilder.AdminTitle = "Properties";

                modelBuilder.GridBuilder
                    .AddField(p => p.Label)
                    .AddField(p => p.Name)
                    .AddField(p => p.IsActive, option =>
                    {
                        option.DisplayName = "Is Active";
                        option.IsTrue = "Active";
                        option.IsFalse = "In Active";
                    });

                modelBuilder.GridBuilder.DisplayFieldAs(c => c.IsActive, LabelType.Badge, c => c.IsActiveBadgeClass);

                modelBuilder.FormBuilder
                    .AddKeyField(p => p.Id)
                    .AddField(p => p.Label)
                    .AddField(p => p.Name, option => { option.EnableIn = FormMode.Create; })
                    .AddField(p => p.Description, option => { option.FieldType = FieldType.TextArea; })
                    .AddField(p => p.IsActive)

                    .AddField(p => p.IsMoreOption)
                    .AddField(p => p.DefaultValue, option => { option.IsRequired = false; })
                    .AddSelectField(p => p.OptionList, p => p.Label, option => { option.IsRequired = false; })
                    .AddSelectField(p => p.DefaultValuePropertyOption, p => p.Label, option => { option.IsRequired = false; });

                modelBuilder.FormBuilder.Property(p => p.DefaultValue)
                    .ShowOn(p => !p.IsMoreOption)
                    .ValidateOn(p => !p.IsMoreOption);

                modelBuilder.FormBuilder.Property(p => p.OptionList)
                    .ShowOn(p => p.IsMoreOption)
                    .ValidateOn(p => p.IsMoreOption);

                modelBuilder.FormBuilder.Property(p => p.DefaultValuePropertyOption)
                    .ShowOn(p => p.IsMoreOption)
                    .ValidateOn(p => p.IsMoreOption);

                modelBuilder.FormBuilder.Property(p => p.OptionList).HasLookup(sp => sp.GetService<PropertyAdminService>().GetOptionList(),
                    ke => ke.Id,
                    de => $"{de.Label}");

                modelBuilder.FormBuilder.Property(p => p.DefaultValuePropertyOption).HasLookup((sp, filterParam) => sp.GetService<PropertyAdminService>().GetPropertyOption(filterParam),
                    ke => ke.Id,
                    de => $"{de.Label} ({de.Name})",
                    property => property.OptionList);

                modelBuilder.FormBuilder.SetCustomValidationFor(p => p.Name,
                    (sp, propertyName) =>
                        sp.GetService<PropertyAdminService>().ValidatePropertyName(propertyName));
            });

            adminBuilder.Register<OptionList, OptionListAdminService>(modelBuilder =>
            {
                modelBuilder.AdminTitle = "Option List";

                modelBuilder.GridBuilder
                    .AddField(p => p.Label)
                    .AddField(p => p.Name)
                    .AddField(p => p.IsActive, option =>
                    {
                        option.DisplayName = "Is Active";
                        option.IsTrue = "Active";
                        option.IsFalse = "In Active";
                    });

                modelBuilder.GridBuilder.DisplayFieldAs(c => c.IsActive, LabelType.Badge, c => c.IsActiveBadgeClass);

                modelBuilder.FormBuilder
                    .AddKeyField(p => p.Id)
                    .AddField(p => p.Label)
                    .AddField(p => p.Name, option => { option.EnableIn = FormMode.Create; })
                    .AddField(p => p.IsActive);

                modelBuilder.AddChildConfig(s => s.List, (childForm) =>
                {
                    childForm.GridBuilder
                        .AddField(c => c.Name)
                        .AddField(c => c.Label);

                    childForm.FormBuilder
                        .AddKeyField(c => c.Id)
                        .AddField(c => c.Name)
                        .AddField(c => c.Label);
                });

                modelBuilder.FormBuilder.SetCustomValidationFor(p => p.Name,
                    (sp, propertyName) =>
                        sp.GetService<OptionListAdminService>().ValidatePropertyName(propertyName));
            });


        }
    }
}
