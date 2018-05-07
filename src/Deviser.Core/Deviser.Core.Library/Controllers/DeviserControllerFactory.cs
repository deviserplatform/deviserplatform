using Autofac;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Library.Internal;
using Deviser.Core.Library.Services;
using ContentResult = Deviser.Core.Common.DomainTypes.ContentResult;
using Module = Deviser.Core.Common.DomainTypes.Module;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;

namespace Deviser.Core.Library.Controllers
{
    public class DeviserControllerFactory : IDeviserControllerFactory
    {
        private readonly ILogger<DeviserControllerFactory> _logger;        
        private readonly IActionInvoker _actionInvoker;
        private readonly IActionSelector _actionSelector;
        private readonly IScopeService _scopeService;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IModuleRepository _moduleRepository;
        private readonly IPageRepository _pageRepository;
        

        public DeviserControllerFactory(ILifetimeScope container, IScopeService scopeService)
        {
            _logger = container.Resolve<ILogger<DeviserControllerFactory>>();
            _actionInvoker = container.Resolve<IActionInvoker>();
            _actionSelector = container.Resolve<IActionSelector>();            
            _htmlHelper = container.Resolve<IHtmlHelper>();
            _pageRepository = container.Resolve<IPageRepository>();
            _moduleRepository = container.Resolve<IModuleRepository>();
            _scopeService = scopeService;
        }

        /// <summary>
        /// This method gets current page "View" actions of all modules
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<ContentResult>>> GetPageModuleResults(ActionContext actionContext)
        {
            Dictionary<string, List<ContentResult>> actionResults = new Dictionary<string, List<ContentResult>>();
            Page currentPage = _scopeService.PageContext.CurrentPage;
            if (currentPage.PageModule != null && currentPage.PageModule.Count > 0)
            {
                currentPage.PageModule = currentPage.PageModule.Where(pm => !pm.IsDeleted).ToList();
                foreach (var pageModule in currentPage.PageModule)
                {
                    Module module = _moduleRepository.Get(pageModule.ModuleId);
                    ModuleAction moduleAction = module.ModuleAction.FirstOrDefault(ma => ma.Id == pageModule.ModuleActionId);
                    ModuleContext moduleContext = new ModuleContext();
                    moduleContext.ModuleInfo = module;
                    moduleContext.PageModuleId = pageModule.Id;
                    if (module != null && moduleAction != null)
                    {
                        List<ContentResult> contentResults;
                        string containerId = pageModule.ContainerId.ToString();
                        //Prepare the result object
                        if (actionResults.ContainsKey(containerId))
                        {
                            contentResults = actionResults[containerId];
                        }
                        else
                        {
                            contentResults = new List<ContentResult>();
                            actionResults.Add(containerId, contentResults);
                        }

                        try
                        {
                            IHtmlContent actionResult = await ExecuteModuleController(actionContext, moduleContext, moduleAction);
                            IHtmlContent moduleResult = GetModuleResult(actionResult, moduleContext);
                            contentResults.Add(new ContentResult
                            {
                                HtmlResult = moduleResult,
                                SortOrder = pageModule.SortOrder
                            });
                        }
                        catch (Exception ex)
                        {
                            var actionResult = "Module load exception has been occured";
                            contentResults.Add(new ContentResult
                            {
                                //Result = actionResult,
                                HtmlResult = new HtmlString(actionResult),
                                SortOrder = pageModule.SortOrder
                            });
                            _logger.LogError("Module load exception has been occured", ex);
                        }
                    }
                }
            }

            return actionResults;
        }

        public async Task<string> GetModuleEditResultAsString(ActionContext actionContext, PageModule pageModule, Guid moduleEditActionId)
        {
            string actionResult = string.Empty;
            try
            {
                var module = _moduleRepository.Get(pageModule.ModuleId);
                ModuleAction moduleAction = module.ModuleAction.FirstOrDefault(ma => ma.Id == moduleEditActionId); //It referes PageModule's Edit ModuleActionType
                ModuleContext moduleContext = new ModuleContext();
                moduleContext.ModuleInfo = module; //Context should be PageModule's instance, but not Edit ModuleActionType
                moduleContext.PageModuleId = pageModule.Id;
                if (module != null && moduleAction != null)
                {
                    actionResult = await ExecuteModuleControllerAsString(actionContext, moduleContext, moduleAction);
                    actionResult = $"<div class=\"dev-module-container\" data-module=\"{moduleContext.ModuleInfo.Name}\" data-page-module-id=\"{moduleContext.PageModuleId}\">{actionResult}</div>";
                }
            }
            catch (Exception ex)
            {
                actionResult = "Module load exception has been occured";
                _logger.LogError("Module load exception has been occured", ex);
            }

            return actionResult;
        }

        public async Task<IHtmlContent> GetModuleEditResult(ActionContext actionContext, PageModule pageModule, Guid moduleEditActionId)
        {
            IHtmlContent editResult = new HtmlString(string.Empty);
            try
            {
                var module = _moduleRepository.Get(pageModule.ModuleId);
                ModuleAction moduleAction = module.ModuleAction.FirstOrDefault(ma => ma.Id == moduleEditActionId); //It referes PageModule's Edit ModuleActionType
                ModuleContext moduleContext = new ModuleContext();
                moduleContext.ModuleInfo = module; //Context should be PageModule's instance, but not Edit ModuleActionType
                moduleContext.PageModuleId = pageModule.Id;
                if (module != null && moduleAction != null)
                {
                    
                    IHtmlContent actionResult = await ExecuteModuleController(actionContext, moduleContext, moduleAction);
                    editResult = GetModuleResult(actionResult, moduleContext);
                }
            }
            catch (Exception ex)
            {
                editResult = new HtmlString("Module load exception has been occured");
                _logger.LogError("Module load exception has been occured", ex);
            }

            return editResult;
        }

        public IHtmlContent GetModuleResult(IHtmlContent actionResult, ModuleContext moduleContext)
        {
            var moduleContainer = new TagBuilder("div");
            moduleContainer.Attributes.Add("class", "dev-module-container");
            moduleContainer.Attributes.Add("data-module", moduleContext.ModuleInfo.Name);
            moduleContainer.Attributes.Add("data-page-module-id", moduleContext.PageModuleId.ToString());
            moduleContainer.InnerHtml.AppendHtml(actionResult);
            return moduleContainer;
        }

        private async Task<string> ExecuteModuleControllerAsString(ActionContext actionContext, ModuleContext moduleContext, ModuleAction moduleAction)
        {
            if (actionContext == null)
            {
                return null;
            }

            ActionContext moduleActionContext = GetModuleActionContext(actionContext, moduleContext, moduleAction);

            //var invoker = _moduleInvokerProvider.CreateInvoker(moduleActionContext);
            //var result = await invoker.InvokeAction() as ViewResult;
            var result = await _actionInvoker.InvokeAction(actionContext.HttpContext, moduleAction, moduleActionContext) as ViewResult;

            string strResult = result.ExecuteResultToString(moduleActionContext);
            return strResult;
        }

        private async Task<IHtmlContent> ExecuteModuleController(ActionContext actionContext, ModuleContext moduleContext, ModuleAction moduleAction)
        {
            if (actionContext == null)
            {
                return null;
            }

            ActionContext moduleActionContext = GetModuleActionContext(actionContext, moduleContext, moduleAction);

            //var invoker = _moduleInvokerProvider.CreateInvoker(moduleActionContext);
            //var result = await invoker.InvokeAction() as ViewResult;
            var result = await _actionInvoker.InvokeAction(actionContext.HttpContext, moduleAction, moduleActionContext) as ViewResult;

            IHtmlContent htmlResult = result.ExecuteResultToHTML(moduleActionContext);
            return htmlResult;
        }

        private ActionContext GetModuleActionContext(ActionContext actionContext, ModuleContext moduleContext, ModuleAction moduleAction)
        {
            RouteContext context = new RouteContext(actionContext.HttpContext);
            context.RouteData = new RouteData();
            context.RouteData.Values.Add("area", moduleContext.ModuleInfo.Name);
            context.RouteData.Values.Add("pageModuleId", moduleContext.PageModuleId);
            context.RouteData.Values.Add("controller", moduleAction.ControllerName);
            context.RouteData.Values.Add("action", moduleAction.ActionName);
            context.RouteData.PushState(actionContext.RouteData.Routers[0], null, null);


            var actionDescriptions = _actionSelector.SelectCandidates(context);
            var actionDescriptor = _actionSelector.SelectBestCandidate(context, actionDescriptions);
            if (actionDescriptor == null)
                throw new NullReferenceException("Action cannot be located, please check whether module has been installed properly");

            var moduleActionContext = new ActionContext(actionContext.HttpContext, context.RouteData, actionDescriptor);
            return moduleActionContext;
        }

        public void Dispose()
        {
            _actionInvoker.Dispose();
        }
    }
}
