using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Services;
using Deviser.Modules.Blog.Models;
using Microsoft.AspNetCore.Http;

namespace Deviser.Modules.Blog.Services
{
    public class BlogService : IAdminService<DTO.Blog>
    {
        private readonly AdminService<DTO.Blog, Models.Blog> _adminService;
        private readonly IMapper _blogMapper;
        private readonly BlogDbContext _dbContext;

        public BlogService(BlogDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _blogMapper = BlogMapper.Mapper;
            _dbContext = dbContext;
            _adminService = new AdminService<DTO.Blog, Models.Blog>(_dbContext, dbContext.Blogs, _blogMapper);
        }

        public async Task<PagedResult<DTO.Blog>> GetAll(int pageNo, int pageSize, string orderByProperties,
            FilterNode filter = null) => await _adminService.GetAll(pageNo, pageSize, orderByProperties, filter);

        public async Task<DTO.Blog> GetItem(string itemId) => await _adminService.GetItem(itemId);

        public async Task<IFormResult<DTO.Blog>> CreateItem(DTO.Blog item) => await _adminService.CreateItem(item);

        public async Task<IFormResult<DTO.Blog>> UpdateItem(DTO.Blog item) => await _adminService.UpdateItem(item);

        public async Task<IAdminResult<DTO.Blog>> DeleteItem(string itemId) => await _adminService.DeleteItem(itemId);

        public ICollection<DTO.Blog> GetBlogs()
        {
            var blogs = _dbContext.Blogs.ToList();
            return _blogMapper.Map<ICollection<DTO.Blog>>(blogs);
        }

    }
}
