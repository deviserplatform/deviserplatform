using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.DomainTypes
{
    public class PageLayout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PlaceHolder> PlaceHolders { get; set; }
        public int PageId { get; set; }
        public bool IsChanged { get; set; }
        public bool IsDeleted { get; set; }

    }
}
