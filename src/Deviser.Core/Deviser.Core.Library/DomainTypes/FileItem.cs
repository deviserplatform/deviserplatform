using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.DomainTypes
{
    public class FileItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public FileItemType Type { get; set; }
    }
}
