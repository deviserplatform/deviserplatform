using Deviser.Admin.Config;
using Deviser.Core.Common.Lambda2Js;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Deviser.Admin.Extensions
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
            if (value is LambdaExpression)
            {
                var expr = value as LambdaExpression;
                var jsBody = expr.CompileToJavascript(new JavascriptCompilationOptions(JsCompilationFlags.BodyOnly));
                var jsExpr = $"return {jsBody};";
                var exprObject = new FieldExpression
                {
                    Parameters = expr.Parameters.Select(p => p.Name).ToList(),
                    Expression = jsExpr
                };

                serializer.Serialize(writer, exprObject);
            }
        }
    }
}
