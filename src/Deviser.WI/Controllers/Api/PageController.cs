using Autofac;
using AutoMapper;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Page = Deviser.Core.Data.Entities.Page;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class PageController : Controller
    {
        //Logger
        private readonly ILogger<PageController> logger;

        IPageProvider pageProvider;
        INavigation navigation;

        public PageController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<PageController>>();
            pageProvider = container.Resolve<IPageProvider>();
            navigation = container.Resolve<INavigation>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var dbResult = navigation.GetPageTree();
                var result = ConvertToDomainType(dbResult);
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all pages"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("list/")]
        public IActionResult GetPages()
        {
            try
            {
                var dbResult = pageProvider.GetPages();
                var result = Mapper.Map<List<Deviser.Core.Common.DomainTypes.Page>>(dbResult);
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all pages"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var dbResult = navigation.GetPage(id);
                var result = ConvertToDomainType(dbResult);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting a page, PageId: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Deviser.Core.Common.DomainTypes.Page page)
        {
            try
            {
                var dbResult = navigation.CreatePage(ConvertToDbType(page));
                var result = ConvertToDomainType(dbResult);
                if (result != null)
                    return Ok(result);
                return BadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a page, PageId: ", page.Id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Deviser.Core.Common.DomainTypes.Page page)
        {
            try
            {
                if (page != null)
                {
                    var dbResult = navigation.UpdatePageTree(ConvertToDbType(page));
                    var result = ConvertToDomainType(dbResult);
                    if (result != null)
                        return Ok(result);
                }
                return BadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating page tree");
                logger.LogError(errorMessage, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutPage(int id, [FromBody] Deviser.Core.Common.DomainTypes.Page page)
        {
            try
            {
                if (page != null)
                {
                    var dbResult = navigation.UpdateSinglePage(ConvertToDbType(page));
                    var result = ConvertToDomainType(dbResult);
                    if (result != null)
                        return Ok(result);
                }
                return BadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating a page, PageId: ", page.Id);
                logger.LogError(errorMessage, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    bool result = navigation.DeletePage(id);
                    if (result)
                        return Ok(result);
                }
                return BadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while deleting page, PageId: ", id);
                logger.LogError(errorMessage, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private Deviser.Core.Common.DomainTypes.Page ConvertToDomainType(Page role)
        {
            return Mapper.Map<Deviser.Core.Common.DomainTypes.Page>(role);
        }

        private Page ConvertToDbType(Deviser.Core.Common.DomainTypes.Page role)
        {
            return Mapper.Map<Page>(role);
        }
    }
}
