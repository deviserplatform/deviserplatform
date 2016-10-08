using Autofac;
using AutoMapper;
using Deviser.Core.Data.DataProviders;
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
        private readonly ILogger<ContentTypeController> logger;

        private IContentTypeProvider contentTypeProvider;

        public ContentTypeController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<ContentTypeController>>();
            contentTypeProvider = container.Resolve<IContentTypeProvider>();
            IHostingEnvironment hostingEnvironment = container.Resolve<IHostingEnvironment>();
        }

        [HttpGet]
        public IActionResult GetContentTypes()
        {
            try
            {
                //if (contentTypes != null)
                //    return Ok(contentTypes);
                //return NotFound();

                var contentTypes = contentTypeProvider.GetContentTypes();

                var result = Mapper.Map<List<Deviser.Core.Common.DomainTypes.ContentType>>(contentTypes);

                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting content types"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("datatype/")]
        public IActionResult GetContentDataTypes()
        {
            try
            {
                var dbResult = contentTypeProvider.GetContentDataTypes();
                var result = Mapper.Map<List<Deviser.Core.Common.DomainTypes.ContentDataType>>(dbResult);

                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting content types"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult CreateContentType([FromBody]Deviser.Core.Common.DomainTypes.ContentType contentType)
        {
            try
            {
                if (contentType == null || string.IsNullOrEmpty(contentType.Name))
                    return BadRequest("Invalid parameter");

                if (contentTypeProvider.GetContentType(contentType.Name) != null)
                    return BadRequest("Content type already exist");

                var dbResult = contentTypeProvider.CreateContentType(ConvertToDbType(contentType));
                var result = Mapper.Map<Deviser.Core.Common.DomainTypes.ContentType>(dbResult);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating layout type"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult UpdateContentType([FromBody]Deviser.Core.Common.DomainTypes.ContentType contentType)
        {
            try
            {
                var dbResult = contentTypeProvider.UpdateContentType(ConvertToDbType(contentType));
                //TODO: Update properties Add/Remove/Update
                var result = Mapper.Map<Deviser.Core.Common.DomainTypes.ContentType>(dbResult);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating content type"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private Deviser.Core.Common.DomainTypes.ContentType ConvertToDomainType(ContentType role)
        {
            return Mapper.Map<Deviser.Core.Common.DomainTypes.ContentType>(role);
        }

        private ContentType ConvertToDbType(Deviser.Core.Common.DomainTypes.ContentType role)
        {
            return Mapper.Map<ContentType>(role);
        }
    }
}
