using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Core.Common.DomainTypes
{
    public class FileItem
    {
        public string Name { get; set; }
        
        public string Path { get; set; }
        
        public string Extension { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FileItemType Type { get; set; }
    }
}
