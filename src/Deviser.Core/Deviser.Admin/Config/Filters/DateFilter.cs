using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin.Config.Filters
{
    public class DateFilter : Filter
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public DateTimeOperator Operator { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}