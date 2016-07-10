using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class ContentTypeController : Controller
    {
        private readonly ILogger<ContentTypeController> logger;

        private IContentTypeProvider contentTypeProvider;
        private JToken contentTypes;

        public ContentTypeController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<ContentTypeController>>();
            contentTypeProvider = container.Resolve<IContentTypeProvider>();
            IHostingEnvironment hostingEnvironment = container.Resolve<IHostingEnvironment>();
            try
            {
                var contentTypesfilePath = Path.Combine(hostingEnvironment.ContentRootPath, "appcontentcofig.json");                
                JObject contentConfig = JObject.Parse(System.IO.File.ReadAllText(contentTypesfilePath));
                contentTypes = (JArray)contentConfig.SelectToken("contentTypes");
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult Get()
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
    }
}
