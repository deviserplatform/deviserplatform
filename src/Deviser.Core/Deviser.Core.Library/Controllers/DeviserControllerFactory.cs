using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Module = Deviser.Core.Data.Entities.Module;

namespace Deviser.Core.Library.Controllers
{
    public class DeviserControllerFactory : IDeviserControllerFactory
    {
        private IPageProvider pageProvider;
        private IModuleProvider moduleProvider;
        private IActionSelector actionSelector;
        private IModuleInvokerProvider moduleInvokerProvider;

        public DeviserControllerFactory(ILifetimeScope container)
        {
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
        public async Task<Dictionary<string, string>> GetPageModuleResults(ActionContext actionContext, int pageId)
        {
            Dictionary<string, string> actionResults = new Dictionary<string, string>();
            Page currentPage = pageProvider.GetPage(pageId);
            if (currentPage.PageModule != null && currentPage.PageModule.Count > 0)
            {
                currentPage.PageModule = currentPage.PageModule.Where(pm => !pm.IsDeleted).ToList();
                foreach (var pageModule in currentPage.PageModule)
                {
                    Module module = moduleProvider.Get(pageModule.ModuleId);
                    ModuleAction moduleAction = module.ModuleAction.FirstOrDefault(ma => ma.ModuleActionTypeId == (int)DomainTypes.ModuleActionType.View);
                    ModuleContext moduleContext = new ModuleContext();
                    moduleContext.ModuleInfo = module;
                    moduleContext.PageModuleId = pageModule.Id;
                    if (module != null && moduleAction != null)
                    {
                        try
                        {
                            string moduleStringResult = await ExecuteModuleController(actionContext, moduleContext, moduleAction);
                            actionResults.Add(pageModule.ContainerId.ToString(), moduleStringResult);
                        }
                        catch (Exception ex)
                        {
                            var actionResult = "Module Load exception occured";
                            actionResults.Add(pageModule.ContainerId.ToString(), actionResult);
                            //TODO: Log exception
                        }
                    }
                }
            }

            return actionResults;
        }

        public async Task<string> ExecuteModuleController(ActionContext actionContext, string moduleName, int pageModuleId, string controllerName, string actionName)
        {
            string actionResult = null;
            Module module = moduleProvider.Get(moduleName);
            if (module == null || pageModuleId == 0)
                return "Invalid module or module context";

            ModuleAction moduleAction = module.ModuleAction.FirstOrDefault(ma => ma.ModuleActionType.ControlType == "VIEW");
            if (moduleAction == null)
                return "Invalid action";

            PageModule pageModule = pageProvider.GetPageModule(pageModuleId);

            if (pageModule == null)
                return "Invalid pageModuleId";

            ModuleContext moduleContext = new ModuleContext();
            moduleContext.ModuleInfo = module;
            moduleContext.PageModuleId = pageModuleId;
            actionResult = await ExecuteModuleController(actionContext, moduleContext, moduleAction);

            return actionResult;
        }


        private async Task<string> ExecuteModuleController(ActionContext actionContext, ModuleContext moduleContext, ModuleAction moduleAction)
        {
            //var routeData = new RouteData();
            //string url = "Modules/{moduleName}/{pageModuleId}/{controller}/{action}/{id}";
            //routeData.Route = new Route(url, controllerContext.RouteData.RouteHandler);
            //routeData.Values.Add("moduleName", moduleContext.ModuleInfo.Name);
            //routeData.Values.Add("pageModuleId", moduleContext.PageModuleId);
            //routeData.Values.Add("controller", moduleAction.ControllerName);
            //routeData.Values.Add("action", moduleAction.ActionName);
            //routeData.DataTokens.Add("Namespaces", new string[] { moduleAction.ControllerNamespace });

            ////controllerName = "Deviser.Modules.HtmlModule.Controllers." + controllerName;

            //var requestContext = new RequestContext(controllerContext.HttpContext, routeData);
            //IController controller = controllerFactory.CreateController(requestContext, moduleAction.ControllerName);


            //var moduleController = controller as IModuleController;

            //moduleController.ModuleContext = moduleContext;
            //moduleController.CanExecuteAction = false;
            //moduleController.Execute(requestContext);

            //ActionResult result = moduleController.ResultOfLastExecute;

            //return new SDActionResult
            //{
            //    ActionResult = result,
            //    ControllerContext = moduleController.SDControllerContext
            //};

            if (actionContext == null)
            {

            }

            RouteContext context = new RouteContext(actionContext.HttpContext);
            context.RouteData = new RouteData();
            context.RouteData.Values.Add("area", moduleContext.ModuleInfo.Name);
            context.RouteData.Values.Add("pageModuleId", moduleContext.PageModuleId);
            context.RouteData.Values.Add("controller", moduleAction.ControllerName);
            context.RouteData.Values.Add("action", moduleAction.ActionName);


            var actionDescriptor = await actionSelector.SelectAsync(context);
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
