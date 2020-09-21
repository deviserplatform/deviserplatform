using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Deviser.Core.Library.IO
{
    public class FileManagement : IFileManagement
    {
        private readonly ILogger<FileManagement> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public FileManagement(ILogger<FileManagement> logger
            , IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostEnvironment;
        }

        public List<FileItem> GetFilesAndFolders()
        {
            var assetsPath = Path.Combine(_hostingEnvironment.ContentRootPath + "\\wwwroot\\assets");
            var folders = Directory.GetDirectories(assetsPath).Select(folderPath => new FileItem
            {
                Name = folderPath.Replace(assetsPath, ""),
                Type = FileItemType.Folder
            }).ToList();

            var files = Directory.GetFiles(assetsPath).Select(folderPath => new FileItem
            {
                Name = folderPath.Replace(assetsPath, ""),
                Type = FileItemType.File
            }).ToList();

            folders.AddRange(files);
            return folders;
        }
    }
}
