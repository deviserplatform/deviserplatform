using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Admin;
using Deviser.Core.Common.DomainTypes;
using Deviser.Modules.ContentManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace Deviser.Modules.ContentManagement
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<ContentType, ContentTypeAdminService>(modelBuilder =>
            {
                modelBuilder.GridBuilder
                    .AddField(c => c.Label)
                    .AddField(c => c.Name);

                modelBuilder.FormBuilder
                    .AddKeyField(c=>c.Id)
                    .AddField(c => c.Label)
                    .AddField(c => c.Name)
                    .AddField(c => c.IconClass);
            });
        }
    }
}
