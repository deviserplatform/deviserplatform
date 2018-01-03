using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Layouts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class LayoutController : Controller
    {
        private readonly ILogger<LayoutController> logger;

        ILayoutProvider layoutProvider;
        ILayoutManager layoutManager;

        public LayoutController(ILifetimeScope container)
        {
            layoutManager = container.Resolve<ILayoutManager>();
            layoutProvider = container.Resolve<ILayoutProvider>();
            logger = container.Resolve<ILogger<LayoutController>>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = layoutManager.GetPageLayouts();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all page layouts"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("deletedlist/")]
        public IActionResult GetDeletedLayouts()
        {
            try
            {
                var result = layoutManager.GetDeletedLayouts();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting deleted layouts"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = layoutManager.GetPageLayout(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting page layout: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]PageLayout layout)
        {
            try
            {
                var result = layoutManager.CreatePageLayout(layout);
                if (result != null)
                    return Ok(result);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a page layout, LayoutName: "), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]PageLayout layout)
        {
            try
            {
                var result = layoutManager.UpdatePageLayout(layout);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating page layout, LayoutName: ", layout.Name), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Restore([FromBody]Layout layout)
        {
            try
            {
                var result = layoutManager.UpdateLayout(layout);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating page layout, LayoutName: ", layout.Name), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var layout = layoutProvider.GetLayout(id);
                
                if (layout != null)
                {
                    if (layout.IsDeleted)
                    {
                         bool result = layoutProvider.DeleteLayout(id);
                        if (result)
                            return Ok();
                    }
                    else
                    {
                        layout.IsDeleted = true;
                        var result = layoutProvider.UpdateLayout(layout);
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
                logger.LogError(string.Format("Error occured while deleting the layout"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
