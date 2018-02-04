using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Library.IO
{
    public interface IFileManagement
    {
        List<FileItem> GetFilesAndFolders();
    }
}