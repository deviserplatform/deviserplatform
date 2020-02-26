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
                modelBuilder.GridBuilder
                    .AddField(c => c.Label)
                    .AddField(c => c.Name)
                    .AddField(c => c.IsActive);

                modelBuilder.FormBuilder
                    .AddKeyField(c => c.Id)
                    .AddField(c => c.Label)
                    .AddField(c => c.Name, option =>
                    {
                        option.EnableIn = FormMode.Create;
                    })
                    .AddField(c => c.IconClass)
                    .AddField(c => c.IsActive);

                modelBuilder.FormBuilder.SetCustomValidationFor(c => c.Name,
                    (sp, contentTypeName) =>
                        sp.GetService<ContentTypeAdminService>().ValidateContentTypeName(contentTypeName));
            });
        }
    }
}
