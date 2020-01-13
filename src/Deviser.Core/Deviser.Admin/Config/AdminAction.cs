using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json;

namespace Deviser.Admin.Config
{
    public class AdminAction
    {
        public string ButtonText { get; set; }
        
        [JsonIgnore]
        public LambdaExpression FormActionExpression { get; set; }
    }
}
