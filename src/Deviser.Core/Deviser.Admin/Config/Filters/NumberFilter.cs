using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin.Config.Filters
{
    public class NumberFilter : Filter
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public NumberOperator Operator { get; set; }
        public decimal Value { get; set; }
        public decimal From { get; set; }
        public decimal To { get; set; }
    }
}
