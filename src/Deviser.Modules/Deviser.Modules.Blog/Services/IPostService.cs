using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Services;
using Deviser.Modules.Blog.DTO;

namespace Deviser.Modules.Blog.Services
{
    public interface IPostService : IAdminService<Post>
    {
        Task<ValidationResult> ValidateSlug(string slug);
        Task<string> GetSlugFor(string title);
        Task<PagedResult<Post>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null);
        Task<Post> GetItem(string itemId);
        Task<IFormResult<Post>> CreateItem(Post item);
        Task<IFormResult<Post>> UpdateItem(Post item);
        Task<IAdminResult<Post>> DeleteItem(string itemId);
        Task<Dictionary<Guid, Core.Common.DomainTypes.User>> GetAllUsers();
        Task<IFormResult<Post>> PublishPost(Post post);
    }
}