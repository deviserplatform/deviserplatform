using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Modules.Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Modules.Blog.Services
{
    public class BlogService
    {
        private readonly BlogDbContext _dbContext;

        public BlogService(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ValidationResult> ValidateSlug(string slug)
        {
            var posts = await _dbContext.Posts.ToListAsync();
            var result = posts.Any(p => p.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase)) ? ValidationResult.Failed(new ValidationError() { Code = "Slug not available!", Description = "This slug has already been used" }) : ValidationResult.Success;
            return await Task.FromResult(result);
        }

        public async Task<string> GetSlugFor(string title)
        {
            var slugCandidate = Regex.Replace(title, @"\s+", "");
            var posts = await _dbContext.Posts.ToListAsync();
            while (posts.Any(p => p.Slug.Equals(slugCandidate, StringComparison.InvariantCultureIgnoreCase)))
            {
                slugCandidate += "1";
            }

            return await Task.FromResult(slugCandidate);
        }
    }
}
