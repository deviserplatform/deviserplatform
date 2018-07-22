using Autofac;
using Deviser.Core.Common;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Library.Modules;

namespace Deviser.WI.Controllers
{
    public class PageController : DeviserController, IDisposable
    {
        private readonly ILogger<PageController> _logger;

        [ActionContext]
        public ActionContext Context { get; set; }


        private readonly ILifetimeScope _container;
        private readonly IPageRepository _pageRepository;
        //private readonly IPageManager _pageManager;
        private readonly IDeviserControllerFactory _deviserControllerFactory;
        private readonly IScopeService _scopeService;
        private readonly IContentManager _contentManager;
        private readonly IModuleManager _moduleManager;
        private readonly IInstallationProvider _installationManager;

        private static bool _isInstalled;
        private static bool _isDbExist;

        public PageController(ILifetimeScope container, IScopeService scopeService)
        {
            _container = container;
            _installationManager = container.Resolve<IInstallationProvider>();
            _logger = container.Resolve<ILogger<PageController>>();
            _scopeService = scopeService;

            if (!_isInstalled)
                _isInstalled = _installationManager.IsPlatformInstalled;

            if(!_isDbExist)
                _isDbExist = _installationManager.IsDatabaseExist;

            if (_isInstalled)
            {
                _pageRepository = container.Resolve<IPageRepository>();
                //_pageManager = container.Resolve<IPageManager>();
                _deviserControllerFactory = container.Resolve<IDeviserControllerFactory>();
                _contentManager = container.Resolve<IContentManager>();
                _moduleManager = container.Resolve<IModuleManager>();
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
                    Page currentPage = _scopeService.PageContext.CurrentPage;
                    FilterPageElements(currentPage);
                    if (currentPage != null)
                    {
                        if (_scopeService.PageContext.HasPageViewPermission)
                        {
                            Dictionary<string, List<Core.Common.DomainTypes.ContentResult>> moduleActionResults = await _deviserControllerFactory.GetPageModuleResults(Context);
                            ViewBag.ModuleActionResults = moduleActionResults;
                            return View(currentPage);
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
        [Route("[controller]/[action]/pageModuleId/{pageModuleId}/moduleActionId/{moduleActionId}")]
        public IActionResult EditModule(string currentLink, Guid pageModuleId, Guid moduleActionId)
        {
            if (pageModuleId != Guid.Empty)
            {
                try
                {
                    var pageModule = _pageRepository.GetPageModule(pageModuleId); //It referes PageModule's View ModuleActionType

                    if (pageModule == null)
                        return NotFound();

                    //if (moduleManager.HasEditPermission(pageModule))
                    if (_scopeService.PageContext.HasPageEditPermission)
                    {
                        object result = _deviserControllerFactory.GetModuleEditResult(Context, pageModule, moduleActionId).Result;
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
