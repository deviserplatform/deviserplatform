using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Deviser.Admin.Config.Filters
{
    public enum NumberOperator
    {
        [Description(">")]
        GreaterThan = 0,
        [Description(">=")]
        GreaterThanOrEqual = 1,
        [Description("=")]
        Equal = 2,
        [Description("<>")]
        NotEqual = 3,
        [Description("<")]
        LessThan = 4,
        [Description("<=")]
        LessThanOrEqual = 5,
        [Description("Range")]
        InRange = 6
    }
}
