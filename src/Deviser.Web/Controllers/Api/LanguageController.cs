using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Multilingual;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Deviser.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class LanguageController : Controller
    {
        //Logger
        private readonly ILogger<LanguageController> _logger;
        private readonly ILanguageManager _languageManager;

        public LanguageController(ILogger<LanguageController> logger, 
            ILanguageManager languageManager)
        {
            _logger = logger;
            _languageManager = languageManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _languageManager.GetAllLanguages(true);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting languages"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("site")]
        public IActionResult GetSiteLanguage()
        {
            try
            {
                var result = _languageManager.GetLanguages();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting languages"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = _languageManager.GetLanguage(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting page language: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Language language)
        {
            try
            {
                var result = _languageManager.CreateLanguage(language);
                if (result != null)
                    return Ok(result);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating a language", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]Language language)
        {
            try
            {
                var result = _languageManager.UpdateLanguage(language);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating language, cultureCode: ", language.CultureCode), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var language = _languageManager.GetLanguage(id);
                if (language != null)
                {
                    language.IsActive = false;
                    var result = _languageManager.UpdateLanguage(language);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while deleting language"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
