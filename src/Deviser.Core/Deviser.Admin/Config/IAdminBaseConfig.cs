using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public interface IAdminBaseConfig
    {
        [JsonIgnore]
        EntityConfig EntityConfig { get; set; }
        IModelConfig ModelConfig { get; }
    }
}