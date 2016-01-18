using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Deviser.Core.Library.DomainTypes;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.IO;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Http;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class ContentTypeController : Controller
    {
        private readonly ILogger<ContentTypeController> logger;

        private JToken contentTypes;

        public ContentTypeController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<ContentTypeController>>();
            IApplicationEnvironment appEnv = container.Resolve<IApplicationEnvironment>();
            try
            {
                var contentTypesfilePath = Path.Combine(appEnv.ApplicationBasePath, "contentTypes.json");                
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
                if (contentTypes != null)
                    return Ok(contentTypes);
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting content types"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
