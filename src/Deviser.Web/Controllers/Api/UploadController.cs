using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Media;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IImageOptimizer _imageOptimizer;
        private readonly string _localImageUploadPath;

        public UploadController(ILogger<UploadController> logger,
        IImageOptimizer imageOptimizer,
        IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _imageOptimizer = imageOptimizer;
            try
            {
                var siteAssetPath = Path.Combine(hostEnvironment.WebRootPath, Globals.SiteAssetsPath.Replace("~/", "").Replace("/", @"\"));
                _localImageUploadPath = Path.Combine(siteAssetPath, Globals.ImagesFolder);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting images"), ex);
            }
        }

        [HttpGet]
        [Route("images")]
        public IActionResult Get()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_localImageUploadPath);
                List<FileItem> fileList = new List<FileItem>();
                string path = "";
                foreach (var file in dir.GetFiles())
                {
                    path = "";
                    if (file != null && !file.Name.Contains(Globals.OriginalFileSuffix))
                    {
                        path = Globals.SiteAssetsPath.Replace("~", "") + Globals.ImagesFolder + "/" + file.Name;
                        fileList.Add(new FileItem
                        {
                            Name = file.Name,
                            Path = path,
                            Extension = file.Extension,
                            Type = FileItemType.File
                        });
                    }
                }

                if (fileList.Count > 0)
                    return Ok(fileList);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting images"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("images")]
        public async Task<IActionResult> UploadImage()
        {
            List<string> fileList = new List<string>();
            try
            {
                if (HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files.Count > 0)
                {
                    foreach (IFormFile file in HttpContext.Request.Form.Files)
                    {
                        if (file.Length > 0)
                        {
                            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                            string filePath = Path.Combine(_localImageUploadPath, fileName);
                            string originalfilePath = filePath + Globals.OriginalFileSuffix;
                            using (var memoryStream = new MemoryStream())
                            {
                                var sourImage = memoryStream.ToArray();
                                await file.CopyToAsync(memoryStream);
                                SaveFile(sourImage, originalfilePath);
                                sourImage = memoryStream.ToArray();
                                var optimizedImage = _imageOptimizer.OptimizeImage(sourImage);
                                SaveFile(optimizedImage, filePath);
                            }

                            fileList.Add(Globals.SiteAssetsPath.Replace("~", "") + Globals.ImagesFolder + "/" + fileName);
                        }
                    }
                    return Ok(fileList);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while uploading images"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task SaveFile(Stream stream, string path)
        {
            using (var fileStream = System.IO.File.Create(path))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        private void SaveFile(byte[] arrBytes, string path)
        {
            System.IO.File.WriteAllBytes(path, arrBytes);
        }
    }
}
