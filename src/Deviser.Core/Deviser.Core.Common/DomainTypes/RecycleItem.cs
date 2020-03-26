using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class RecycleItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public RecycleItemType RecycleItemType { get; set; }
    }
}
