using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deviser.Core.Common.Extensions;

namespace Deviser.Admin.Config.Filters
{
    public class FilterOperator
    {
        public IDictionary<string, string> DateTimeOperator { get; }
        public IDictionary<string, string> NumberOperator { get; }
        public IDictionary<string, string> TextOperator { get; }

        public FilterOperator()
        {
            DateTimeOperator = Enum.GetValues(typeof(DateTimeOperator)).OfType<DateTimeOperator>()
                .ToDictionary(k => k.ToString(), v => v.GetDescription());

            NumberOperator = Enum.GetValues(typeof(NumberOperator)).OfType<NumberOperator>()
                .ToDictionary(k => k.ToString(), v => v.GetDescription());
            
            TextOperator = Enum.GetValues(typeof(TextOperator)).OfType<TextOperator>()
                .ToDictionary(k => k.ToString(), v => v.GetDescription());
        }
    }
}
