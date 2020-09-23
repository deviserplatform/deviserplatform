using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
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
    public class ContentTypeController : Controller
    {
        private readonly ILogger<ContentTypeController> _logger;
        private readonly IContentTypeRepository _contentTypeRepository;

        public ContentTypeController(ILogger<ContentTypeController> logger,
        IContentTypeRepository contentTypeRepository)
        {
            _logger = logger;
            _contentTypeRepository = contentTypeRepository;
        }

        [HttpGet]
        [AllowAnonymous]
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
