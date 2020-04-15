using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace Deviser.Modules.PageManagement.Services
{
    public class PageManagementAdminService : IAdminTreeService<PageViewModel> //: IAdminFormService<RecycleItem>
    {
        private readonly ILogger<PageManagementAdminService> _logger;
        private readonly IPageRepository _pageRepository;
        private readonly IMapper _mapper;
        private readonly IModuleRepository _moduleRepository;
        private readonly INavigation _navigation;
        //private readonly IScopeService _scopeService;
        private readonly IThemeManager _themeManager;

        //private readonly IList<PageType> _pageTypes;
        private readonly IList<Module> _modules;
        private readonly IList<Theme> _themes;

        public PageManagementAdminService(ILogger<PageManagementAdminService> logger,
            IPageRepository pageRepository,
            IModuleRepository moduleRepository,
            INavigation navigation,
            IScopeService scopeService,
            IThemeManager themeManager)
        {
            _logger = logger;
            _pageRepository = pageRepository;
            _mapper = PageManagementMapper.Mapper;
            _moduleRepository = moduleRepository;
            _navigation = navigation;
            //_scopeService = scopeService;
            _themeManager = themeManager;

            //_pageTypes = GetPageTypes();
            _modules = GetModules();
            _themes = GetThemes();
        }

        public async Task<PageViewModel> GetTree()
        {
            var pageTree = _pageRepository.GetPageTree(true);
            return await Task.FromResult(ConvertToPageViewModel(pageTree));
        }

        public async Task<PageViewModel> GetItem(string itemId)
        {
            var result = _navigation.GetPageAndDependencies(Guid.Parse(itemId));
            return await Task.FromResult(ConvertToPageViewModel(result));
        }

        public async Task<IFormResult<PageViewModel>> CreateItem(PageViewModel item)
        {
            var pageResult = _navigation.CreatePage(ConvertToSingePage(item));
            if (pageResult != null)
            {
                var result = new FormResult<PageViewModel>(ConvertToPageViewModel(pageResult))
                {
                    IsSucceeded = true,
                    SuccessMessage = "Page has been created successfully"
                };
                return await Task.FromResult(result);
            }

            return new FormResult<PageViewModel>()
            {
                IsSucceeded = false,
                ErrorMessage = "Unable to create a page"
            };
        }

        public async Task<IFormResult<PageViewModel>> UpdateItem(PageViewModel item)
        {
            var pageResult = _navigation.UpdateSinglePage(ConvertToSingePage(item));
            if (pageResult != null)
            {
                var result = new FormResult<PageViewModel>(ConvertToPageViewModel(pageResult))
                {
                    IsSucceeded = true,
                    SuccessMessage = "Page has been updated successfully"
                };
                return await Task.FromResult(result);
            }

            return new FormResult<PageViewModel>()
            {
                IsSucceeded = false,
                ErrorMessage = "Unable to update page tree"
            };
        }

        public async Task<IFormResult<PageViewModel>> UpdateTree(PageViewModel item)
        {
            var pageResult = _navigation.UpdatePageTree(ConvertToPage(item));
            if (pageResult != null)
            {
                //UpdateSinglePage returns only the single node. However, the UI expects full tree
                var pageViewModel = await GetTree();
                var result = new FormResult<PageViewModel>(pageViewModel)
                {
                    IsSucceeded = true,
                    SuccessMessage = "Page tree has been updated successfully"
                };
                return await Task.FromResult(result);
            }

            return new FormResult<PageViewModel>()
            {
                IsSucceeded = false,
                ErrorMessage = "Unable to update page tree"
            };
        }

        public async Task<IAdminResult<PageViewModel>> DeleteItem(string itemId)
        {

            Page page = _pageRepository.GetPage(Guid.Parse(itemId));
            if (page != null)
            {
                page.IsDeleted = true;
                var pageResult = _pageRepository.UpdatePage(page);
                if (pageResult != null)
                {
                    //UpdateSinglePage returns only the single node. However, the UI expects full tree
                    var pageViewModel = await GetTree();
                    var result = new FormResult<PageViewModel>(pageViewModel)
                    {
                        IsSucceeded = true,
                        SuccessMessage = "Page has been deleted successfully"
                    };
                    return await Task.FromResult(result);
                }
            }

            return new FormResult<PageViewModel>()
            {
                IsSucceeded = false,
                ErrorMessage = "Unable to delete page"
            };
        }

        public IList<PageType> GetPageTypes()
        {
            var result = _pageRepository.GetPageTypes();
            return result;
        }

        public IList<Module> GetModules()
        {
            var result = _moduleRepository.GetModules();
            return result;
        }

        public IList<Theme> GetThemes()
        {
            var themes = _themeManager.GetHostThemes().Select(kvp => new Theme() { Key = kvp.Value, Value = kvp.Key })
                .ToList();
            return themes;
        }

        private PageViewModel ConvertToPageViewModel(Page page)
        {
            var pageViewModel = ConvertToSinglePageViewModel(page);

            if (page.ChildPage != null && page.ChildPage.Count > 0)
            {
                pageViewModel.ChildPage = new List<PageViewModel>();
                foreach (var child in page.ChildPage)
                {
                    var childPageViewModel = ConvertToPageViewModel(child);
                    pageViewModel.ChildPage.Add(childPageViewModel);
                }
            }
            return pageViewModel;
        }

        private PageViewModel ConvertToSinglePageViewModel(Page page)
        {
            if(page.ParentId==null ||page.ParentId == Guid.Empty)
                return _mapper.Map<PageViewModel>(page);

            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

            var pageTranslation = page.PageTranslation.FirstOrDefault(pt => string.Equals(pt.Locale,
                currentCulture, StringComparison.InvariantCultureIgnoreCase));

            if (pageTranslation == null)
            {
                throw new InvalidOperationException($"Page does not have translation for default locale: {currentCulture}");
            }
            page.PageTranslation.Remove(pageTranslation); // Always remove the default translation from Admin UI, add it back to pageTranslation while updating a page.
            var pageViewModel = _mapper.Map<PageViewModel>(page);
            pageViewModel.Name = pageTranslation.Name;
            pageViewModel.Title = pageTranslation.Title;
            pageViewModel.Description = pageTranslation.Description;
            pageViewModel.PageHeaderTags = pageTranslation.PageHeaderTags;
            pageViewModel.RedirectUrl = pageTranslation.RedirectUrl;
            pageViewModel.IsLinkNewWindow = pageTranslation.IsLinkNewWindow;
            pageViewModel.Url = pageTranslation.URL;
            pageViewModel.Module = pageViewModel.AdminPage != null && pageViewModel.AdminPage.ModuleId != Guid.Empty ? _modules.FirstOrDefault(m => m.Id == pageViewModel.AdminPage.ModuleId) : null;
            pageViewModel.Theme = !string.IsNullOrEmpty(pageViewModel.ThemeSrc)
                ? _themes.FirstOrDefault(t => t.Key == pageViewModel.ThemeSrc)
                : null;

            return pageViewModel;

        }

        private Page ConvertToPage(PageViewModel pageViewModel)
        {
            var page = ConvertToSingePage(pageViewModel);

            if (pageViewModel.ChildPage != null && pageViewModel.ChildPage.Count > 0)
            {
                page.ChildPage = new List<Page>();
                foreach (var child in pageViewModel.ChildPage)
                {
                    var childPage = ConvertToPage(child);
                    page.ChildPage.Add(childPage);
                }
            }
            return page;
        }

        private Page ConvertToSingePage(PageViewModel pageViewModel)
        {
            Page page;
            PageTranslation pageTranslation;
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

            if (pageViewModel.ParentId == null || pageViewModel.ParentId == Guid.Empty)
            {
                return pageViewModel;
            }

            if (pageViewModel.Id == Guid.Empty)
            {
                page = new Page();
                pageTranslation = new PageTranslation();
            }
            else
            {
                page = _navigation.GetPageAndDependencies(pageViewModel.Id);
                pageTranslation = page.PageTranslation.FirstOrDefault(pt => string.Equals(pt.Locale, currentCulture));
            }

            if (pageTranslation == null)
            {
                throw new InvalidOperationException($"Page does not have translation for default locale: {currentCulture}");
            }

            _mapper.Map(pageViewModel, page);
            page.PageTypeId = page.PageType?.Id;
            page.PageType = null;

            pageTranslation.Name = pageViewModel.Name;
            pageTranslation.Title = pageViewModel.Title;
            pageTranslation.Description = pageViewModel.Description;
            pageTranslation.PageHeaderTags = pageViewModel.PageHeaderTags;
            pageTranslation.RedirectUrl = pageViewModel.RedirectUrl;
            pageTranslation.IsLinkNewWindow = pageViewModel.IsLinkNewWindow;

            //Default page translation will be removed for both new and existing pages. Therefore, add it while updating a pge.
            page.PageTranslation.Add(pageTranslation);

            if (pageViewModel.Module != null)
            {
                page.AdminPage = new AdminPage()
                {
                    ModuleId = pageViewModel.Module.Id,
                    PageId = pageViewModel.Id
                };
            }

            page.ThemeSrc = pageViewModel.Theme?.Key;

            return page;
        }
    }
}
