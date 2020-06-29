using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class PageContentController : Controller
    {
        private readonly ILogger<PageContentController> _logger;
        private readonly IModuleManager _moduleManager;
        private readonly IContentManager _contentManager;

        public PageContentController(ILogger<PageContentController> logger,
            IModuleManager moduleManager,
            IContentManager contentManager)
        {
            _logger = logger;
            _moduleManager = moduleManager;
            _contentManager = contentManager;
        }

        [HttpGet("{contentId}")]
        public IActionResult Get(Guid contentId)
        {
            try
            {
                var result = _contentManager.Get(contentId);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting page content, contentId: {0}", contentId), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{cultureCode}/{pageId}")]
        public IActionResult Get(string cultureCode, Guid pageId)
        {
            try
            {
                var result = _contentManager.Get(pageId, cultureCode);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting page content, pageId: {0}, cultureCode: {1}", pageId, cultureCode), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("list/")]
        public IActionResult Get()
        {
            try
            {
                var result = _contentManager.GetDeletedPageContents();
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting deleted page contents"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] PageContent pageContent)
        {
            try
            {
                var result = _contentManager.AddOrUpdatePageContent(pageContent);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a page content"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] PageContent pageContent)
        {
            try
            {
                var result = _contentManager.AddOrUpdatePageContent(pageContent);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating page content, pageContentId", pageContent.Id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("list/")]
        public IActionResult Put([FromBody] PageContent[] pageContents)
        {
            try
            {
                if (pageContents == null || pageContents.Length == 0)
                    return BadRequest();

                _contentManager.AddOrUpdatePageContents(new List<PageContent>(pageContents));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating page contents"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("permission/")]
        public IActionResult PutPermissions([FromBody] PageContent pageContent)
        {
            try
            {
                if (pageContent == null || pageContent.ContentPermissions == null || pageContent.ContentPermissions.Count() == 0)
                    return BadRequest();

                _contentManager.UpdateContentPermission(pageContent);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating page content permissions"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("restore/{id}")]
        public IActionResult PutPageContent(Guid id)
        {
            try
            {
                var result = _contentManager.RestorePageContent(id);
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating page content permissions"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var deleteResult = _contentManager.RemovePageContent(id);
                if (deleteResult)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while removing page content"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeletePageContent(Guid id)
        {
            try
            {
                bool result = _contentManager.DeletePageContent(id);
                if (result)
                    return Ok();

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while deleting page content"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}


