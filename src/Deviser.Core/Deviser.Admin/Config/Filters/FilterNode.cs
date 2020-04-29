﻿using System.Collections.Generic;
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
        public  FilterOperator RootOperator { get; set; }

        /// <summary>
        /// Operator used to connect child nodes those belong to this node
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterOperator ChildOperator { get; set; }
        
        /// <summary>
        /// Actual filter
        /// </summary>
        public Filter Filter { get; set; }
        
        /// <summary>
        /// Reference of the parent filter
        /// </summary>
        public FilterNode Parent { get; set; }
        
        /// <summary>
        /// Child filter
        /// </summary>
        public ICollection<FilterNode> ChildNodes { get; set; }
    }
}
