using Autofac;
using AutoMapper;
using Deviser.Core.Data.DataProviders;
//using Deviser.Core.Data.Entities;
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

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class PageContentController : Controller
    {
        private readonly ILogger<PageContentController> logger;

        //private IPageContentProvider pageContentProvider;
        private IModuleManager moduleManager;
        private IContentManager contentManager;

        public PageContentController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<PageContentController>>();
            //pageContentProvider = container.Resolve<IPageContentProvider>();
            moduleManager = container.Resolve<IModuleManager>();
            contentManager = container.Resolve<IContentManager>();
        }

        [HttpGet("{contentId}")]
        public IActionResult Get(Guid contentId)
        {
            try
            {
                var dataResult = contentManager.Get(contentId);
                var result = Mapper.Map<PageContent>(dataResult);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting page content, contentId: {0}", contentId), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{cultureCode}/{pageId}")]
        public IActionResult Get(string cultureCode, Guid pageId)
        {
            try
            {
                var dbResult = contentManager.Get(pageId, cultureCode);
                if (dbResult != null)
                {   
                    var result = Mapper.Map<List<PageContent>>(dbResult);
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting page content, pageId: {0}, cultureCode: {1}", pageId, cultureCode), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] PageContent pageContent)
        {
            try
            {
                var dataResult = contentManager.AddOrUpdatePageContent(Mapper.Map<Deviser.Core.Data.Entities.PageContent>(pageContent));
                var result = Mapper.Map<PageContent>(dataResult);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a page content"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] PageContent pageContent)
        {
            try
            {
                var dataResult = contentManager.AddOrUpdatePageContent(Mapper.Map<Deviser.Core.Data.Entities.PageContent>(pageContent));
                var result = Mapper.Map<PageContent>(dataResult);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating page content, pageContentId", pageContent.Id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("list/")]
        public IActionResult Put([FromBody] IEnumerable<PageContent> pageContents)
        {
            try
            {
                if (pageContents == null || pageContents.Count() == 0)
                    return BadRequest();

                var dataPageContent = Mapper.Map<IEnumerable<Deviser.Core.Data.Entities.PageContent>>(pageContents);
                contentManager.AddOrUpdatePageContents(new List<Deviser.Core.Data.Entities.PageContent>(dataPageContent));
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating page contents"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("permission/")]
        public IActionResult PutPermissions([FromBody] PageContent pageContent)
        {
            try
            {
                if (pageContent == null || pageContent.ContentPermissions ==null || pageContent.ContentPermissions.Count() == 0)
                    return BadRequest();
                
                contentManager.UpdateContentPermission(Mapper.Map<Deviser.Core.Data.Entities.PageContent>(pageContent));
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating page content permissions"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var deleteResult = contentManager.DeletePageContent(id);
                if (deleteResult)
                {                    
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while deleting page content"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
