using System.Collections.Generic;
using Deviser.Admin.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin.Config.Filters
{
    public class FilterNode
    {
        /// <summary>
        /// Operator used to connect root
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public  LogicalOperator RootOperator { get; set; }

        /// <summary>
        /// Operator used to connect child nodes those belong to this node
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public LogicalOperator ChildOperator { get; set; }

        /// <summary>
        /// Actual filter
        /// </summary>
        /// 
        [JsonConverter(typeof(FilterJsonConverter))]
        public Filter Filter { get; set; }
        
        /// <summary>
        /// Reference of the parent filter
        /// </summary>
        //public FilterNode Parent { get; set; }
        
        /// <summary>
        /// Child filter
        /// </summary>
        public ICollection<FilterNode> ChildNodes { get; set; }
    }
}
