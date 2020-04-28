using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin.Config.Filters
{
    public class DateTimeFilter : Filter
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public DateTimeOperator Operator { get; set; }
        public DateTime Value { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}