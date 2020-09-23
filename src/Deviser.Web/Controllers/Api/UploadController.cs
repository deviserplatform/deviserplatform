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
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common.Security;
using Microsoft.AspNetCore.Authorization;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    [PermissionAuthorize("PAGE", "EDIT")]
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IImageOptimizer _imageOptimizer;
        private readonly string _localImageUploadPath;
        private readonly string _localDocumentUploadPath;

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
                _localDocumentUploadPath = Path.Combine(siteAssetPath, Globals.DocumentsFolder);
                if (!Directory.Exists(_localImageUploadPath))
                {
                    Directory.CreateDirectory(_localImageUploadPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting images"), ex);
            }
        }

        [HttpGet]
        [Route("images")]
        [AllowAnonymous]
        public IActionResult GetImages(string searchTerm)
        {
            try
            {
                var dir = new DirectoryInfo(_localImageUploadPath);
                var fileList = new List<FileItem>();
                foreach (var file in dir.GetFiles())
                {
                    if (file == null || file.Name.Contains(Globals.OriginalFileSuffix)) continue;
                    var path = Globals.SiteAssetsPath.Replace("~", "") + Globals.ImagesFolder + "/" + file.Name;
                    fileList.Add(new FileItem
                    {
                        Name = file.Name,
                        Path = path,
                        Extension = file.Extension,
                        Type = FileItemType.File
                    });
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    fileList = fileList.Where(file => file.Name.Contains(searchTerm) || file.Path.Contains(searchTerm))
                        .ToList();
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
            var fileList = new List<string>();
            try
            {
                if (HttpContext.Request.Form.Files == null || HttpContext.Request.Form.Files.Count <= 0)
                    return BadRequest();
                foreach (var file in HttpContext.Request.Form.Files)
                {
                    if (file.Length <= 0) continue;

                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                    var filePath = Path.Combine(_localImageUploadPath, fileName);
                    var originalPath = filePath + Globals.OriginalFileSuffix;
                    await using (var memoryStream = new MemoryStream())
                    {
                        var sourImage = memoryStream.ToArray();
                        await file.CopyToAsync(memoryStream);
                        SaveFile(sourImage, originalPath);
                        sourImage = memoryStream.ToArray();
                        var optimizedImage = _imageOptimizer.OptimizeImage(sourImage);
                        SaveFile(optimizedImage, filePath);
                    }

                    fileList.Add(Globals.SiteAssetsPath.Replace("~", "") + Globals.ImagesFolder + "/" + fileName);
                }
                return Ok(fileList);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while uploading images"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("documents")]
        [AllowAnonymous]
        public IActionResult Get(string searchTerm)
        {
            try
            {
                var dir = new DirectoryInfo(_localDocumentUploadPath);
                var fileList = new List<FileItem>();
                foreach (var file in dir.GetFiles())
                {
                    if (file == null || file.Name.Contains(Globals.OriginalFileSuffix)) continue;
                    var path = Globals.SiteAssetsPath.Replace("~", "") + Globals.DocumentsFolder + "/" + file.Name;
                    fileList.Add(new FileItem
                    {
                        Name = file.Name,
                        Path = path,
                        Extension = file.Extension,
                        Type = FileItemType.File
                    });
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    fileList = fileList.Where(file => file.Name.Contains(searchTerm) || file.Path.Contains(searchTerm))
                        .ToList();
                }

                if (fileList.Count > 0)
                    return Ok(fileList);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting documents"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("documents")]
        public async Task<IActionResult> UploadDocuments()
        {
            var fileList = new List<string>();
            try
            {
                if (HttpContext.Request.Form.Files == null || HttpContext.Request.Form.Files.Count <= 0)
                    return BadRequest();
                foreach (var file in HttpContext.Request.Form.Files)
                {
                    if (file.Length <= 0) continue;

                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                    var filePath = Path.Combine(_localDocumentUploadPath, fileName);
                    var originalPath = filePath + Globals.OriginalFileSuffix;
                    await using (var memoryStream = new MemoryStream())
                    {
                        var sourceDocument = memoryStream.ToArray();
                        //await file.CopyToAsync(memoryStream);
                        //SaveFile(sourImage, originalPath);
                        //sourImage = memoryStream.ToArray();
                        //var optimizedImage = _imageOptimizer.OptimizeImage(sourImage);
                        SaveFile(sourceDocument, filePath);
                    }

                    fileList.Add(Globals.SiteAssetsPath.Replace("~", "") + Globals.DocumentsFolder + "/" + fileName);
                }
                return Ok(fileList);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while uploading documents"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task SaveFile(Stream stream, string path)
        {
            await using var fileStream = System.IO.File.Create(path);
            await stream.CopyToAsync(fileStream);
        }

        private void SaveFile(byte[] arrBytes, string path)
        {
            System.IO.File.WriteAllBytes(path, arrBytes);
        }
    }
}
