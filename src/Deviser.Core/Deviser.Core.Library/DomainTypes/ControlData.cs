using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.DomainTypes
{
    public class ControlData
    {
        public string HtmlResult { get; set; }
        public List<PageContent> PageContents { get; set; }
        public PlaceHolder ContentItem { get; set; }
    }
}
