using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;
using ContentResult = Deviser.Core.Common.DomainTypes.ContentResult;
using Module = Deviser.Core.Data.Entities.Module;

namespace Deviser.Core.Library.Controllers
{
    public class DeviserControllerFactory : IDeviserControllerFactory
    {
        private readonly ILogger<DeviserControllerFactory> logger;

        private IPageProvider pageProvider;
        private IModuleProvider moduleProvider;
        private IActionSelector actionSelector;
        private IModuleInvokerProvider moduleInvokerProvider;

        public DeviserControllerFactory(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<DeviserControllerFactory>>();
            actionSelector = container.Resolve<IActionSelector>();
            moduleInvokerProvider = container.Resolve<IModuleInvokerProvider>();
            pageProvider = container.Resolve<IPageProvider>();
            moduleProvider = container.Resolve<IModuleProvider>();
        }

        /// <summary>
        /// This method gets current page "View" actions of all modules
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<ContentResult>>> GetPageModuleResults(ActionContext actionContext, Guid pageId)
        {
            Dictionary<string, List<ContentResult>> actionResults = new Dictionary<string, List<ContentResult>>();
            Page currentPage = pageProvider.GetPage(pageId);
            if (currentPage.PageModule != null && currentPage.PageModule.Count > 0)
            {
                currentPage.PageModule = currentPage.PageModule.Where(pm => !pm.IsDeleted).ToList();
                foreach (var pageModule in currentPage.PageModule)
                {
                    Module module = moduleProvider.Get(pageModule.ModuleId);
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
                            string moduleStringResult = await ExecuteModuleController(actionContext, moduleContext, moduleAction);
                            moduleStringResult = $"<div class=\"sd-module-container\" data-module=\"{moduleContext.ModuleInfo.Name}\" data-page-module-id=\"{moduleContext.PageModuleId}\">{moduleStringResult}</div>";
                            contentResults.Add(new ContentResult
                            {
                                Result = moduleStringResult,
                                SortOrder = pageModule.SortOrder
                            });
                        }
                        catch (Exception ex)
                        {
                            var actionResult = "Module load exception has been occured";
                            contentResults.Add(new ContentResult
                            {
                                Result = actionResult,
                                SortOrder = pageModule.SortOrder
                            });
                            logger.LogError("Module load exception has been occured", ex);
                        }
                    }
                }
            }

            return actionResults;
        }

        public async Task<string> GetModuleEditResult(ActionContext actionContext, Guid pageModuleId, Guid moduleActionid)
        {
            string actionResult = string.Empty;
            try
            {
                var pageModule = pageProvider.GetPageModule(pageModuleId);
                var module = moduleProvider.Get(pageModule.ModuleId);
                ModuleAction moduleAction = module.ModuleAction.FirstOrDefault(ma => ma.Id == moduleActionid);
                ModuleContext moduleContext = new ModuleContext();
                moduleContext.ModuleInfo = module;
                moduleContext.PageModuleId = pageModule.Id;
                if (module != null && moduleAction != null)
                {
                    List<ContentResult> contentResults = new List<ContentResult>();
                    actionResult = await ExecuteModuleController(actionContext, moduleContext, moduleAction);
                    actionResult = $"<div class=\"sd-module-container\" data-module=\"{moduleContext.ModuleInfo.Name}\" data-page-module-id=\"{moduleContext.PageModuleId}\">{actionResult}</div>";
                }
            }
            catch (Exception ex)
            {
                actionResult = "Module load exception has been occured";
                logger.LogError("Module load exception has been occured", ex);
            }

            return actionResult;
        }
        
        private async Task<string> ExecuteModuleController(ActionContext actionContext, ModuleContext moduleContext, ModuleAction moduleAction)
        {          
            if (actionContext == null)
            {
                return null;
            }

            RouteContext context = new RouteContext(actionContext.HttpContext);
            context.RouteData = new RouteData();
            context.RouteData.Values.Add("area", moduleContext.ModuleInfo.Name);
            context.RouteData.Values.Add("pageModuleId", moduleContext.PageModuleId);
            context.RouteData.Values.Add("controller", moduleAction.ControllerName);
            context.RouteData.Values.Add("action", moduleAction.ActionName);
            context.RouteData.PushState(actionContext.RouteData.Routers[0], null, null);


            var actionDescriptions = actionSelector.SelectCandidates(context);
            var actionDescriptor = actionSelector.SelectBestCandidate(context, actionDescriptions);
            if (actionDescriptor == null)
                throw new NullReferenceException("Action cannot be located, please check whether module has been installed properly");

            var moduleActionContext = new ActionContext(actionContext.HttpContext, context.RouteData, actionDescriptor);

            var invoker = moduleInvokerProvider.CreateInvoker(moduleActionContext);
            var result = await invoker.InvokeAction() as ViewResult;
            string strResult = result.ExecuteResultToString(moduleActionContext);
            return strResult;

        }
    }
}
