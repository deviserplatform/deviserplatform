using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Library.FileManagement
{
    public interface IFileManagement
    {
        List<FileItem> GetFilesAndFolders();
    }
}