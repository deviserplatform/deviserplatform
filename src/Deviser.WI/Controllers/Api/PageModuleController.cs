using Autofac;
using Deviser.Core.Data.DataProviders;
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
using Deviser.Core.Library.Sites;
using AutoMapper;
using Deviser.Core.Library.Services;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class PageModuleController : Controller
    {
        //Logger
        private readonly ILogger<PageModuleController> logger;

        private IPageProvider pageProvider;
        private IModuleManager moduleManager;
        private IPageManager pageManager;
        private IScopeService scopeService;

        public PageModuleController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<PageModuleController>>();
            pageProvider = container.Resolve<IPageProvider>();
            moduleManager = container.Resolve<IModuleManager>();
            pageManager = container.Resolve<IPageManager>();
            scopeService = container.Resolve<IScopeService>();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var dbResult = moduleManager.GetPageModule(id);                
                if (dbResult != null && moduleManager.HasEditPermission(dbResult))
                {
                    var result = Mapper.Map<List<PageModule>>(dbResult);
                    return Ok(result);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting pageModule, id: ", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("page/{pageId}")]
        public IActionResult GetByPage(Guid pageId)
        {
            try
            {
                var dbResult = moduleManager.GetPageModuleByPage(pageId);                
                if (dbResult != null)
                {  
                    var result = Mapper.Map<List<PageModule>>(dbResult);
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting pageModules, pageId: ", pageId), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Post([FromBody]PageModule pageModule)
        {
            try
            {
                if (pageModule != null)
                {
                    var page = pageProvider.GetPage(pageModule.Id);
                    if (pageManager.HasEditPermission(page)) //Check edit permission for the page
                    {
                        var dbResult = moduleManager.CreateUpdatePageModule(Mapper.Map<Deviser.Core.Data.Entities.PageModule>(pageModule));
                        var result = Mapper.Map<PageModule>(dbResult);
                        if (result != null)
                            return Ok(result);
                    }
                    return Unauthorized();
                }

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
                if (pageModules == null || pageModules.Count == 0)
                    return BadRequest();

                var page = pageProvider.GetPage(pageModules.First().PageId);
                if (pageManager.HasEditPermission(page))//Check edit permission for the page
                {
                    moduleManager.UpdatePageModules(Mapper.Map<List<Deviser.Core.Data.Entities.PageModule>>(pageModules));
                    return Ok();
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating pageModules"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("permission/")]
        public IActionResult Put([FromBody] PageModule pageModule)
        {
            try
            {
                if (pageModule == null || pageModule.ModulePermissions == null || pageModule.ModulePermissions.Count == 0)
                    return BadRequest();

                var page = pageProvider.GetPage(pageModule.PageId);
                if (pageManager.HasEditPermission(page)) //Check edit permission for the page
                {
                    moduleManager.UpdateModulePermission(Mapper.Map<Deviser.Core.Data.Entities.PageModule>(pageModule));
                    return Ok();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating pageModule permissions"), ex);
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
                    var page = pageProvider.GetPage(pageModule.PageId);
                    if (pageManager.HasEditPermission(page)) //Check edit permission for the page
                    {
                        pageModule.IsDeleted = true;
                        pageProvider.UpdatePageModule(pageModule);
                        return Ok();
                    }
                    return Unauthorized();
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
