using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.Controllers.Api
{
    [Route("api/[controller]")]
    public class ContentTranslationController : Controller
    {
        private readonly ILogger<ContentTranslationController> logger;
        IPageContentProvider pageContentProvider;
        public ContentTranslationController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<ContentTranslationController>>();
            pageContentProvider = container.Resolve<IPageContentProvider>();
        }

        [HttpGet("{cultureCode}/{contentId}")]
        public IActionResult Get(string cultureCode, Guid contentId)
        {
            try
            {
                var result = pageContentProvider.GetTranslations(contentId, cultureCode);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting page content translation, contentId: {0}, cultureCode: {1}", contentId, cultureCode), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] PageContentTranslation contentTranslation)
        {
            try
            {
                var result = pageContentProvider.CreateTranslation(contentTranslation);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating/updating a page content translation"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] PageContentTranslation contentTranslation)
        {
            try
            {
                var result = pageContentProvider.UpdateTranslation(contentTranslation);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating/updating a page content translation"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var translation = pageContentProvider.GetTranslation(id);
                if (translation != null)
                {
                    translation.IsDeleted = true;
                    pageContentProvider.UpdateTranslation(translation);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while deleting page content translation"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
