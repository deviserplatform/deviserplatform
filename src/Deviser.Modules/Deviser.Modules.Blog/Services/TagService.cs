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
    public class TagService : IAdminService<Tag>
    {
        private readonly AdminService<Tag, Models.Tag> _adminService;
        private readonly IMapper _blogMapper;
        private readonly BlogDbContext _dbContext;

        public TagService(BlogDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _blogMapper = BlogMapper.Mapper;
            _dbContext = dbContext;
            _adminService = new AdminService<Tag, Models.Tag>(_dbContext, dbContext.Tags,_blogMapper);
        }

        //public async Task<PagedResult<Tag>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        //{
        //    var dbTags = _dbContext.Tags.ToList();
        //    var tags = _blogMapper.Map<ICollection<Tag>>(dbTags);
        //    var pagedResult = new PagedResult<Tag>(tags, pageNo, pageSize, orderByProperties);
        //    return await Task.FromResult(pagedResult);
        //}

        //public async Task<Tag> GetItem(string itemId)
        //{
        //    var dbTag = await _dbContext.Tags.Where(t => t.Id == Guid.Parse(itemId)).FirstOrDefaultAsync();
        //    var result = _blogMapper.Map<Tag>(dbTag);
        //    return await Task.FromResult(result);
        //}

        //public async Task<IFormResult<Tag>> CreateItem(Tag item)
        //{
        //    var dbTag = _blogMapper.Map<Models.Tag>(item);
        //    var tag = _dbContext.Tags.Add(dbTag);
        //    await _dbContext.SaveChangesAsync();

        //    if (tag == null)
        //        return new FormResult<Tag>()
        //        {
        //            IsSucceeded = false,
        //            ErrorMessage = "Unable to create the Tag"
        //        };

        //    var result = new FormResult<Tag>(_blogMapper.Map<Tag>(tag))
        //    {
        //        IsSucceeded = true,
        //        SuccessMessage = "Tag has been created"
        //    };

        //    return await Task.FromResult(result);
        //}

        //public async Task<IFormResult<Tag>> UpdateItem(Tag item)
        //{
        //    var dbTag = _blogMapper.Map<Models.Tag>(item);
        //    var queryableData = _dbContext.UpdateGraph(dbTag);
        //    await _dbContext.SaveChangesAsync();

        //    if (queryableData == null)
        //        return new FormResult<Tag>()
        //        {
        //            IsSucceeded = false,
        //            ErrorMessage = "Unable to update the Tag"
        //        };

        //    var tag = await GetItem(queryableData.Id.ToString());
        //    var result = new FormResult<Tag>(tag)
        //    {
        //        IsSucceeded = true,
        //        SuccessMessage = "Tag has been updated"
        //    };
        //    return await Task.FromResult(result);
        //}

        //public async Task<IAdminResult<Tag>> DeleteItem(string itemId)
        //{
        //    var dgTag = await _dbContext.Tags.FirstOrDefaultAsync(p => p.Id == Guid.Parse(itemId));
        //    _dbContext.Tags.Remove(dgTag);
        //    await _dbContext.SaveChangesAsync();
        //    var post = _blogMapper.Map<Tag>(dgTag);
        //    var result = new FormResult<Tag>(post)
        //    {
        //        IsSucceeded = true,
        //        SuccessMessage = "Tag has been removed"
        //    };
        //    return await Task.FromResult(result);
        //}

        public async Task<PagedResult<Tag>> GetAll(int pageNo, int pageSize, string orderByProperties,
            FilterNode filter = null) => await _adminService.GetAll(pageNo, pageSize, orderByProperties, filter);

        public async Task<Tag> GetItem(string itemId) => await _adminService.GetItem(itemId);

        public async Task<IFormResult<Tag>> CreateItem(Tag item) => await _adminService.CreateItem(item);

        public async Task<IFormResult<Tag>> UpdateItem(Tag item) => await _adminService.UpdateItem(item);

        public async Task<IAdminResult<Tag>> DeleteItem(string itemId) => await _adminService.DeleteItem(itemId);

        public ICollection<Tag> GetTags()
        {
            var dbTags = _dbContext.Tags.ToList();
            return _blogMapper.Map<ICollection<Tag>>(dbTags);
        }
    }
}
