using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deviser.Admin;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Multilingual;
using Deviser.Modules.RecycleBin.Services;
using Microsoft.Extensions.DependencyInjection;
using FieldType = Deviser.Admin.Config.FieldType;

namespace Deviser.Modules.RecycleBin
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.RegisterGrid<RecycleItem, RecycleAdminService>(builder =>
            {
                builder.Title = "Recycle Bin";
                builder
                    .AddKeyField(r => r.Id)
                    .AddField(r => r.Name, option => option.DisplayName = "Name / Title")
                    .AddField(r => r.RecycleItemTypeString, option => option.DisplayName = "Item Type");

                builder.DisplayFieldAs(c => c.Name, LabelType.Icon, c => c.RecycleItemTypeIconClass);

                builder.AddRowAction("Restore", "Restore",
                    (provider, item) => provider.GetService<RecycleAdminService>().Restore(item));

                builder.HideEditButton();
            });
        }
    }
}
