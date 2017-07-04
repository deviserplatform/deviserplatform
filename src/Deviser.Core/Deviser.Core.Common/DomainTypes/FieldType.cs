using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class FieldType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } //System or Custom
        public string Label { get; set; }
    }
}
