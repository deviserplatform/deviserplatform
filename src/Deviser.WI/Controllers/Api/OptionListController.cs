using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deviser.Core.Data.DataProviders;
using Microsoft.Extensions.Logging;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Deviser.WI.Controllers.Api
{
    [Route("api/[controller]")]
    public class OptionListController : Controller
    {
        private readonly ILogger<OptionListController> logger;

        private IOptionListProvider optionListProvider;
        public OptionListController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<OptionListController>>();
            optionListProvider = container.Resolve<IOptionListProvider>();
        }

        [HttpGet]
        public IActionResult GetOptionList()
        {
            try
            {
                //if (contentTypes != null)
                //    return Ok(contentTypes);
                //return NotFound();

                var dbOptionList = optionListProvider.GetOptionLists();
                var result = Mapper.Map<List<Deviser.Core.Common.DomainTypes.OptionList>>(dbOptionList);

                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting option lists"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult CreateOptionList([FromBody]Deviser.Core.Common.DomainTypes.OptionList optionList)
        {
            try
            {
                if (optionList == null || string.IsNullOrEmpty(optionList.Name))
                    return BadRequest("Invalid parameter");

                if (optionListProvider.GetOptionList(optionList.Name) != null)
                    return BadRequest("Option list already exist");

                var dbResult = optionListProvider.CreateOptionList(optionList);
                var result = Mapper.Map<Deviser.Core.Common.DomainTypes.OptionList>(dbResult);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating option list"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult UpdateOptionList([FromBody]Deviser.Core.Common.DomainTypes.OptionList contentType)
        {
            try
            {
                var dbResult = optionListProvider.UpdateOptionList(contentType);
                var result = Mapper.Map<Deviser.Core.Common.DomainTypes.OptionList>(dbResult);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating option list"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
