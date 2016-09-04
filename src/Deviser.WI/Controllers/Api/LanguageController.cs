using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Multilingual;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Deviser.WI.Controllers.Api
{
    [Route("api/[controller]")]
    public class LanguageController : Controller
    {
        //Logger
        private readonly ILogger<LanguageController> logger;        
        private ILanguageManager languageManager;

        public LanguageController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<LanguageController>>();            
            languageManager = container.Resolve<ILanguageManager>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                //var result = CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures).OrderBy(c => c.NativeName)
                //           .Select(c => new Language
                //           {
                //               EnglishName = c.EnglishName,
                //               NativeName = c.NativeName,
                //               CultureCode = c.Name,
                //               FallbackCulture = Globals.FallbackLanguage
                //           })
                //           .ToList();

                var result = languageManager.GetAllLanguages(true);

                

                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting languages"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("site")]
        public IActionResult GetSiteLanguage()
        {
            try
            {
                var result = languageManager.GetLanguages()
                    .ToList();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting languages"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = languageManager.GetLanguage(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting page language: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Language language)
        {
            try
            {
                var result = languageManager.CreateLanguage(language);
                if (result != null)
                    return Ok(result);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while creating a language", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]Language language)
        {
            try
            {
                var result = languageManager.UpdateLanguage(language);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating language, LayoutName: ", language.CultureCode), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var language = languageManager.GetLanguage(id);
                if (language != null)
                {
                    language.IsActive = false;
                    var result = languageManager.UpdateLanguage(language);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while deleting language"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
