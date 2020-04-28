using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Extensions;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Services;
using Microsoft.Extensions.Logging;

namespace Deviser.Modules.Language.Services
{
    public class LanguageAdminService : IAdminService<Core.Common.DomainTypes.Language> //: IAdminFormService<RecycleItem>
    {
        private readonly ILogger<LanguageAdminService> _logger;
        private readonly ILanguageManager _languageManager;
        private readonly IScopeService _scopeService;

        public LanguageAdminService(ILogger<LanguageAdminService> logger,
            ILanguageManager languageManager,
            IScopeService scopeService)
        {
            _logger = logger;
            _languageManager = languageManager;
            _scopeService = scopeService;
        }

        //public async Task<PagedResult<RecycleItem>> GetAll(int pageNo, int pageSize, string orderByProperties)
        //{
        //    var recycleItems = GetRecycleItems();

        //    var skip = (pageNo - 1) * pageSize;
        //    var total = recycleItems.Count;
        //    var result = recycleItems.Skip(skip).Take(pageSize);
        //    var pagedResult = new PagedResult<RecycleItem>(result, pageNo, pageSize, total);

        //    return await Task.FromResult(pagedResult);
        //}

        //private List<RecycleItem> GetRecycleItems()
        //{
        //    var pages = _pageRepository.GetDeletedPages();
        //    var layouts = _layoutRepository.GetDeletedLayouts();
        //    var pageContents = _pageContentRepository.GetDeletedPageContents();
        //    var pageModules = _pageRepository.GetDeletedPageModules();

        //    var recycleItems = new List<RecycleItem>();
        //    var allRecycleItemTypes = RecycleItemType.GetRecycleItemTypes();

        //    if (layouts != null && layouts.Count > 0)
        //    {
        //        recycleItems.AddRange(layouts.Select(layout => new RecycleItem()
        //        {
        //            Id = layout.Id,
        //            Name = layout.Name,
        //            RecycleItemType = allRecycleItemTypes.First(rt => rt.Name == "Layouts")
        //        }));
        //    }

        //    if (pages != null && pages.Count > 0)
        //    {
        //        recycleItems.AddRange(pages.Select(page => new RecycleItem()
        //        {
        //            Id = page.Id,
        //            Name = page.PageTranslation.FirstOrDefault()?.Name,
        //            RecycleItemType = allRecycleItemTypes.First(rt => rt.Name == "Page")
        //        }));
        //    }

        //    if (pageContents != null && pageContents.Count > 0)
        //    {
        //        recycleItems.AddRange(pageContents.Select(pc => new RecycleItem()
        //        {
        //            Id = pc.Id,
        //            Name = pc.Title,
        //            RecycleItemType = allRecycleItemTypes.First(rt => rt.Name == "PageContent")
        //        }));
        //    }

        //    if (pageModules != null && pageModules.Count > 0)
        //    {
        //        recycleItems.AddRange(pageModules.Select(pm => new RecycleItem()
        //        { Id = pm.Id, Name = pm.Title, RecycleItemType = allRecycleItemTypes.First(rt => rt.Name == "PageModule") }));
        //    }

        //    return recycleItems;
        //}

        //public async Task<IAdminResult<RecycleItem>> DeleteItem(string itemId)
        //{
        //    var id = Guid.Parse(itemId);
        //    var recycleItems = GetRecycleItems();
        //    var itemToBeDeleted = recycleItems.FirstOrDefault(ri => ri.Id == id);
        //    var isDeleted = false;
        //    if (itemToBeDeleted == null)
        //    {
        //        return null;
        //    }

        //    isDeleted = itemToBeDeleted.RecycleItemType.Name switch
        //    {
        //        "Layouts" => _layoutRepository.DeleteLayout(itemToBeDeleted.Id),
        //        "Page" => _pageRepository.DeletePage(itemToBeDeleted.Id),
        //        "PageContent" => _pageContentRepository.DeletePageContent(itemToBeDeleted.Id),
        //        "PageModule" => _pageRepository.DeletePageModule(itemToBeDeleted.Id),
        //        _ => isDeleted
        //    };

        //    //_pageRepository.DeletePage();
        //    if (isDeleted)
        //    {
        //        return await Task.FromResult(new AdminResult<RecycleItem>(itemToBeDeleted)
        //        {
        //            IsSucceeded = true,
        //            SuccessMessage = $"{itemToBeDeleted.RecycleItemType.Name} has been removed"
        //        });
        //    }

        //    return await Task.FromResult(new AdminResult<RecycleItem>(itemToBeDeleted)
        //    {
        //        IsSucceeded = false,
        //        SuccessMessage = $"Unable to delete {itemToBeDeleted.RecycleItemType.Name}"
        //    });
        //}

        //public async Task<IAdminResult> Restore(RecycleItem item)
        //{
        //    if (item == null)
        //    {
        //        return null;
        //    }

        //    object resultItem = null;

        //    switch (item.RecycleItemType.Name)
        //    {
        //        case "Layouts":
        //            var layout = _layoutRepository.GetLayout(item.Id);
        //            layout.IsDeleted = false;
        //            resultItem = _layoutRepository.UpdateLayout(layout);
        //            break;
        //        case "Page":
        //            var page = _pageRepository.GetPage(item.Id);
        //            page.IsDeleted = false;
        //            resultItem = _pageRepository.UpdatePage(page);
        //            break;
        //        case "PageContent":
        //            var pageContent = _pageContentRepository.Get(item.Id);
        //            pageContent.IsDeleted = false;
        //            resultItem = _pageContentRepository.Update(pageContent);
        //            break;
        //        case "PageModule":
        //            var pageModule = _pageRepository.GetPageModule(item.Id);
        //            pageModule.IsDeleted = false;
        //            resultItem = _pageRepository.UpdatePageModule(pageModule);
        //            break;
        //    };

        //    //_pageRepository.DeletePage();
        //    if (resultItem != null)
        //    {
        //        return await Task.FromResult(new AdminResult(resultItem)
        //        {
        //            IsSucceeded = true,
        //            SuccessMessage = $"{item.RecycleItemType.Name} has been restored"
        //        });
        //    }

        //    return await Task.FromResult(new AdminResult(resultItem)
        //    {
        //        IsSucceeded = false,
        //        SuccessMessage = $"Unable to restore {item.RecycleItemType.Name}"
        //    });
        //}

        public async Task<PagedResult<Core.Common.DomainTypes.Language>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            var languages = _languageManager.GetLanguages();
            if (filter != null)
            {
                languages = languages.ApplyFilter(filter).ToList();
            }
            var pagedResult = new PagedResult<Core.Common.DomainTypes.Language>(languages, pageNo, pageSize);

            return await Task.FromResult(pagedResult);
        }


        public async Task<Core.Common.DomainTypes.Language> GetItem(string itemId)
        {
            var languages = _languageManager.GetAllLanguages();
            var language = languages.FirstOrDefault(l => string.Equals(l.CultureCode, itemId));
            return await Task.FromResult(language);
        }

        public async Task<IFormResult<Core.Common.DomainTypes.Language>> CreateItem(Core.Common.DomainTypes.Language item)
        {
            var languageResult = _languageManager.CreateLanguage(item);
            if (languageResult != null)
            {
                var result = new FormResult<Core.Common.DomainTypes.Language>(languageResult)
                {
                    IsSucceeded = true,
                    SuccessMessage = "Language has been added successfully"
                };
                return await Task.FromResult(result);
            }

            return new FormResult<Core.Common.DomainTypes.Language>()
            {
                IsSucceeded = false,
                ErrorMessage = "Unable to add the language"
            };

        }

        public async Task<IFormResult<Core.Common.DomainTypes.Language>> UpdateItem(Core.Common.DomainTypes.Language item)
        {
            var languageResult = _languageManager.UpdateLanguage(item);
            if (languageResult != null)
            {
                // Make sure the required properties in _scopeService.PageContext are initialized in PageContext Middleware
                var isMultilingual = _languageManager.GetActiveLanguages().Count > 1;
                var currentUrl = _scopeService.PageContext.CurrentUrl;
                var currentLocale = _scopeService.PageContext.CurrentLocale;
                if (isMultilingual && !currentUrl.ToLower().StartsWith(currentLocale.ToLower()))
                {
                    currentUrl = $"{currentLocale.ToLower()}/{currentUrl}";
                }
                else if (!isMultilingual && currentUrl.ToLower().StartsWith(currentLocale.ToLower()))
                {
                    currentUrl = currentUrl.Replace($"{currentLocale.ToLower()}/","");
                }
                var result = new FormResult<Core.Common.DomainTypes.Language>(languageResult)
                {
                    IsSucceeded = true,
                    SuccessMessage = "Language has been updated successfully",
                    SuccessAction = new OpenUrlAction()
                    {
                        OpenAfterSec = 5,
                        Url = currentUrl
                    }

                };
                return await Task.FromResult(result);
            }

            return new FormResult<Core.Common.DomainTypes.Language>()
            {
                IsSucceeded = false,
                ErrorMessage = "Unable to update the language"
            };
        }

        public async Task<IAdminResult<Core.Common.DomainTypes.Language>> DeleteItem(string itemId)
        {
            var languages = _languageManager.GetLanguages();
            var language = languages.FirstOrDefault(l => string.Equals(l.CultureCode, itemId));
            language.IsActive = false;
            var languageResult = _languageManager.UpdateLanguage(language);
            if (languageResult != null)
            {
                var result = new FormResult<Core.Common.DomainTypes.Language>(languageResult)
                {
                    IsSucceeded = true,
                    SuccessMessage = "Language has been updated successfully"
                };
                return await Task.FromResult(result);
            }

            return new FormResult<Core.Common.DomainTypes.Language>()
            {
                IsSucceeded = false,
                ErrorMessage = "Unable to update the language"
            };
        }

        public IList<Core.Common.DomainTypes.Language> GetAllLanguages()
        {
            return _languageManager.GetAllLanguages(true);
        }
    }
}
