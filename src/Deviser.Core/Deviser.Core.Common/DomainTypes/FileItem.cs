using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Common.DomainTypes
{
    public class FileItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public FileItemType Type { get; set; }
    }
}
