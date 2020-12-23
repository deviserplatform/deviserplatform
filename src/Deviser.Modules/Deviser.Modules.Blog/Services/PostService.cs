using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Services;
using Deviser.Core.Data.Entities;
using Deviser.Detached;
using Deviser.Modules.Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Post = Deviser.Modules.Blog.DTO.Post;
using Tag = Deviser.Modules.Blog.DTO.Tag;

namespace Deviser.Modules.Blog.Services
{
    public class PostService : IAdminService<Post>
    {
        private readonly AdminService<Post, Models.Post> _adminService;
        private readonly IMapper _blogMapper;
        private readonly BlogDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _platformMapper;
        private readonly UserManager<User> _userManager;
        private IQueryable<Models.Post> _postBaseQueryable;

        public PostService(BlogDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _blogMapper = BlogMapper.Mapper;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _platformMapper = mapper;
            _userManager = userManager;

            _postBaseQueryable = _dbContext.Posts
                .Include(p => p.Blog)
                .Include(p => p.Category)
                .Include(p => p.Tags);

            _adminService = new AdminService<Post, Models.Post>(_dbContext, _postBaseQueryable, _blogMapper);
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

        public async Task<PagedResult<Post>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            var result = await _adminService.GetAll(pageNo, pageSize, orderByProperties, filter);
            var allUsers = await GetAllUsers();
            foreach (var post in result.Data)
            {
                post.CreatedByUser = _platformMapper.Map<Core.Common.DomainTypes.User>(allUsers[post.CreatedBy]);
                if (post.ModifiedBy == Guid.Empty) continue;

                post.ModifiedByUser = _platformMapper.Map<Core.Common.DomainTypes.User>(allUsers[post.ModifiedBy]);
            }
            return await Task.FromResult(result);
        }

        public async Task<Post> GetItem(string itemId)
        {
            var result = await _adminService.GetItem(itemId);
            var allUsers = await GetAllUsers();
            result.CreatedByUser = _platformMapper.Map<Core.Common.DomainTypes.User>(allUsers[result.CreatedBy]);
            if (result.ModifiedBy != Guid.Empty)
            {
                result.ModifiedByUser = _platformMapper.Map<Core.Common.DomainTypes.User>(allUsers[result.ModifiedBy]);
            }
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Post>> CreateItem(Post item)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            item.Id = Guid.NewGuid();
            item.BlogId = item.Blog.Id;
            item.Blog = null;
            item.CategoryId = item.Category.Id;
            item.Category = null;
            item.CreatedBy = user.Id;
            item.CreatedOn = DateTime.Now;
            item.Status = PostStatus.Draft.ToString();
            
            //var dbTags = await _dbContext.Tags.ToDictionaryAsync(t => t.Id, t => t);

            //var tags = item.Tags as List<Tag>;

            //for (int i = 0; i < tags.Count; i++)
            //{   
            //    tags[i] = _blogMapper.Map<Tag>(dbTags[tags[i].Id]);
            //}

            var entity = _blogMapper.Map<Models.Post>(item);

            var dbTags = await _dbContext.Tags.ToDictionaryAsync(t => t.Id, t => t);
            var tags = entity.Tags as List<Models.Tag>;
            for (int i = 0; i < tags.Count; i++)
            {
                tags[i] = dbTags[tags[i].Id];
            }
            var queryableData = await _dbContext.Posts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            if (queryableData.Entity == null)
                return new FormResult<Post>()
                {
                    IsSucceeded = false,
                    ErrorMessage = $"Unable to create the Post"
                };

            var newPost = await GetItem(item.Id.ToString());

            var result = new FormResult<Post>(newPost)
            {
                IsSucceeded = true,
                SuccessMessage = $"Post has been created"
            };

            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Post>> UpdateItem(Post item)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            item.BlogId = item.Blog.Id;
            item.Blog = null;
            item.CategoryId = item.Category.Id;
            item.Category = null;
            item.CreatedBy = user.Id;
            item.CreatedOn = DateTime.Now;
            item.Status = PostStatus.Draft.ToString();

            var entity = _blogMapper.Map<Models.Post>(item);
            _dbContext.UpdateGraph(entity, mapping => mapping.AssociatedCollection(p=>p.Tags));
            await _dbContext.SaveChangesAsync();

            var post = await GetItem(item.Id.ToString());
            var result = new FormResult<Post>(post)
            {
                IsSucceeded = true,
                SuccessMessage = $"Post has been updated"
            };
            return await Task.FromResult(result);
        }

        public async Task<IAdminResult<Post>> DeleteItem(string itemId) => await _adminService.DeleteItem(itemId);

        private async Task<Dictionary<Guid, User>> GetAllUsers()
        {
            var allUsers = await _userManager.Users.ToDictionaryAsync(u => u.Id, u => u);
            return allUsers;
        }

    }
}
