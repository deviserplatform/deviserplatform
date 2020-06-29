using Deviser.Core.Data.Repositories;
using Deviser.Core.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class PageController : Controller
    {
        //Logger
        private readonly ILogger<PageController> _logger;
        private readonly IPageRepository _pageRepository;
        private readonly INavigation _navigation;

        public PageController(ILogger<PageController> logger,
            IPageRepository pageRepository,
            INavigation navigation)
        {
            _logger = logger;
            _pageRepository = pageRepository;
            _navigation = navigation;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {   
                var result = _navigation.GetPageTree();
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting all pages"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("list/")]
        public IActionResult GetPages()
        {
            try
            {
                var result = _pageRepository.GetPagesFlat();
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting all pages"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet]
        [Route("deletedlist/")]
        public IActionResult GetDeletedPages()
        {
            try
            {
                var result = _pageRepository.GetDeletedPages();
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting all pages"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {   
                var result = _navigation.GetPageAndDependencies(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting a page, PageId: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Deviser.Core.Common.DomainTypes.Page page)
        {
            try
            {
                var result = _navigation.CreatePage(page);               
                if (result != null)
                        return Ok(result);
                
                return BadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a page, PageId: ", page.Id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("draft/{id}")]
        public IActionResult DraftPage(Guid id)
        {
            try
            {
                bool result = _pageRepository.DraftPage(id);
                if (result)
                    return Ok();

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while drafting the page", ex));
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("publish/{id}")]
        public IActionResult PublishPage(Guid id)
        {
            try
            {
                bool result = _pageRepository.PublishPage(id);
                if (result)
                    return Ok();

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while publishing the page", ex));
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
                    var result = _navigation.UpdatePageTree(page);
                    if (result != null)
                        return Ok(result);
                }
                return BadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating page tree");
                _logger.LogError(errorMessage, ex);
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
                    var result = _navigation.UpdatePageAndChildren(page);
                    if (result != null)
                        return Ok(result);
                }
                return BadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating a page, PageId: ", page.Id);
                _logger.LogError(errorMessage, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("restore/{id}")]
        public IActionResult PutPage(Guid id)
        {
            try
            {                
                var result = _pageRepository.RestorePage(id);
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while restoring a page", ex));
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
                    bool result = _navigation.DeletePage(id);
                    if (result)
                        return Ok(result);
                }
                return BadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while deleting page, PageId: ", id);
                _logger.LogError(errorMessage, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeletePage(Guid id)
        {
            try
            {
                bool result = _pageRepository.DeletePage(id);
                if (result)
                    return Ok();

                return BadRequest();
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("Error occured while deleting the page", ex));
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
      
    }
}
