using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.Json
{
    public class TypeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;//typeof(Expression).IsAssignableFrom(objectType);
        }

        // this converter is only used for serialization, not to deserialize
        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        => throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Type)
            {
                var type = value as Type;
                var returnType = type.IsGenericType ? type.GenericTypeArguments[0] : type;
                serializer.Serialize(writer, returnType.Name);
            }
        }
    }
}
