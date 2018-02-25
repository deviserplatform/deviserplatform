using Autofac;
using Deviser.Core.Library.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.Controllers.Api
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly IFileManagement _fileManagement;

        public FileController(ILifetimeScope container)
        {            
            _logger = container.Resolve<ILogger<FileController>>();
            _fileManagement = container.Resolve<IFileManagement>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _fileManagement.GetFilesAndFolders();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting all files and folders"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
