using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin.Config
{
    public class Field : BaseField
    {   

        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }

        public FieldOption FieldOption { get; set; } 
    }
}
