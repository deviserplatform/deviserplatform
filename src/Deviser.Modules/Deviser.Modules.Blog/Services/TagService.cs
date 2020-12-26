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
using Deviser.Detached;
using Deviser.Modules.Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tag = Deviser.Modules.Blog.DTO.Tag;

namespace Deviser.Modules.Blog.Services
{
    public class TagService : ITagService
    {
        private readonly AdminModelService<Tag, Models.Tag> _adminModelService;
        private readonly IMapper _blogMapper;
        private readonly BlogDbContext _dbContext;

        public TagService(BlogDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _blogMapper = BlogMapper.Mapper;
            _dbContext = dbContext;
            _adminModelService = new AdminModelService<Tag, Models.Tag>(_dbContext, dbContext.Tags,_blogMapper);
        }

        public async Task<PagedResult<Tag>> GetAll(int pageNo, int pageSize, string orderByProperties,
            FilterNode filter = null) => await _adminModelService.GetAll(pageNo, pageSize, orderByProperties, filter);

        public async Task<Tag> GetItem(string itemId) => await _adminModelService.GetItem(itemId);

        public async Task<IFormResult<Tag>> CreateItem(Tag item) => await _adminModelService.CreateItem(item);

        public async Task<IFormResult<Tag>> UpdateItem(Tag item) => await _adminModelService.UpdateItem(item);

        public async Task<IAdminResult<Tag>> DeleteItem(string itemId) => await _adminModelService.DeleteItem(itemId);

        public ICollection<Tag> GetTags()
        {
            var dbTags = _dbContext.Tags.ToList();
            return _blogMapper.Map<ICollection<Tag>>(dbTags);
        }
    }
}
