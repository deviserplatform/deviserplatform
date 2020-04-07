using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class RecycleItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RecycleItemTypeString => RecycleItemType?.Name;

        public string RecycleItemTypeIconClass
        {
            get
            {
                if (RecycleItemType == null) return string.Empty;

                return RecycleItemType.Name switch
                {
                    "Layouts" => "icon layouts",
                    "Page" => "icon page",
                    "PageContent" => "icon page-content",
                    "PageModule" => "icon page-module",
                    _ => string.Empty
                };
            }
        }
        public RecycleItemType RecycleItemType { get; set; }
    }
}
