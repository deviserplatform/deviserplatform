using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class LayoutTypeController : Controller
    {
        private readonly ILogger<LayoutTypeController> _logger;
        private readonly ILayoutTypeRepository _layoutTypeRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private const string StrContentTypes = @"{ 'rootLayoutTypes': [ '9341f92e-83d8-4afe-ad4a-a95deeda9ae3', '5a0a5884-da84-4922-a02f-5828b55d5c92']}";
        //private JToken contentTypes;
        private List<string> _rootAllowedTypes;
        public LayoutTypeController(ILogger<LayoutTypeController> logger,
            ILayoutTypeRepository layoutTypeRepository,
            IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _layoutTypeRepository = layoutTypeRepository;
            _hostingEnvironment = hostingEnvironment;
            try
            {
                //var contentTypesfilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "appcontentcofig.json");                
                
                var contentConfig = JObject.Parse(StrContentTypes);
                //contentTypes = (JArray)contentConfig.SelectToken("layoutTypes");
                //TODO: Move it to SiteSettings
                _rootAllowedTypes = contentConfig.SelectToken("rootLayoutTypes").ToObject<List<string>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while initializing LayoutTypeController"), ex);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _layoutTypeRepository.GetLayoutTypes();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting layout types"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("rootallowedtypes")]
        public IActionResult GetRootAllowedTypes()
        {
            try
            {
                if (_rootAllowedTypes != null)
                    return Ok(_rootAllowedTypes);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting root allowed types"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult CreateLayoutType([FromBody]LayoutType layoutType)
        {
            try
            {
                if (layoutType == null || string.IsNullOrEmpty(layoutType.Name))
                    return BadRequest("Invalid parameter");

                if(_layoutTypeRepository.GetLayoutType(layoutType.Name)!=null)
                    return BadRequest("Layout type already exist");
                
                var result = _layoutTypeRepository.CreateLayoutType(layoutType);
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
        public IActionResult UpdateLayoutType([FromBody]LayoutType layoutType)
        {
            try
            {   
                var result = _layoutTypeRepository.UpdateLayoutType(layoutType);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating layout type"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
