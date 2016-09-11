using Autofac;
using AutoMapper;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.Multilingual;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var dbResult = languageManager.GetAllLanguages(true);
                var result = Mapper.Map<List<Deviser.Core.Common.DomainTypes.Language>>(dbResult);
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
                var dbResult = languageManager.GetLanguages()
                    .ToList();
                var result = Mapper.Map<List<Deviser.Core.Common.DomainTypes.Language>>(dbResult);
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
                var dbResult = languageManager.GetLanguage(id);
                var result = ConvertToDomainType(dbResult);
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
                var dbResult = languageManager.CreateLanguage(language);
                var result = ConvertToDomainType(dbResult);
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
                var dbResult = languageManager.UpdateLanguage(language);
                var result = ConvertToDomainType(dbResult);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating language, cultureCode: ", language.CultureCode), ex);
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
                    var dbResult = languageManager.UpdateLanguage(language);
                    var result = ConvertToDomainType(dbResult);
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

        private Deviser.Core.Common.DomainTypes.Language ConvertToDomainType(Language language)
        {
            return Mapper.Map<Deviser.Core.Common.DomainTypes.Language>(language);
        }

        private Language ConvertToDbType(Deviser.Core.Common.DomainTypes.Language language)
        {
            return Mapper.Map<Language>(language);
        }
    }
}
