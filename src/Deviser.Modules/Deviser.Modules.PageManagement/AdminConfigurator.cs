using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deviser.Admin;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
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
                builder.TreeBuilder.ConfigureTree(p => p.Id,
                    p => p.Name,
                    p => p.Parent,
                    p => p.ChildPage,
                    p => p.PageOrder);

                var formBuilder = builder.FormBuilder;
                var adminId = Guid.Parse("5308b86c-a2fc-4220-8ba2-47e7bec1938d");
                var urlId = Guid.Parse("bfefa535-7af1-4ddc-82c0-c906c948367a");
                var standardId = Guid.Parse("4c06dcfd-214f-45af-8404-ff84b412ab01");

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
                             .AddField(p => p.PageHeaderTags, option =>
                             {   
                                 option.FieldType = FieldType.TextArea;
                                 option.IsRequired = false;
                             })
                             .AddSelectField(p => p.Module)
                             .AddField(p => p.ModelName, option => option.DisplayName = "Model Name")
                             .AddField(p => p.RedirectUrl, option => option.DisplayName = "Redirect Link")
                             .AddField(p => p.IsSystem, option => option.DisplayName = "Is System")
                             .AddField(p => p.IsIncludedInMenu, option => option.DisplayName = "Include in Menu")
                             .AddSelectField(p => p.Theme, null, option => option.DisplayName = "Theme");
                     })

                     .AddFieldSet("Permissions", fieldBuilder =>
                     {
                         fieldBuilder.AddCheckBoxMatrix(p => p.PagePermissions,
                             p => p.RoleId,
                             p => p.PermissionId,
                             p => p.Id,
                             p => p.PageId, typeof(Role), typeof(Permission),
                             option => option.IsRequired = false);
                     });

                builder.AddChildConfig(p => p.PageTranslation, modelBuilder =>
                {
                    var formBuilder = modelBuilder.FormBuilder;

                    modelBuilder.GridBuilder
                        .AddField(p => p.Language.EnglishName, option => option.DisplayName = "Language")
                        .AddField(p => p.Name)
                        .AddField(p => p.Title);

                    formBuilder
                        .AddSelectField(p => p.Language)
                        .AddField(p => p.Name)
                        .AddField(p => p.Title)
                        .AddField(p => p.Description, option =>
                        {
                            option.FieldType = FieldType.TextArea;
                            option.IsRequired = false;
                        })
                        .AddField(p => p.PageHeaderTags)
                        .AddField(p => p.RedirectUrl);

                    formBuilder.Property(p => p.Language)
                        .HasLookup(sp => sp.GetService<PageManagementAdminService>().GetTranslateLanguages(),
                            language => language.CultureCode,
                            language => language.EnglishName);
                });

                builder.ShowChildConfigOn(p => p.PageTranslation,
                    provider => provider.GetService<PageManagementAdminService>().IsSiteMultilingual());

                formBuilder.Property(f => f.Name)
                    .ShowOn(f => f.PageType != null && (f.PageType.Id == standardId || f.PageTypeId == adminId))
                    .ValidateOn(f => f.PageType != null && (f.PageType.Id == standardId || f.PageTypeId == adminId));

                formBuilder.Property(f => f.Title)
                    .ShowOn(f => f.PageType != null && f.PageType.Id == standardId)
                    .ValidateOn(f => f.PageType != null && f.PageType.Id == standardId);

                formBuilder.Property(f => f.Description)
                    .ShowOn(f => f.PageType != null && f.PageType.Id == standardId);

                formBuilder.Property(f => f.Module)
                    .ShowOn(f => f.PageType != null && f.PageType.Id == adminId)
                    .ValidateOn(f => f.PageType != null && f.PageType.Id == adminId);

                formBuilder.Property(f => f.ModelName)
                    .ShowOn(f => f.PageType != null && f.PageType.Id == adminId)
                    .ValidateOn(f => f.PageType != null && f.PageType.Id == adminId);

                formBuilder.Property(f => f.RedirectUrl)
                    .ShowOn(f => f.PageType != null && f.PageType.Id == urlId)
                    .ValidateOn(f => f.PageType != null && f.PageType.Id == urlId);

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

                formBuilder.Property(f => f.PagePermissions).HasMatrixLookup<Role, Permission, Guid>(
                    sp => sp.GetService<IRoleRepository>().GetRoles(),
                    ke => ke.Id,
                    de => de.Name,
                    sp => sp.GetService<IPermissionRepository>().GetPagePermissions(),
                    ke => ke.Id,
                    de => de.Name);
            });
        }
    }
}
