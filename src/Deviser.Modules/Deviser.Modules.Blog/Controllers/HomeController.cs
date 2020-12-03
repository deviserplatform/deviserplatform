using System;
using System.Linq;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Deviser.Modules.Blog.Models;
using Microsoft.AspNetCore.Mvc;


namespace Deviser.Modules.Blog
{
    [Module("Blog")]
    public class HomeController : ModuleController
    {
        private BlogDbContext _dbContext;
        public HomeController(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var posts = _dbContext.Posts.ToList();
            return View(posts);
        }

        public IActionResult Post(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return NotFound();

            var post = _dbContext.Posts.ToList().FirstOrDefault(p =>
                string.Equals(slug, p.Slug, StringComparison.InvariantCultureIgnoreCase));

            if(post==null)
                return NotFound();

            return View(post);
        }
    }
}
