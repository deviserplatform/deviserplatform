using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Deviser.Admin.Config
{
    public class LabelOption
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LabelType LabelType { get; set; }
        public IDictionary<string, string> Parameters { get; }
        public LabelOption()
        {
            Parameters = new Dictionary<string, string>();
        }
    }

    public enum LabelType
    {
        Badge = 1,
        Icon = 2,
    }
}
