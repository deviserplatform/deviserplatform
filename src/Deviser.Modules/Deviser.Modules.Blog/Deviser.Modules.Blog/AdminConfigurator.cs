using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Admin;
using Deviser.Admin.Extensions;
using Deviser.Modules.Blog.Models;

namespace Deviser.Modules.Blog
{
    public class AdminConfigurator : IAdminConfigurator<BlogDbContext>
    {
        public void ConfigureAdmin(IAdminSite adminSite)
        {
            adminSite.SiteName = "TestAdmin";
            //Register Student model, fields are automatically identified
            adminSite.Register<Tag>();


            //Register Student model by specifying the fields to be included with custom configuration
            adminSite.Register<Post>(config =>
            {
                config.FieldConfig
                .AddField(s => s.Title)
                .AddField(s => s.Content)
                .AddField(s => s.CreatedOn)
                .AddField(s => s.CreatedBy);
            });
        }
    }
}
