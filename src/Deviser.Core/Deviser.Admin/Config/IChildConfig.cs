using System.Linq.Expressions;
using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public interface IChildConfig : IAdminBaseConfig
    {
        Field Field { get; }
        IModelConfig ModelConfig { get; }

        [JsonIgnore]
        LambdaExpression ShowOnStaticExpression { get; set; }

        bool IsShown { get; }
    }
}