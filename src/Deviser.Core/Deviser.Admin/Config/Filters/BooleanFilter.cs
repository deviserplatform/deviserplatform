using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config.Filters
{
    public class BooleanFilter : Filter
    {
        public bool IsTrue { get; set; }
        public bool IsFalse { get; set; }
    }
}
