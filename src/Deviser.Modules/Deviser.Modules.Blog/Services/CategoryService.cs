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
using Category = Deviser.Modules.Blog.DTO.Category;
using Tag = Deviser.Modules.Blog.DTO.Tag;

namespace Deviser.Modules.Blog.Services
{
    public class CategoryService : IAdminService<Category>
    {
        private readonly AdminService<Category, Models.Category> _adminService;
        private readonly IMapper _blogMapper;
        private readonly BlogDbContext _dbContext;

        public CategoryService(BlogDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _blogMapper = BlogMapper.Mapper;
            _dbContext = dbContext;
            _adminService = new AdminService<Category, Models.Category>(_dbContext, dbContext.Categories, _blogMapper);
        }

        public async Task<PagedResult<Category>> GetAll(int pageNo, int pageSize, string orderByProperties,
            FilterNode filter = null) => await _adminService.GetAll(pageNo, pageSize, orderByProperties, filter);
        
        public async Task<Category> GetItem(string itemId) => await _adminService.GetItem(itemId);

        public async Task<IFormResult<Category>> CreateItem(Category item) => await _adminService.CreateItem(item);

        public async Task<IFormResult<Category>> UpdateItem(Category item) => await _adminService.UpdateItem(item);

        public async Task<IAdminResult<Category>> DeleteItem(string itemId) => await _adminService.DeleteItem(itemId);

        public ICollection<Category> GetCategories()
        {
            var dbCategories = _dbContext.Categories.ToList();
            return _blogMapper.Map<ICollection<Category>>(dbCategories);
        }

    }
}
