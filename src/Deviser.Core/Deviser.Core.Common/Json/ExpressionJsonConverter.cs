using Deviser.Core.Common.Lambda2Js;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Deviser.Core.Common.Json
{
    public class ExpressionJsonConverter : JsonConverter
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
            if (value is Expression)
            {
                var expr = value as Expression;
                var jsBody = expr.CompileToJavascript(new JavascriptCompilationOptions(JsCompilationFlags.BodyOnly));
                var jsExpr = $"return {jsBody}";
                serializer.Serialize(writer, jsExpr);
            }
        }
    }
}
