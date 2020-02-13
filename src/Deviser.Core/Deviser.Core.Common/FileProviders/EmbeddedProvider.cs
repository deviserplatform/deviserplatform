using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace Deviser.Core.Common.FileProviders
{
    public class EmbeddedProvider
    {

        public static IFileInfo GetFileInfo(Assembly assembly, string fileName)
        {
            var manifestEmbeddedProvider =
                new ManifestEmbeddedFileProvider(assembly);

            var fileInfo = manifestEmbeddedProvider.GetFileInfo(fileName);
            return fileInfo;
        }

        public static string GetFileContentAsString(Assembly assembly, string fileName)
        {
            var fileInfo = GetFileInfo(assembly, fileName);
            using var stream = fileInfo.CreateReadStream();
            using var reader = new StreamReader(stream);
            var fileContent = reader.ReadToEnd();
            return fileContent;
        }
    }
}
