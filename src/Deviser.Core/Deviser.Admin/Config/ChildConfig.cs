using System;
using System.Linq.Expressions;
using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public class ChildConfig : IChildConfig
    {
        [JsonIgnore]
        public EntityConfig EntityConfig { get; set; }
        
        [JsonIgnore]
        public LambdaExpression ShowOnStaticExpression { get; set; }

        [JsonIgnore]
        public Func<bool> ShowOnDelegateFunc { get; set; }

        public Field Field { get; set; }
        public bool IsShown => ShowOnDelegateFunc?.Invoke() ?? true; //By default it child config should be visible
        public IModelConfig ModelConfig { get; set; }
        
    }
}