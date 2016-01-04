using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.DomainTypes
{
    public class ContentItem
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string LayoutTemplate { get; set; }
        public int Index { get; set; }
        public Module Module { get; set; }
        public List<ItemProperty> Properties { get; set; }
        public List<ContentItem> ContentItems { get; set; }
    }
}
