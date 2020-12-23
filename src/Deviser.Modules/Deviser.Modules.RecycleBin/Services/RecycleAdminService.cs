using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Extensions;
using Deviser.Admin.Services;
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
        private readonly ILogger<RecycleAdminService> _logger;
        private readonly ILayoutRepository _layoutRepository;
        private readonly IPageContentRepository _pageContentRepository;
        private readonly IPageRepository _pageRepository;

        public RecycleAdminService(ILogger<RecycleAdminService> logger,
            ILayoutRepository layoutRepository,
            IPageContentRepository pageContentRepository,
            IPageRepository pageRepository)
        {
            _logger = logger;
            _layoutRepository = layoutRepository;
            _pageContentRepository = pageContentRepository;
            _pageRepository = pageRepository;
        }

        public async Task<PagedResult<RecycleItem>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            var recycleItems = GetRecycleItems();

            if (filter != null)
            {
                recycleItems = recycleItems.ApplyFilter(filter).ToList();
            }
            var pagedResult = new PagedResult<RecycleItem>(recycleItems, pageNo, pageSize, orderByProperties);

            return await Task.FromResult(pagedResult);
        }

        private List<RecycleItem> GetRecycleItems()
        {
            var pages = _pageRepository.GetDeletedPages();
            var layouts = _layoutRepository.GetDeletedLayouts();
            var pageContents = _pageContentRepository.GetDeletedPageContents();
            var pageModules = _pageRepository.GetDeletedPageModules();

            var recycleItems = new List<RecycleItem>();
            var allRecycleItemTypes = RecycleItemType.GetRecycleItemTypes();

            if (layouts != null && layouts.Count > 0)
            {
                recycleItems.AddRange(layouts.Select(layout => new RecycleItem()
                {
                    Id = layout.Id,
                    Name = layout.Name,
                    RecycleItemType = allRecycleItemTypes.First(rt => rt.Name == "Layouts")
                }));
            }

            if (pages != null && pages.Count > 0)
            {
                recycleItems.AddRange(pages.Select(page => new RecycleItem()
                {
                    Id = page.Id,
                    Name = page.PageTranslation.FirstOrDefault()?.Name,
                    RecycleItemType = allRecycleItemTypes.First(rt => rt.Name == "Page")
                }));
            }

            if (pageContents != null && pageContents.Count > 0)
            {
                recycleItems.AddRange(pageContents.Select(pc => new RecycleItem()
                {
                    Id = pc.Id,
                    Name = pc.Title,
                    RecycleItemType = allRecycleItemTypes.First(rt => rt.Name == "PageContent")
                }));
            }

            if (pageModules != null && pageModules.Count > 0)
            {
                recycleItems.AddRange(pageModules.Select(pm => new RecycleItem()
                { Id = pm.Id, Name = pm.Title, RecycleItemType = allRecycleItemTypes.First(rt => rt.Name == "PageModule") }));
            }

            return recycleItems;
        }

        public async Task<IAdminResult<RecycleItem>> DeleteItem(string itemId)
        {
            var id = Guid.Parse(itemId);
            var recycleItems = GetRecycleItems();
            var itemToBeDeleted = recycleItems.FirstOrDefault(ri => ri.Id == id);
            var isDeleted = false;
            if (itemToBeDeleted == null)
            {
                return null;
            }

            isDeleted = itemToBeDeleted.RecycleItemType.Name switch
            {
                "Layouts" => _layoutRepository.DeleteLayout(itemToBeDeleted.Id),
                "Page" => _pageRepository.DeletePage(itemToBeDeleted.Id),
                "PageContent" => _pageContentRepository.DeletePageContent(itemToBeDeleted.Id),
                "PageModule" => _pageRepository.DeletePageModule(itemToBeDeleted.Id),
                _ => isDeleted
            };

            //_pageRepository.DeletePage();
            if (isDeleted)
            {
                return await Task.FromResult(new AdminResult<RecycleItem>(itemToBeDeleted)
                {
                    IsSucceeded = true,
                    SuccessMessage = $"{itemToBeDeleted.RecycleItemType.Name} has been deleted"
                });
            }

            return await Task.FromResult(new AdminResult<RecycleItem>(itemToBeDeleted)
            {
                IsSucceeded = false,
                SuccessMessage = $"Unable to delete {itemToBeDeleted.RecycleItemType.Name}"
            });
        }

        public async Task<IAdminResult> Restore(RecycleItem item)
        {
            if (item == null)
            {
                return null;
            }

            object resultItem = null;

            switch (item.RecycleItemType.Name)
            {
                case "Layouts":
                    var layout = _layoutRepository.GetLayout(item.Id);
                    layout.IsActive = true;
                    resultItem = _layoutRepository.UpdateLayout(layout);
                    break;
                case "Page":
                    var page = _pageRepository.GetPage(item.Id);
                    page.IsActive = true;
                    resultItem = _pageRepository.UpdatePageActiveAndLayout(page);
                    break;
                case "PageContent":
                    var pageContent = _pageContentRepository.Get(item.Id);
                    pageContent.IsActive = true;
                    resultItem = _pageContentRepository.Update(pageContent);
                    break;
                case "PageModule":
                    var pageModule = _pageRepository.GetPageModule(item.Id);
                    pageModule.IsActive = true;
                    resultItem = _pageRepository.UpdatePageModule(pageModule);
                    break;
            };

            //_pageRepository.DeletePage();
            if (resultItem != null)
            {
                return await Task.FromResult(new AdminResult(resultItem)
                {
                    IsSucceeded = true,
                    SuccessMessage = $"{item.RecycleItemType.Name} has been restored"
                });
            }

            return await Task.FromResult(new AdminResult(resultItem)
            {
                IsSucceeded = false,
                SuccessMessage = $"Unable to restore {item.RecycleItemType.Name}"
            });
        }
    }
}
