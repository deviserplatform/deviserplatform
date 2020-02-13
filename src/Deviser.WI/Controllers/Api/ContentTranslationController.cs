using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Deviser.WI.Controllers.Api
{
    [Route("api/[controller]")]
    public class ContentTranslationController : Controller
    {
        private readonly ILogger<ContentTranslationController> _logger;
        private readonly IPageContentRepository _pageContentRepository;
        public ContentTranslationController(ILogger<ContentTranslationController> logger,
            IPageContentRepository pageContentRepository)
        {
            _logger = logger;
            _pageContentRepository = pageContentRepository;
        }

        [HttpGet("{cultureCode}/{contentId}")]
        public IActionResult Get(string cultureCode, Guid contentId)
        {
            try
            {                
                var result = _pageContentRepository.GetTranslations(contentId, cultureCode);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting page content translation, contentId: {0}, cultureCode: {1}", contentId, cultureCode), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] PageContentTranslation contentTranslation)
        {
            try
            {                
                var result = _pageContentRepository.CreateTranslation(contentTranslation);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating/updating a page content translation"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] PageContentTranslation contentTranslation)
        {
            try
            {                
                var result = _pageContentRepository.UpdateTranslation(contentTranslation);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating/updating a page content translation"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var translation = _pageContentRepository.GetTranslation(id);
                if (translation != null)
                {
                    translation.IsDeleted = true;
                    _pageContentRepository.UpdateTranslation(translation);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while deleting page content translation"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
