using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Deviser.WI.Infrastructure
{
    public class SDJsonConvert
    {
        static JsonSerializerSettings settings;

        static SDJsonConvert()
        {
            settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                Formatting = Newtonsoft.Json.Formatting.Indented
            };
        }

        public static string Serialize(object value)
        {
            string result = JsonConvert.SerializeObject(value, settings);
            return result;
        }

        public static T DeserializeObject<T>(string value)
        {
            return (T)JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}
