using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace Deviser.Admin.Config.Filters
{
    public class TextFilter : Filter
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TextOperator Operator { get; set; }
        public string Text { get; set; }
    }
}
