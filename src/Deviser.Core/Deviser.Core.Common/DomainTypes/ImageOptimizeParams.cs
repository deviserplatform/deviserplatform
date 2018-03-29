using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class ImageOptimizeParams
    {
        public int Dpi { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public int QualityPercent { get; set; }

    }
}
