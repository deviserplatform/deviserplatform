using Autofac;
using Deviser.Core.Library.DomainTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.FileManagement
{
    public class FileManagement : IFileManagement
    {
        private readonly ILogger<FileManagement> logger;
        private IHostingEnvironment hostingEnvironment;
        public FileManagement(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<FileManagement>>();
            hostingEnvironment = container.Resolve<IHostingEnvironment>();
        }

        public List<FileItem> GetFilesAndFolders()
        {
            //Linq (Language Integrated Query) to Objects
            try
            {
                string assetsPath = Path.Combine(hostingEnvironment.ContentRootPath + "\\wwwroot\\assets");
                List<FileItem> folders = Directory.GetDirectories(assetsPath).Select(folderPath => new FileItem
                {
                    Name = folderPath.Replace(assetsPath, ""),
                    Type = FileItemType.Folder
                }).ToList();

                List<FileItem> files = Directory.GetFiles(assetsPath).Select(folderPath => new FileItem
                {
                    Name = folderPath.Replace(assetsPath, ""),
                    Type = FileItemType.File
                }).ToList();

                folders.AddRange(files);
                return folders;
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all files and folders"), ex);
                throw ex;
            }
        }
    }
}
