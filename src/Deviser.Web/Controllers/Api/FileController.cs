using Deviser.Core.Library.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Deviser.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly IFileManagement _fileManagement;

        public FileController(ILogger<FileController> logger,
            IFileManagement fileManagement)
        {            
            _logger = logger;
            _fileManagement = fileManagement;
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
