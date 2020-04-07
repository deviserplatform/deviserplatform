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

            adminBuilder.RegisterTreeAndForm<Page, PageManagementAdminService>(builder =>
                {
                    builder.TreeBuilder.ConfigureTree(p => p.Id, p => p.PageName, p => p.ChildPage);
                });
        }
    }
}
