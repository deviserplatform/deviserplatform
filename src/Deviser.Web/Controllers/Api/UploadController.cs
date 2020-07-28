﻿using Deviser.Core.Common;
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
        public IActionResult Get()
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
