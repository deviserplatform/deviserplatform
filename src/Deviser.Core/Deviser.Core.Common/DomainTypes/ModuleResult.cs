using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class ContentResult
    {
        public int SortOrder { get; set;}
        public string Result { get; set; }
    }
}
