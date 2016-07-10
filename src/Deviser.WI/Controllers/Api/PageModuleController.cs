using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("page/{pageId}")]
        public IActionResult Get(Guid pageId)
        {
            try
            {
                var result = moduleManager.GetPageModuleByPage(pageId);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting pageModules, pageId: ", pageId), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]PageModule pageModule)
        {
            try
            {
                var result = moduleManager.CreatePageModule(pageModule);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a pageModule, moduleId: ", pageModule.ModuleId), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("list/")]
        public IActionResult Put([FromBody]List<PageModule> pageModules)
        {
            try
            {                
                if (pageModules== null || pageModules.Count==0)
                    return BadRequest();
                moduleManager.UpdatePageModules(pageModules);
                return Ok();
                
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating pageModules"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
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
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while deleting page module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


    }
}
