using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Deviser.Admin;
using Deviser.Admin.Extensions;
using Deviser.Core.Data.Repositories;
using Deviser.Modules.Blog.DTO;
using Deviser.Modules.Blog.Models;
using Deviser.Modules.Blog.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Modules.Blog
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.MapperConfiguration = BlogMapper.MapperConfiguration;

            adminBuilder.Register<DTO.Blog, IBlogService>(modelBuilder =>
            {
                modelBuilder.GridBuilder
                    .AddField(p => p.Name);

                modelBuilder.FormBuilder
                    .AddKeyField(p => p.Id)
                    .AddField(p => p.Name);
            });

            adminBuilder.Register<DTO.Post, IPostService>(modelBuilder =>
            {
                modelBuilder.GridBuilder.Title = "Posts";
                modelBuilder.FormBuilder.Title = "Post";

                modelBuilder.GridBuilder
                    .AddField(p => p.Blog)
                    .AddField(p => p.Title)
                    .AddField(p => p.Category)
                    .AddField(p => p.Tags)
                    .AddField(p => p.CreatedOn, option => option.Format = "dd.MM.yyyy")
                    .AddField(p => p.CreatedBy, option => option.DisplayName = "Author");

                modelBuilder.FormBuilder
                    .AddKeyField(p => p.Id)
                    .AddSelectField(p => p.Blog)
                    .AddField(p => p.Title)
                    .AddField(p => p.Slug)
                    .AddField(s => s.Summary)
                    .AddField(s => s.Thumbnail)
                    .AddField(s => s.Content)
                    .AddSelectField(s => s.Category, expr => expr.Name)
                    .AddInlineMultiSelectField<DTO.Tag>(s => s.Tags, expr => expr.Name)
                    .AddField(p => p.CreatedOn, option => option.Format = "dd.MM.yyyy");


                modelBuilder.FormBuilder.SetCustomValidationFor(c => c.Slug,
                    (sp, slug) =>
                        sp.GetService<IPostService>().ValidateSlug(slug));

                modelBuilder.FormBuilder.Property(c => c.Slug).AutoFillBasedOn(p => p.Title,
                    (sp, title) =>
                        sp.GetService<IPostService>().GetSlugFor(title));

                modelBuilder.FormBuilder.Property(c => c.Slug)
                    .CalculateWith(c => new
                    {
                        c.Content,
                        c.Slug,
                        c.Title,
                    }, (post) => $"{post.Title};{post.Slug};{post.Content}");

                modelBuilder.FormBuilder
                .Property(p => p.Tags)
                    .AddItemBy(t => t.Name);

                modelBuilder.FormBuilder.Property(u => u.Blog).HasLookup(sp => sp.GetService<IBlogService>().GetBlogs(),
                    ke => ke.Id,
                    de => de.Name);

                modelBuilder.FormBuilder.Property(u => u.Category).HasLookup(sp => sp.GetService<ICategoryService>().GetCategories(),
                    ke => ke.Id,
                    de => de.Name);

                modelBuilder.FormBuilder.Property(u => u.Tags).HasLookup(sp => sp.GetService<ITagService>().GetTags(),
                    ke => ke.Id,
                    de => de.Name);

                modelBuilder.FormBuilder.AddFormAction("Publish", "Publish",
                    (sp, post) => sp.GetService<IPostService>().PublishPost(post));


                modelBuilder.AddChildConfig(s => s.Comments, (childForm) =>
                  {
                      childForm.FormBuilder
                      .AddKeyField(c => c.Id)
                      .AddField(c => c.UserName)
                      .AddField(c => c.Comment)
                      .AddField(c => c.CreatedOn)
                      .AddField(c => c.IsApproved);
                  });
            });

            adminBuilder.Register<DTO.Category, ICategoryService>(modelBuilder =>
            {
                modelBuilder.GridBuilder
                    .AddField(p => p.Name);

                modelBuilder.FormBuilder
                    .AddKeyField(p => p.Id)
                    .AddField(p => p.Name);
            });

            adminBuilder.Register<DTO.Tag, ITagService>(modelBuilder =>
            {
                modelBuilder.GridBuilder
                    .AddField(p => p.Name);

                modelBuilder.FormBuilder
                    .AddKeyField(p => p.Id)
                    .AddField(p => p.Name);
            });
        }
    }
}
