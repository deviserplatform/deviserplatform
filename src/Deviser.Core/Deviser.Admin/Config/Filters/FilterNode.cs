using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin.Config.Filters
{
    public class FilterNode
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public  NodeOperator Operator { get; set; }
        public Filter Filter { get; set; }
        public FilterNode Parent { get; set; }
        public ICollection<FilterNode> ChildNodes { get; set; }
    }
}
