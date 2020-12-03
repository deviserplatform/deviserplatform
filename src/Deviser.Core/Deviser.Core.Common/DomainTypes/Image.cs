using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class Image
    {
        public string ImageUrl { get; set; }
        public string ImageAltText { get; set; }
        public dynamic FocusPoint { get; set; }
    }
}
