using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Web.Controllers
{
    public class PageController : DeviserController, IDisposable
    {
        private readonly ILogger<PageController> _logger;

        [ActionContext]
        public ActionContext Context { get; set; }

        private readonly IPageRepository _pageRepository;
        private readonly IPageManager _pageManager;
        private readonly IDeviserControllerFactory _deviserControllerFactory;
        private readonly IScopeService _scopeService;
        private readonly IContentManager _contentManager;
        private readonly IModuleManager _moduleManager;
        private readonly IInstallationProvider _installationManager;

        private static bool _isInstalled;
        private static bool _isDbExist;

        public PageController(ILogger<PageController> logger,
            IPageRepository pageRepository,
            IPageManager pageManager,
            IDeviserControllerFactory deviserControllerFactory,
            IScopeService scopeService,
            IContentManager contentManager,
            IModuleManager moduleManager,
            IInstallationProvider installationManager)
        {
            _logger = logger;
            _installationManager = installationManager;
            _scopeService = scopeService;

            if (!_isInstalled)
                _isInstalled = _installationManager.IsPlatformInstalled;

            if (!_isDbExist)
                _isDbExist = _installationManager.IsDatabaseExist;

            if (_isInstalled)
            {
                _pageRepository = pageRepository;
                _pageManager = pageManager;
                _deviserControllerFactory = deviserControllerFactory;
                _contentManager = contentManager;
                _moduleManager = moduleManager;
            }

        }

        public async Task<IActionResult> Index(string permalink)
        {
            try
            {
                if (!_isInstalled)
                {
                    //Not installed, start installation wizzard
                    return RedirectToAction("Index", "Install");
                }

                if (_isInstalled && !_isDbExist)
                {
                    //Platform properly installed, but Database not found. Kindly check the connection string
                    return View("DbNotInstalled");
                }

                if (_isInstalled && _isDbExist)
                {
                    //Platform is properly installed
                    Page currentPage = _pageManager.GetPageAndDependencies(_scopeService.PageContext.CurrentPage.Id);
                    _scopeService.PageContext.CurrentPage = currentPage;
                    FilterPageElements(currentPage);
                    if (currentPage != null)
                    {
                        if (_scopeService.PageContext.HasPageViewPermission)
                        {
                            if (currentPage.PageTypeId == Globals.PageTypeStandard)
                            {
                                Dictionary<string, List<Core.Common.DomainTypes.ContentResult>> moduleViewResult = await _deviserControllerFactory.GetPageModuleResults(Context);
                                ViewBag.ModuleViewResults = moduleViewResult;
                                return View(currentPage);

                            }
                            else if (currentPage.PageTypeId == Globals.PageTypeAdmin)
                            {
                                var result = await _deviserControllerFactory.GetAdminPageResult(Context);
                                ViewBag.AdminResult = result;
                                return View(currentPage);
                            }
                        }
                        else
                        {
                            return View("UnAuthorized");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Page load exception has been occured", ex);
                throw ex;
            }
            return View("NotFound");
        }

        public IActionResult Layout(string permalink)
        {
            Page currentPage = _scopeService.PageContext.CurrentPage;
            FilterPageElements(currentPage);
            if (_scopeService.PageContext != null)
            {
                ViewBag.Theme = Globals.AdminTheme;
                RouteData.Values.Add("permalink", permalink);
                return View(ViewBag);
            }
            return null;
        }

        public IActionResult Edit(string permalink)
        {
            Page currentPage = _scopeService.PageContext.CurrentPage;
            FilterPageElements(currentPage);
            if (currentPage != null && _scopeService.PageContext != null)
            {
                if (_scopeService.PageContext.HasPageEditPermission)
                {
                    ViewBag.Theme = Globals.AdminTheme;
                    RouteData.Values.Add("permalink", permalink);
                    return View(_scopeService.PageContext.CurrentPage);
                }
                else
                {
                    return View("UnAuthorized");
                }

            }
            return View("NotFound");
        }

        [HttpGet]
        [Route("[controller]/[action]/pageModuleId/{pageModuleId}/moduleViewId/{moduleViewId}")]
        public IActionResult EditModule(string currentLink, Guid pageModuleId, Guid moduleViewId)
        {
            if (pageModuleId != Guid.Empty)
            {
                try
                {
                    var pageModule = _pageRepository.GetPageModule(pageModuleId); //It referes PageModule's View ModuleViewType

                    if (pageModule == null)
                        return NotFound();

                    //if (moduleManager.HasEditPermission(pageModule))
                    if (_scopeService.PageContext.HasPageEditPermission)
                    {
                        object result = _deviserControllerFactory.GetModuleEditResult(Context, pageModule, moduleViewId).Result;
                        ViewBag.result = result;
                        return View(result);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Module load exception has been occured", ex);
                    throw ex;
                }
            }
            else
            {
                return BadRequest("Invalid module id");
            }

        }

        private void FilterPageElements(Page currentPage)
        {
            if (currentPage != null)
            {

                if (currentPage.PageContent != null && currentPage.PageContent.Count > 0)
                {
                    //Filter pageContents where current user has view permissions
                    var filteredPageContents = currentPage.PageContent.Where(pc => _contentManager.HasViewPermission(pc, true)).ToList();
                    currentPage.PageContent = filteredPageContents;
                }

                if (currentPage.PageModule != null && currentPage.PageModule.Count > 0)
                {
                    //Filter pageContents where current user has view permissions
                    var filteredPageModules = currentPage.PageModule.Where(pm => _moduleManager.HasViewPermission(pm, true)).ToList();
                    currentPage.PageModule = filteredPageModules;
                }

                //Themes are not used for sometime period
                string theme = "";
                if (!string.IsNullOrEmpty(currentPage.ThemeSrc))
                    theme = currentPage.ThemeSrc;
                else
                    theme = Globals.DefaultTheme;

                theme = theme.Replace("[G]", "~/Sites/Default/");

                ViewBag.Theme = theme;
            }
        }

        //public new void Dispose()
        //{
        //    _deviserControllerFactory.Dispose();
        //    base.Dispose();
        //}


    }
}
