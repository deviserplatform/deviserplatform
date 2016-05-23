using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
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
    public class PageContentController : Controller
    {
        private readonly ILogger<PageContentController> logger;

        IPageContentProvider pageContentProvider;
        IModuleManager moduleManager;

        public PageContentController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<PageContentController>>();
            pageContentProvider = container.Resolve<IPageContentProvider>();
            moduleManager = container.Resolve<IModuleManager>();
        }

        [HttpGet("{contentId}")]
        public IActionResult Get(Guid contentId)
        {
            try
            {
                var result = pageContentProvider.Get(contentId);
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
        public IActionResult Get(string cultureCode, int pageId)
        {
            try
            {
                var result = pageContentProvider.Get(pageId, cultureCode);
                if (result != null)
                    return Ok(result);
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
                var result = moduleManager.CreatePageContent(pageContent);
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
                var result = pageContentProvider.Update(pageContent);
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

                pageContentProvider.Update(new List<PageContent>(pageContents));
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating page contents"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var content = pageContentProvider.Get(id);
                if (content != null)
                {
                    content.IsDeleted = true;
                    pageContentProvider.Update(content);
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
