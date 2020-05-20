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
        public string Number { get; set; }
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }
    }
}
