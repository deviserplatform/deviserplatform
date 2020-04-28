using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Deviser.Admin.Config.Filters
{
    public enum DateTimeOperator
    {
        [Description("After")]
        After = 0,
        [Description("After And")]
        AfterAnd = 1,
        [Description("Equal")]
        Equal = 2,
        [Description("Not Equal")]
        NotEqual = 3,
        [Description("Before")]
        Before = 4,
        [Description("Before And")]
        BeforeAnd = 5,
        [Description("In Range")]
        InRange = 6
    }
}
