using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class LayoutContent
    {
        public PlaceHolder PlaceHolder { get; set; }
        public IHtmlContent ContentResult { get; set; }
    }
}
