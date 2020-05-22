using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Deviser.Admin.Config.Filters
{
    public enum TextOperator
    {
        [Description("Starts With")]
        StartsWith = 0,
        [Description("Ends With")]
        EndsWith = 1,
        [Description("Contains")]
        Contains = 2,
        [Description("Equal")]
        Equal = 3,
        [Description("Not Equal")]
        NotEqual = 4
    }
}
