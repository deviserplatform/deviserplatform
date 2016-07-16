﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Common.DomainTypes
{
    public class ContentType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } //old property - Type        
        public string Label { get; set; }
        public string IconImage { get; set; }
        public string IconClass { get; set; }
        public int SortOrder { get; set; }
        public string DataType { get; set; }
        public ContentDataType ContentDataType { get; set; }
        public ICollection<Property> Properties { get; set; }
    }
}