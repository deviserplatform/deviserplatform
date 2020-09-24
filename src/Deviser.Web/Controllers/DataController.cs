using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Core.Data;
using Deviser.Core.Data.DataMigration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deviser.Web.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class DataController : Controller
    {
        private readonly DeviserDbContext _dbContext;
        private readonly ILogger<DataController> _logger;

        public DataController(DeviserDbContext dbContext,
            ILogger<DataController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public IActionResult Export()
        {
            try
            {
                var entityManager = new EntityManager(_dbContext);
                var dataAsJson = entityManager.ExportDataAsJson();

                if (!string.IsNullOrEmpty(dataAsJson))
                {
                    var dataBytes = Encoding.UTF8.GetBytes(dataAsJson);
                    return File(dataBytes, "application/json","export.json");
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while exporting the site", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
