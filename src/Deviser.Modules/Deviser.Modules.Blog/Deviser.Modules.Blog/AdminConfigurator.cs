using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Deviser.Admin;
using Deviser.Admin.Extensions;
using Deviser.Modules.Blog.Models;
using Deviser.Modules.Blog.Services;
using Microsoft.Extensions.DependencyInjection;

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
            adminBuilder.MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, DTO.Post>()
                .ForMember(dest => dest.Tags, opt =>
                {
                    opt.Condition(src => src.PostTags != null && src.PostTags.Count > 0);
                    opt.MapFrom(src => src.PostTags.Select(pt => pt.Tag));
                }).ReverseMap()
                .ForMember(dest => dest.PostTags, opt =>
                {
                    opt.Condition(src => src.Tags != null && src.Tags.Count > 0);
                    opt.MapFrom(src => src.Tags.Select(t => new PostTag { TagId = t.Id, PostId = src.Id }));
                });
                cfg.CreateMap<Tag, DTO.Tag>().ReverseMap();
                cfg.CreateMap<Comments, DTO.Comments>().ReverseMap();
                cfg.CreateMap<Category, DTO.Category>().ReverseMap();
            });


            //adminBuilder.Register<PostTag>(form =>
            //{

            //    form.FormBuilder
            //         .AddSelectField(s => s.Post, expr => expr.Title);
            //});

            adminBuilder.Register<DTO.Post>(modelBuilder =>
            {
                modelBuilder.GridBuilder.Title = "Posts";
                modelBuilder.FormBuilder.Title = "Post";

                modelBuilder.GridBuilder
                    .AddField(p => p.Title)
                    
                    .AddField(p => p.Category)
                    .AddField(p => p.Tags)
                    .AddField(p => p.CreatedOn, option => option.Format = "dd.MM.yyyy")
                    .AddField(p => p.CreatedBy);

                modelBuilder.FormBuilder
                .AddKeyField(p => p.Id)
                .AddField(p => p.Title)
                .AddField(p => p.Slug)
                .AddField(s => s.Content)
                .AddSelectField(s => s.Category, expr => expr.Name)
                .AddInlineMultiSelectField<DTO.Tag>(s => s.Tags, expr => expr.TagName)
                .AddField(p => p.CreatedBy, option => option.DisplayName="Author");


                modelBuilder.FormBuilder.SetCustomValidationFor(c => c.Slug,
                    (sp, slug) =>
                        sp.GetService<BlogService>().ValidateSlug(slug));

                modelBuilder.FormBuilder.Property(c => c.Slug).AutoFillBasedOn(p => p.Title,
                    (sp, title) =>
                        sp.GetService<BlogService>().GetSlugFor(title));

                modelBuilder.FormBuilder
                    .Property(p => p.Tags)
                    .AddItemBy(t => t.TagName);
                    

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

            adminBuilder.Register<DTO.Category>(modelBuilder =>
            {

                modelBuilder.GridBuilder
                    .AddField(p => p.Name);

                modelBuilder.FormBuilder
                    .AddKeyField(p => p.Id)
                    .AddField(p => p.Name);
            });

            adminBuilder.Register<DTO.Tag>(modelBuilder =>
            {

                modelBuilder.GridBuilder
                    .AddField(p => p.TagName);

                modelBuilder.FormBuilder
                    .AddKeyField(p => p.Id)
                    .AddField(p => p.TagName);
            });
        }
    }
}
