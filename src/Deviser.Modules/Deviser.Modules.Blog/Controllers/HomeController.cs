using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Deviser.Modules.Blog.Models;
using Deviser.Modules.Blog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Category = Deviser.Modules.Blog.DTO.Category;
using Post = Deviser.Modules.Blog.DTO.Post;
using Tag = Deviser.Modules.Blog.DTO.Tag;


namespace Deviser.Modules.Blog
{
    [Module("Blog")]
    public class HomeController : ModuleController
    {
        private readonly BlogDbContext _dbContext;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;


        public HomeController(BlogDbContext dbContext, IPostService postService)
        {
            _dbContext = dbContext;
            _mapper = BlogMapper.Mapper;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            return await Post();
        }

        [Route("modules/[area]/[controller]/{blogName}/Post/Category/{categoryName?}")]
        [Route("modules/[area]/[controller]/{blogName}/Post/Tag/{tagName?}")]
        public async Task<IActionResult> Post(string blogName = null, string categoryName = null, string tagName = null)
        {
            if (string.IsNullOrEmpty(blogName))
            {
                if (ModuleContext == null || ModuleContext.ModuleViewProperties == null) return NotFound();
                blogName = ModuleContext.ModuleViewProperties["BlogName"].Value;
            }


            //var dbAllPosts = await _dbContext.Posts
            //    .Include(p => p.Category)
            //    .Include(p => p.Tags)
            //    .Include(p => p.Blog)
            //    .ToListAsync();

            var allPosts = await _postService.GetAll(1, int.MaxValue, null);

            var posts = allPosts.Data.Where(p => string.Equals(p.Blog.Name, blogName, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (posts.Count == 0) return NotFound();

            var dbCategories = await _dbContext.Categories.Include(c => c.Posts).ToListAsync();
            var dbTags = await _dbContext.Tags.Include(t => t.Posts).ToListAsync();

            if (!string.IsNullOrEmpty(categoryName))
            {
                posts = posts.Where(p =>
                    string.Equals(p.Category.Name, categoryName, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(tagName))
            {
                posts = posts.Where(p =>
                    p.Tags.Any(t => string.Equals(t.Name, tagName, StringComparison.InvariantCultureIgnoreCase))).ToList();
            }

            var categories = new List<Category>();
            var tags = new List<Tag>();

            foreach (var dbCategory in dbCategories)
            {
                var category = _mapper.Map<Category>(dbCategory);
                category.PostCount = dbCategory.Posts?.Count ?? 0;
                if (category.PostCount == 0) continue;
                categories.Add(category);
            }

            foreach (var dbTag in dbTags)
            {
                var tag = _mapper.Map<Tag>(dbTag);
                tag.PostCount = dbTag.Posts?.Count ?? 0;
                if (tag.PostCount == 0) continue;
                tags.Add(tag);
            }

            ViewBag.Categories = categories;
            ViewBag.Tags = tags;
            ViewBag.ModuleContext = ModuleContext;
            ViewBag.BlogName = blogName;
            return await ViewAsync("Index", posts);
        }

        [Route("modules/[area]/[controller]/{blogName}/Post/{slug}")]
        public async Task<IActionResult> Post(string blogName, string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return NotFound();

            //var dbPosts = await _dbContext.Posts
            //    .Include(p => p.Category)
            //    .Include(p => p.Tags)
            //    .Include(p => p.Blog)
            //    .ToListAsync();

            var allPosts = await _postService.GetAll(1, int.MaxValue, null);

            var post = allPosts.Data.FirstOrDefault(p =>
            string.Equals(p.Blog.Name, blogName, StringComparison.InvariantCultureIgnoreCase) &&
            string.Equals(slug, p.Slug, StringComparison.InvariantCultureIgnoreCase));

            if (post == null)
                return NotFound();
            ViewBag.BlogName = blogName;
            return await ViewAsync(post);
        }
    }
}
