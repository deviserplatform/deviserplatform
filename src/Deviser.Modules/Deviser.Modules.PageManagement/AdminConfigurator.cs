using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deviser.Admin;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Multilingual;
using Deviser.Modules.PageManagement.Services;
using Microsoft.Extensions.DependencyInjection;
using FieldType = Deviser.Admin.Config.FieldType;

namespace Deviser.Modules.PageManagement
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {

            //adminBuilder.RegisterGrid<Page, PageManagementAdminService>(builder =>
            //{
            //    builder
            //        .AddKeyField(r => r.Id)
            //        .AddField(r => r.Name, option => option.DisplayName = "Name / Title")
            //        .AddField(r => r.RecycleItemTypeString, option => option.DisplayName = "Item Type");

            //    builder.DisplayFieldAs(c => c.Name, LabelType.Icon, c => c.RecycleItemTypeIconClass);

            //    builder.AddRowAction("Restore", "Restore",
            //        (provider, item) => provider.GetService<PageManagementAdminService>().Restore(item));

            //    builder.HideEditButton();
            //});

            adminBuilder.RegisterTreeAndForm<PageViewModel, PageManagementAdminService>(builder =>
            {
                builder.TreeBuilder.ConfigureTree(p => p.Id, p => p.Name, p => p.ChildPage, p => p.PageOrder);

                var formBuilder = builder.FormBuilder;

                formBuilder
                     .AddFieldSet("General", fieldBuilder =>
                     {
                         fieldBuilder
                             .AddSelectField(p => p.PageType, null, option => option.DisplayName = "Page Type")
                             .AddField(p => p.Name)
                             .AddField(p => p.Title)
                             .AddField(p => p.Description, option =>
                             {
                                 option.FieldType = FieldType.TextArea;
                                 option.IsRequired = false;
                             })
                             .AddField(p => p.PageHeaderTags, option => option.FieldType = FieldType.TextArea)
                             .AddSelectField(p => p.Module)
                             .AddField(p => p.ModelName, option => option.DisplayName = "Model Name")
                             .AddField(p => p.RedirectUrl, option => option.DisplayName = "Redirect Link")
                             .AddField(p => p.IsSystem, option => option.DisplayName = "Is System")
                             .AddField(p => p.IsIncludedInMenu, option => option.DisplayName = "Include in Menu")
                             .AddSelectField(p => p.Theme, null, option => option.DisplayName = "Theme");
                     })

                     .AddFieldSet("Permissions", fieldBuilder =>
                     {
                         //fieldBuilder.add
                     });

                formBuilder.Property(f => f.Name)
                    .ShowOn(f => f.PageType.Name == "Standard")
                    .ValidateOn(f => f.PageType.Name == "Standard");

                formBuilder.Property(f => f.Title)
                    .ShowOn(f => f.PageType.Name == "Standard")
                    .ValidateOn(f => f.PageType.Name == "Standard");

                formBuilder.Property(f => f.Description)
                    .ShowOn(f => f.PageType.Name == "Standard");

                formBuilder.Property(f => f.Module)
                    .ShowOn(f => f.PageType.Name == "Admin")
                    .ValidateOn(f => f.PageType.Name == "Admin");

                formBuilder.Property(f => f.ModelName)
                    .ShowOn(f => f.PageType.Name == "Admin")
                    .ValidateOn(f => f.PageType.Name == "Admin");

                formBuilder.Property(f => f.RedirectUrl)
                    .ShowOn(f => f.PageType.Name == "URL")
                    .ValidateOn(f => f.PageType.Name == "URL");

                formBuilder.Property(f => f.PageType).HasLookup(
                     sp => sp.GetService<PageManagementAdminService>().GetPageTypes(),
                     ke => ke.Id,
                     de => de.Name);

                formBuilder.Property(f => f.Module).HasLookup(
                     sp => sp.GetService<PageManagementAdminService>().GetModules(),
                     ke => ke.Id,
                     de => de.Name);

                formBuilder.Property(f => f.Theme).HasLookup(
                     sp => sp.GetService<PageManagementAdminService>().GetThemes(),
                     ke => ke.Key,
                     de => de.Value);





                //builde.AddSelectField(p => p.Theme, null, option => option.DisplayName = "Theme");r.FormBuilder.AddSelectField(p => p.Locale, null, option => option.DisplayName = "Locale");
                //builder.FormBuilder.AddSelectField(p => p.PageType, null, option => option.DisplayName = "Page Type");
                //builder.FormBuilder.AddField(p => p.IsSystem, option => option.DisplayName = "Is System");
                //builder.FormBuilder.AddField(p => p.IsIncludedInMenu, option => option.DisplayName = "Include in Menu");
                //builder.FormBuilder.AddSelectField(p => p.Theme, null, option => option.DisplayName = "Theme");
            });
        }
    }
}
