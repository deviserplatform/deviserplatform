using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Admin.Services;
using Deviser.Core.Common;
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
        private readonly ILanguageManager _languageManager;
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
            ILanguageManager languageManager,
            IPageRepository pageRepository,
            IModuleRepository moduleRepository,
            INavigation navigation,
            IScopeService scopeService,
            IThemeManager themeManager)
        {
            _logger = logger;
            _languageManager = languageManager;
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
            var translateLanguages = GetTranslateLanguages();
            var isMultilingual = translateLanguages.Count > 0;
            var pageTree = _pageRepository.GetPageTree(true);
            return await Task.FromResult(ConvertToPageViewModel(pageTree, isMultilingual, translateLanguages));
        }

        public async Task<PageViewModel> GetItem(string itemId)
        {
            var translateLanguages = GetTranslateLanguages();
            var isMultilingual = translateLanguages.Count > 0;

            var result = _navigation.GetPageAndDependencies(Guid.Parse(itemId));
            return await Task.FromResult(ConvertToPageViewModel(result, isMultilingual, translateLanguages));
        }

        public async Task<IFormResult<PageViewModel>> CreateItem(PageViewModel item)
        {
            var translateLanguages = GetTranslateLanguages();
            var isMultilingual = translateLanguages.Count > 0;

            var pageResult = _navigation.CreatePage(ConvertToSingePage(item, isMultilingual, translateLanguages));
            if (pageResult == null)
                return new FormResult<PageViewModel>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to create a page"
                };
            var result = new FormResult<PageViewModel>(ConvertToPageViewModel(pageResult, isMultilingual, translateLanguages))
            {
                IsSucceeded = true,
                SuccessMessage = "Page has been created"
            };
            return await Task.FromResult(result);

        }

        public async Task<IFormResult<PageViewModel>> UpdateItem(PageViewModel item)
        {
            var translateLanguages = GetTranslateLanguages();
            var isMultilingual = translateLanguages.Count > 0;

            var pageResult = _navigation.UpdatePageAndChildren(ConvertToSingePage(item, isMultilingual, translateLanguages));
            if (pageResult == null)
                return new FormResult<PageViewModel>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to update page tree"
                };
            var result = new FormResult<PageViewModel>(ConvertToPageViewModel(pageResult, isMultilingual, translateLanguages))
            {
                IsSucceeded = true,
                SuccessMessage = "Page has been updated"
            };
            return await Task.FromResult(result);

        }

        public async Task<IFormResult<PageViewModel>> UpdateTree(PageViewModel item)
        {
            var translateLanguages = GetTranslateLanguages();
            var isMultilingual = translateLanguages.Count > 0;
            var pageTreeToBeUpdated = ConvertToPage(item, isMultilingual, translateLanguages);
            var pageResult = _navigation.UpdatePageTree(pageTreeToBeUpdated);
            if (pageResult == null)
                return new FormResult<PageViewModel>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to update page tree"
                };
            //UpdateSinglePage returns only the single node. However, the UI expects full tree
            var pageViewModel = await GetTree();
            var result = new FormResult<PageViewModel>(pageViewModel)
            {
                IsSucceeded = true,
                SuccessMessage = "Page tree has been updated"
            };
            return await Task.FromResult(result);

        }

        public async Task<IAdminResult<PageViewModel>> DeleteItem(string itemId)
        {

            var page = _pageRepository.GetPage(Guid.Parse(itemId));
            if (page == null)
                return new FormResult<PageViewModel>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to delete page"
                };
            page.IsActive = false;
            var pageResult = _pageRepository.UpdatePageActiveAndLayout(page);
            if (pageResult == null)
                return new FormResult<PageViewModel>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to delete page"
                };
            //UpdateSinglePage returns only the single node. However, the UI expects full tree
            var pageViewModel = await GetTree();
            var result = new FormResult<PageViewModel>(pageViewModel)
            {
                IsSucceeded = true,
                SuccessMessage = "Page has been deleted"
            };
            return await Task.FromResult(result);

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

        public bool IsSiteMultilingual()
        {
            var activeLanguages = GetActiveLanguages();
            return activeLanguages?.Count > 1;
        }

        public IList<Language> GetTranslateLanguages()
        {
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            var activeLanguages = GetActiveLanguages();
            //Excluding current culture since the current culture translation will be embedded in PageViewModel
            return activeLanguages.Where(l => !string.Equals(l.CultureCode,
                currentCulture, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        private PageViewModel ConvertToPageViewModel(Page page, bool isMultilingual, IList<Language> translateLanguages)
        {
            var pageViewModel = ConvertToSinglePageViewModel(page, isMultilingual, translateLanguages);

            if (page.ChildPage == null || page.ChildPage.Count <= 0) return pageViewModel;

            pageViewModel.ChildPage = new List<PageViewModel>();
            foreach (var child in page.ChildPage)
            {
                var childPageViewModel = ConvertToPageViewModel(child, isMultilingual, translateLanguages);
                pageViewModel.ChildPage.Add(childPageViewModel);
            }
            return pageViewModel;
        }

        private PageViewModel ConvertToSinglePageViewModel(Page page, bool isMultilingual, IList<Language> translateLanguages)
        {
            if (page.ParentId == null || page.ParentId == Guid.Empty)
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
            if (page.AdminPage != null && page.AdminPage.ModuleId != Guid.Empty)
            {
                pageViewModel.Module = _modules.FirstOrDefault(m => m.Id == page.AdminPage.ModuleId);
                pageViewModel.ModelName = page.AdminPage.ModelName;
            }

            pageViewModel.Theme = !string.IsNullOrEmpty(pageViewModel.ThemeSrc)
                ? _themes.FirstOrDefault(t => t.Key == pageViewModel.ThemeSrc)
                : null;

            if (!isMultilingual || pageViewModel?.PageTranslation == null || pageViewModel.PageTranslation.Count <= 0)
                return pageViewModel;

            foreach (var translation in pageViewModel.PageTranslation)
            {
                translation.Language =
                    translateLanguages.FirstOrDefault(t => string.Equals(t?.CultureCode, translation?.Locale));
            }


            return pageViewModel;

        }

        private Page ConvertToPage(PageViewModel pageViewModel, bool isMultilingual, IList<Language> translateLanguages)
        {
            var page = ConvertToSingePage(pageViewModel, isMultilingual, translateLanguages);

            if (pageViewModel.ChildPage == null || pageViewModel.ChildPage.Count <= 0) return page;

            page.ChildPage = new List<Page>();
            foreach (var child in pageViewModel.ChildPage)
            {
                var childPage = ConvertToPage(child, isMultilingual, translateLanguages);
                page.ChildPage.Add(childPage);
            }
            return page;
        }

        private Page ConvertToSingePage(PageViewModel pageViewModel, bool isMultilingual, IList<Language> translateLanguages)
        {
            Page page;
            PageTranslation pageTranslation;
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

            if (pageViewModel.Parent != null)
            {
                pageViewModel.ParentId = pageViewModel.Parent.Id;
                pageViewModel.Parent = null;
            }

            if (pageViewModel.ParentId == null || pageViewModel.ParentId == Guid.Empty)
            {
                return _mapper.Map<Page>(pageViewModel);
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
            pageTranslation.Locale = currentCulture;
            pageTranslation.Description = pageViewModel.Description;
            pageTranslation.PageHeaderTags = pageViewModel.PageHeaderTags;
            pageTranslation.RedirectUrl = pageViewModel.RedirectUrl;
            pageTranslation.IsLinkNewWindow = pageViewModel.IsLinkNewWindow;

            //Default page translation will be removed for both new and existing pages. Therefore, add it while updating a pge.
            page.PageTranslation.Add(pageTranslation);

            if (isMultilingual && page.PageTranslation.Count > 1)
            {
                foreach (var translation in page.PageTranslation)
                {
                    var selectedLanguage = translateLanguages.FirstOrDefault(t => string.Equals(t?.CultureCode, translation?.Language?.CultureCode,
                            StringComparison.InvariantCultureIgnoreCase));

                    if (selectedLanguage != null)
                    {
                        translation.Locale = selectedLanguage.CultureCode;
                    }
                }
            }

            if (pageViewModel.Module != null)
            {
                page.AdminPage = new AdminPage()
                {
                    ModuleId = pageViewModel.Module.Id,
                    PageId = pageViewModel.Id,
                    ModelName = pageViewModel.ModelName
                };
            }
            else
            {
                page.AdminPage = null;
            }

            page.ThemeSrc = pageViewModel.Theme?.Key;

            return page;
        }

        private IList<Language> GetActiveLanguages()
        {
            return _languageManager.GetActiveLanguages();
        }
    }
}
