using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
using Deviser.Core.Library.Layouts;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
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
                if (result!= null)
                    return Ok(result);
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all page layouts"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost]
        public IActionResult Post(PageLayout layout)
        {
            try
            {
                var result = layoutManager.CreatePageLayout(layout);
                if (result != null)
                    return Ok(result);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a page layout, LayoutName: ", layout.Name), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put(PageLayout layout)
        {
            try
            {               
                var result = layoutManager.UpdatePageLayout(layout);
                if (result != null)
                    return Ok(result);
                return HttpBadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating page layout, LayoutName: ", layout.Name), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var layout = layoutProvider.GetLayout(id);
                if (layout != null)
                {
                    layout.IsDeleted = true;
                    var result = layoutProvider.UpdateLayout(layout);                    
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }   
                return HttpBadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while deleting page layout"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
