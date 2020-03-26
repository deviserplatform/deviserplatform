using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deviser.Admin;
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
                builder
                    .AddKeyField(r => r.Id)
                    .AddField(r => r.Name)
                    .AddField(r => r.RecycleItemType);

                builder.AddRowAction("Restore", "Restore",
                    (provider, item) => provider.GetService<RecycleAdminService>().Restore(item));

                builder.HideEditButton();
            });
        }
    }
}
