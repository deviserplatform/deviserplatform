using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Admin;
using Deviser.Admin.Extensions;
using Deviser.Core.Common.DomainTypes.Admin;
using Deviser.Modules.Blog.Models;

namespace Deviser.Modules.Blog
{
    public class AdminConfigurator : IAdminConfigurator<BlogDbContext>
    {
        public void ConfigureAdmin(IAdminSite adminSite)
        {
            //adminSite.SiteName = "TestAdmin";
            ////Register Student model, fields are automatically identified
            //adminSite.Register<Tag>();


            ////Register Student model by specifying the fields to be included with custom configuration
            //adminSite.Register<Post>(config =>
            //{
            //    config.FieldConfig
            //    .AddField(s => s.Title)
            //    .AddField(s => s.Content, new FieldOption { ValidationType = ValidationType.UserExist })
            //    .AddField(s => s.PostTags)
            //    .AddField(s => s.CreatedOn)
            //    .AddField(s => s.CreatedBy);

            //    config.Property(s => s.Content)
            //    .ValidateOn(p => p.Title == "Test");



            //    config.AddChildConfig(s => s.PostTags, (childConfig) =>
            //      {
            //          childConfig.FieldConfig.AddField(s => s.Capacity);
            //      });
            //});
        }

        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<PostTag>(form =>
            {
                form.FieldBuilder
                     .AddManyToOneField(s => s.Post, expr => expr.Title);
            });

            adminBuilder.Register<Post>(form =>
            {
                form.FieldBuilder
                     .AddField(s => s.Title)
                     //.AddField(s => s.Content, fieldOption => { fieldOption.ValidationType = ValidationType.UserExist; })
                     .AddField(s => s.Content)
                     .AddInlineManyToOneField(s => s.Category, expr => expr.Name)
                     .AddInlineManyToManyField<Tag>(s => s.PostTags, expr => expr.TagName)
                     .AddField(s => s.CreatedOn)
                     .AddField(s => s.CreatedBy);

                //form.Property(s => s.Content)
                //.ValidateOn(p => p.Title == "Test");

                form.AddChildConfig(s => s.PostTags, (childForm) =>
                  {
                      childForm.FieldBuilder.AddField(s => s.Capacity);
                  });

                //form.FieldSetBuilder
                //.AddFieldSet("General", fieldBuilder =>
                //                        fieldBuilder.AddField(s => s.FirstName).AddInlineField(s => s.LastName)
                //                    );
            });
        }
    }
}
