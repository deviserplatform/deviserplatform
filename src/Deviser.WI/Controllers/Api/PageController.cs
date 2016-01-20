﻿using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Deviser.Core.Library.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
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
                var pages = pageProvider.GetPageTree();
                if (pages != null)
                    return Ok(pages);

                return HttpNotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all pages"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]        
        public IActionResult Get(int id)
        {
            try
            {
                var result = pageProvider.GetPage(id);
                if (result != null)
                    return Ok(result);
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting a page, PageId: {0}", id), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post(Page page)
        {
            try
            {
                var result = pageProvider.CreatePage(page);
                if (result != null)
                    return Ok(result);
                return HttpBadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a page, PageId: ", page.Id), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
               
        [HttpPut]
        public IActionResult Put(Page page)
        {
            try
            {
                if (page != null)
                {
                    var result = navigation.UpdatePageTree(page);
                    if (result != null)
                        return Ok(result);
                }
                return HttpBadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating page tree");
                logger.LogError(errorMessage, ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("api/page/{id:int}")]
        [HttpPut]
        public IActionResult PutPage(int id, [FromBody] Page page)
        {
            try
            {
                if (page != null)
                {
                    var result = navigation.UpdateSinglePage(page);
                    if (result != null)
                        return Ok(result);
                }
                return HttpBadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating a page, PageId: ", page.Id);
                logger.LogError(errorMessage, ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("api/page/{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    bool result = navigation.DeletePage(id);
                    if (result)
                        return Ok(result);
                }
                return HttpBadRequest("Invalid page");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while deleting page, PageId: ", id);
                logger.LogError(errorMessage, ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
