using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public class ChildConfig : IChildConfig
    {
        [JsonIgnore]
        public EntityConfig EntityConfig { get; set; }
        public Field Field { get; set; }
        public IModelConfig ModelConfig { get; set; }
    }
}