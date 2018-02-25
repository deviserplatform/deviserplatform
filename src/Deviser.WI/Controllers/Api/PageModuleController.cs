using Autofac;
using Deviser.Core.Data.Repositories;
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
        private readonly ILogger<PageModuleController> _logger;
        private readonly IPageRepository _pageRepository;
        private readonly IModuleManager _moduleManager;
        private readonly IPageManager _pageManager;
        private readonly IScopeService _scopeService;

        public PageModuleController(ILifetimeScope container)
        {
            _logger = container.Resolve<ILogger<PageModuleController>>();
            _pageRepository = container.Resolve<IPageRepository>();
            _moduleManager = container.Resolve<IModuleManager>();
            _pageManager = container.Resolve<IPageManager>();
            _scopeService = container.Resolve<IScopeService>();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var dbResult = _moduleManager.GetPageModule(id);                
                if (dbResult != null && _moduleManager.HasEditPermission(dbResult))
                {
                    var result = Mapper.Map<PageModule>(dbResult);
                    return Ok(result);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting pageModule, id: ", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("page/{pageId}")]
        public IActionResult GetByPage(Guid pageId)
        {
            try
            {
                var dbResult = _moduleManager.GetPageModuleByPage(pageId);                
                if (dbResult != null)
                {  
                    var result = Mapper.Map<List<PageModule>>(dbResult);
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting pageModules, pageId: ", pageId), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("list/")]
        public IActionResult Get()
        {
            try
            {
                var dbResult = _moduleManager.GetDeletedPageModules();
                if (dbResult != null)
                {
                    var result = Mapper.Map<List<PageModule>>(dbResult);
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting deleted pageModules"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult CreateUpdatePageModule([FromBody]PageModule pageModule)
        {
            try
            {
                if (pageModule != null)
                {
                    var page = _pageRepository.GetPage(pageModule.PageId);
                    if (_pageManager.HasEditPermission(page)) //Check edit permission for the page
                    {
                        var dbResult = _moduleManager.CreateUpdatePageModule(pageModule);
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
                _logger.LogError(string.Format("Error occured while creating a pageModule, moduleId: ", pageModule.ModuleId), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("list/")]
        public IActionResult UpdatePageModules([FromBody]List<PageModule> pageModules)
        {
            try
            {
                if (pageModules == null || pageModules.Count == 0)
                    return BadRequest();

                var page = _pageRepository.GetPage(pageModules.First().PageId);
                if (_pageManager.HasEditPermission(page))//Check edit permission for the page
                {
                    _moduleManager.UpdatePageModules(pageModules);
                    return Ok();
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating pageModules"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("permission/")]
        public IActionResult UpdatePermission([FromBody] PageModule pageModule)
        {
            try
            {
                if (pageModule == null || pageModule.ModulePermissions == null || pageModule.ModulePermissions.Count == 0)
                    return BadRequest();

                var page = _pageRepository.GetPage(pageModule.PageId);
                if (_pageManager.HasEditPermission(page)) //Check edit permission for the page
                {
                    _moduleManager.UpdateModulePermission(pageModule);
                    return Ok();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating pageModule permissions"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("restore/{id}")]
        public IActionResult RestorePageModule(Guid id)
        {
            try
            {
                var result = _pageRepository.RestorePageModule(id);
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating pageModule permissions"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var pageModule = _pageRepository.GetPageModule(id);
                if (pageModule != null)
                {
                    var page = _pageRepository.GetPage(pageModule.PageId);
                    if (_pageManager.HasEditPermission(page)) //Check edit permission for the page
                    {
                        pageModule.IsDeleted = true;
                        _pageRepository.UpdatePageModule(pageModule);
                        return Ok();
                    }
                    return Unauthorized();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while deleting page module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeletePageModule(Guid id)
        {
            try
            {
                var pageModule = _pageRepository.GetPageModule(id);
                if (pageModule != null)
                {
                    var page = _pageRepository.GetPage(pageModule.PageId);
                    if (_pageManager.HasEditPermission(page)) //Check edit permission for the page
                    {
                        bool result = _pageRepository.DeletePageModule(id);
                        if (result)
                            return Ok();

                    }
                    return Unauthorized();
                }   
                return BadRequest();

            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("Error occured while deleting page module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


    }
}
