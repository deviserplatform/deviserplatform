using Autofac;
using AutoMapper;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
                var dbResult = pageContentProvider.GetTranslations(contentId, cultureCode);
                var result = Mapper.Map<List<Deviser.Core.Common.DomainTypes.PageContentTranslation>>(dbResult);
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
        public IActionResult Post([FromBody] Deviser.Core.Common.DomainTypes.PageContentTranslation contentTranslation)
        {
            try
            {
                var dbResult = pageContentProvider.CreateTranslation(ConvertToDbType(contentTranslation));
                var result = ConvertToDomainType(dbResult);
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
        public IActionResult Put([FromBody] Deviser.Core.Common.DomainTypes.PageContentTranslation contentTranslation)
        {
            try
            {
                var dbResult = pageContentProvider.UpdateTranslation(ConvertToDbType(contentTranslation));
                var result = ConvertToDomainType(dbResult);
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

        private Deviser.Core.Common.DomainTypes.PageContentTranslation ConvertToDomainType(PageContentTranslation role)
        {
            return Mapper.Map<Deviser.Core.Common.DomainTypes.PageContentTranslation>(role);
        }

        private PageContentTranslation ConvertToDbType(Deviser.Core.Common.DomainTypes.PageContentTranslation role)
        {
            return Mapper.Map<PageContentTranslation>(role);
        }
    }
}
