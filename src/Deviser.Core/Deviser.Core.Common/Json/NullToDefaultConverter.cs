using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Common.Json
{
    /// <summary>
    /// JsonConverter that converts a null Json token to a default value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullToDefaultConverter<T> : JsonConverter where T : struct
    {
        /// <summary>
        ///  Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token == null || token.Type == JTokenType.Null || string.IsNullOrEmpty(((JValue)token).Value as string))
                return default(T);
            return token.ToObject(objectType);
        }

        /// <summary>
        /// Gets a value indicating whether this Newtonsoft.Json.JsonConverter can write.
        /// Return false instead if you don't want default values to be written as null
        /// </summary>
        public override bool CanWrite { get { return true; } }

        /// <summary>
        ///  Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (EqualityComparer<T>.Default.Equals((T)value, default(T)))
                writer.WriteNull();
            else
                writer.WriteValue(value);
        }
    }
}
