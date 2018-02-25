using Autofac;
using AutoMapper;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;


namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class ContentTypeController : Controller
    {
        private readonly ILogger<ContentTypeController> _logger;
        private readonly IContentTypeRepository _contentTypeRepository;

        public ContentTypeController(ILifetimeScope container)
        {
            _logger = container.Resolve<ILogger<ContentTypeController>>();
            _contentTypeRepository = container.Resolve<IContentTypeRepository>();
            //IHostingEnvironment hostingEnvironment = container.Resolve<IHostingEnvironment>();
        }

        [HttpGet]
        public IActionResult GetContentTypes()
        {
            try
            {
                var result = _contentTypeRepository.GetContentTypes();

                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting content types"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult CreateContentType([FromBody] ContentType contentType)
        {
            try
            {
                if (contentType == null || string.IsNullOrEmpty(contentType.Name))
                    return BadRequest("Invalid parameter");

                if (_contentTypeRepository.GetContentType(contentType.Name) != null)
                    return BadRequest("Content type already exist");

                var result = _contentTypeRepository.CreateContentType(contentType);

                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating layout type"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult UpdateContentType([FromBody] ContentType contentType)
        {
            try
            {
                var result = _contentTypeRepository.UpdateContentType(contentType);
                //TODO: Update properties Add/Remove/Update
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating content type"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }        
    }
}
