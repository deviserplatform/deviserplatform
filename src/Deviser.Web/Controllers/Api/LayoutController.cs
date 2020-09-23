using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Layouts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Deviser.Core.Common.Security;
using Microsoft.AspNetCore.Authorization;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    [PermissionAuthorize("PAGE", "EDIT")]
    public class LayoutController : Controller
    {
        private readonly ILogger<LayoutController> _logger;
        private readonly ILayoutRepository _layoutRepository;
        private readonly ILayoutManager _layoutManager;

        public LayoutController(ILogger<LayoutController> logger,
            ILayoutRepository layoutRepository,
            ILayoutManager layoutManager)
        {
            _layoutManager = layoutManager;
            _layoutRepository = layoutRepository;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                var result = _layoutManager.GetPageLayouts();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting all page layouts"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("deletedlist/")]
        public IActionResult GetDeletedLayouts()
        {
            try
            {
                var result = _layoutManager.GetDeletedLayouts();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting deleted layouts"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = _layoutManager.GetPageLayout(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting page layout: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]PageLayout layout)
        {
            try
            {
                var result = _layoutManager.CreatePageLayout(layout);
                if (result != null)
                    return Ok(result);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a page layout, LayoutName: "), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]PageLayout layout)
        {
            try
            {
                var result = _layoutManager.UpdatePageLayout(layout);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating page layout, LayoutName: ", layout.Name), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Restore([FromBody]Layout layout)
        {
            try
            {
                var result = _layoutManager.UpdateLayout(layout);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating page layout, LayoutName: ", layout.Name), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var layout = _layoutRepository.GetLayout(id);
                
                if (layout != null)
                {
                    if (!layout.IsActive)
                    {
                         var result = _layoutRepository.DeleteLayout(id);
                        if (result)
                            return Ok();
                    }
                    else
                    {
                        layout.IsActive = false;
                        var result = _layoutRepository.UpdateLayout(layout);
                        if (result != null)
                        {
                            return Ok(result);
                        }
                    }                                       
                    
                }   
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while deleting the layout"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
