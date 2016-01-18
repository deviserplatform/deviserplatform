using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Deviser.Core.Library.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class PageModuleController : Controller
    {
        //Logger
        private readonly ILogger<PageModuleController> logger;

        IPageProvider pageProvider;
        IModuleManager moduleManager;

        public PageModuleController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<PageModuleController>>();
            pageProvider = container.Resolve<IPageProvider>();
            moduleManager = container.Resolve<IModuleManager>();
        }

        [HttpPost]
        public IActionResult Post(PageModule pageModule)
        {
            try
            {
                var result = moduleManager.CreatePageModule(pageModule);
                if (result != null)
                    return Ok(result);
                return HttpBadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a pageModule, moduleId: ", pageModule.ModuleId), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var pageModule = pageProvider.GetPageModule(id);
                if (pageModule != null)
                {
                    pageModule.IsDeleted = true;
                    pageProvider.UpdatePageModule(pageModule);
                    return Ok();
                }
                return HttpBadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while deleting page module"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


    }
}
