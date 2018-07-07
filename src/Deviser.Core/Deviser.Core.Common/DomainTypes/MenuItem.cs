using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class MenuItem
    {
        public MenuItem Parent { get; set; }
        public Page Page { get; set; }
        public bool HasChild { get; set; }
        public string PageName { get; set; }                
        public int PageLevel { get; set; }
        public bool IsActive { get; set; }
        public bool IsBreadCrumb { get; set; }
        public bool IsLinkNewWindow { get; set; }
        public string URL { get; set; }
        public virtual ICollection<MenuItem> ChildMenuItems { get; set; }

    }
}
