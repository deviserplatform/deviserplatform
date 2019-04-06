using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public class KeyField : BaseField
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public KeyFieldType KeyFieldType { get; set; }
    }
}
