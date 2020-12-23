using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Deviser.Modules.Blog.Models;
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
        private readonly IMapper _mapper;
        public HomeController(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = BlogMapper.Mapper;
        }

        //public async Task<IActionResult> Index()
        //{
        //    return await Post();
        //}

        //[Route("modules/[area]/[controller]/Post/Category/{categoryName?}")]
        //[Route("modules/[area]/[controller]/Post/Tag/{tagName?}")]
        //public async Task<IActionResult> Post(string categoryName = null, string tagName = null)
        //{
        //    var dbPosts = await _dbContext.Posts
        //        .Include(p => p.Category)
        //        .Include(p => p.PostTags)
        //        .ToListAsync();
        //    var dbCategories = await _dbContext.Categories.Include(c => c.Posts).ToListAsync();
        //    var dbTags = await _dbContext.Tags.Include(t => t.PostTags).ToListAsync();

        //    if (!string.IsNullOrEmpty(categoryName))
        //    {
        //        dbPosts = dbPosts.Where(p =>
        //            string.Equals(p.Category.Name, categoryName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        //    }

        //    if (!string.IsNullOrEmpty(tagName))
        //    {
        //        dbPosts = dbPosts.Where(p =>
        //            p.Tags.Any(t => string.Equals(t.Name, tagName, StringComparison.InvariantCultureIgnoreCase))).ToList();
        //    }

        //    var posts = _mapper.Map<ICollection<Post>>(dbPosts);
        //    var categories = new List<Category>();
        //    var tags = new List<Tag>();

        //    foreach (var dbCategory in dbCategories)
        //    {
        //        var category = _mapper.Map<Category>(dbCategory);
        //        category.PostCount = dbCategory.Posts?.Count ?? 0;
        //        categories.Add(category);
        //    }

        //    foreach (var dbTag in dbTags)
        //    {
        //        var tag = _mapper.Map<Tag>(dbTag);
        //        tag.PostCount = dbTag.PostTags?.Count ?? 0;
        //        tags.Add(tag);
        //    }

        //    ViewBag.Categories = categories;
        //    ViewBag.Tags = tags;
        //    return View("Index", posts);
        //}

        //[Route("modules/[area]/[controller]/Post/{slug}")]
        //public IActionResult Post(string slug)
        //{
        //    if (string.IsNullOrEmpty(slug))
        //        return NotFound();

        //    var post = _dbContext.Posts.ToList().FirstOrDefault(p =>
        //        string.Equals(slug, p.Slug, StringComparison.InvariantCultureIgnoreCase));

        //    if (post == null)
        //        return NotFound();

        //    return View(post);
        //}
    }
}
