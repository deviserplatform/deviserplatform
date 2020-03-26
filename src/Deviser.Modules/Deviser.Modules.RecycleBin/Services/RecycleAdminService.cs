using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Sites;
using Microsoft.Extensions.Logging;

namespace Deviser.Modules.RecycleBin.Services
{
    public class RecycleAdminService : IAdminGridService<RecycleItem> //: IAdminFormService<RecycleItem>
    {
        private readonly IContentManager _contentManager;
        private readonly ILogger<RecycleAdminService> _logger;
        private readonly ILayoutManager _layoutManager;
        private readonly IModuleManager _moduleManager;
        private readonly IPageRepository _pageRepository;

        public RecycleAdminService(IContentManager contentManager,
            ILogger<RecycleAdminService> logger,
            ILayoutManager layoutManager,
            IModuleManager moduleManager,
            IPageRepository pageRepository)
        {
            _contentManager = contentManager;
            _logger = logger;
            _layoutManager = layoutManager;
            _moduleManager = moduleManager;
            _pageRepository = pageRepository;
        }

        public async Task<PagedResult<RecycleItem>> GetAll(int pageNo, int pageSize, string orderByProperties)
        {
            var pages = _pageRepository.GetDeletedPages();
            var layouts = _layoutManager.GetDeletedLayouts();
            var pageContents = _contentManager.GetDeletedPageContents();
            var pageModules = _moduleManager.GetDeletedPageModules();

            var recycleItems = new List<RecycleItem>();

            if (layouts != null && layouts.Count > 0)
            {
                recycleItems.AddRange(layouts.Select(layout => new RecycleItem() { Id = layout.Id, Name = layout.Name }));
            }

            if (pages != null && pages.Count > 0)
            {
                recycleItems.AddRange(pages.Select(page => new RecycleItem() { Id = page.Id, Name = page.PageTranslation.FirstOrDefault()?.Name }));
            }

            if (pageContents != null && pageContents.Count > 0)
            {
                recycleItems.AddRange(pageContents.Select(pc => new RecycleItem() { Id = pc.Id, Name = pc.Title }));
            }

            if (pageModules != null && pageModules.Count > 0)
            {
                recycleItems.AddRange(pageModules.Select(pm => new RecycleItem() { Id = pm.Id, Name = pm.Title }));
            }

            var skip = (pageNo - 1) * pageSize;
            var total = recycleItems.Count;
            var result = recycleItems.Skip(skip).Take(pageSize);
            var pagedResult = new PagedResult<RecycleItem>(result, pageNo, pageSize, total);

            return await Task.FromResult(pagedResult);
        }

        public async Task<RecycleItem> DeleteItem(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<FormResult> Restore(RecycleItem item)
        {
            throw new NotImplementedException();
        }
    }
}
