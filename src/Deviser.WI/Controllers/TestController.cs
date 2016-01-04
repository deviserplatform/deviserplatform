using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Deviser.WI.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        ILayoutProvider layoutProvider;
        public TestController(ILayoutProvider layoutProvider)
        {
            this.layoutProvider = layoutProvider;
        }

        [HttpGet]
        public IActionResult GetLayouts()
        {
            try
            {
                var result = layoutProvider.GetLayouts();
                return Ok(result);
            }
            catch(Exception ex)
            {
                var errorResult = new ObjectResult(ex);
                errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                return errorResult;
            }
        }
    }
}
