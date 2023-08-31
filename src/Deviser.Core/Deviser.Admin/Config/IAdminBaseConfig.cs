using Deviser.Admin.Config;
using Deviser.Core.Common.Json;
using Newtonsoft.Json;
using System;

namespace Deviser.Admin
{
    public interface IAdminBaseConfig
    {
        [JsonIgnore]
        EntityConfig EntityConfig { get; set; }
        IModelConfig ModelConfig { get; }

        [JsonConverter(typeof(TypeJsonConverter))]
        Type ModelType { get; }
    }
}