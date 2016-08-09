using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class PageTranslation
    {
        public Guid PageId { get; set; }
        public string Locale { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }

        public virtual Page Page { get; set; }
    }
}
