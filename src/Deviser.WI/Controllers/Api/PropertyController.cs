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
using AutoMapper;
using Property = Deviser.Core.Common.DomainTypes.Property;

namespace Deviser.WI.Controllers.Api
{
    [Route("api/[controller]")]
    public class PropertyController : Controller
    {
        private readonly ILogger<PropertyController> logger;
        IPropertyProvider propertyProvider;

        public PropertyController(ILifetimeScope container)
        {            
            logger = container.Resolve<ILogger<PropertyController>>();
            propertyProvider = container.Resolve<IPropertyProvider>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var dbResult = propertyProvider.GetProperties();
                var result = Mapper.Map<List<Deviser.Core.Common.DomainTypes.Property>>(dbResult);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting properties"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var dbResult = propertyProvider.GetProperty(id);
                var result = Mapper.Map<Deviser.Core.Common.DomainTypes.Property>(dbResult);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting property: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Property property)
        {
            try
            {
                var dbResult = propertyProvider.CreateProperty(property);
                var result = Mapper.Map<Deviser.Core.Common.DomainTypes.Property>(dbResult);
                if (result != null)
                    return Ok(result);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a property, propertyName: {0}", property.Name), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]Property property)
        {
            try
            {
                var dbResult = propertyProvider.UpdateProperty(property);
                var result = Mapper.Map<Deviser.Core.Common.DomainTypes.Property>(dbResult);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating property, LayoutName: ", property.Name), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}