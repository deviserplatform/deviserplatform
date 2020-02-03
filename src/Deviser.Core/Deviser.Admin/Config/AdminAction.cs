using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Deviser.Admin.Config
{
    public class AdminAction
    {
        public string ButtonText { get; set; }
        
        [JsonIgnore]
        public LambdaExpression FormActionExpression { get; set; }
    }
}
