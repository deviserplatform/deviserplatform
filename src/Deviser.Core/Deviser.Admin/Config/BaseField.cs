using System;
using System.Linq.Expressions;
using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Json;
using Newtonsoft.Json;

namespace Deviser.Admin.Config
{
    public class BaseField
    {
        private string _fieldName;

        [JsonIgnore]
        public LambdaExpression FieldExpression { get; set; }

        [JsonConverter(typeof(TypeJsonConverter))]
        public Type FieldClrType
        {
            get
            {
                if (FieldExpression == null)
                    return null;

                if (ReflectionExtensions.IsCollectionType(FieldExpression.Body.Type))
                    return FieldExpression.Body.Type.GenericTypeArguments[0];

                return FieldExpression.Body.Type;
            }
        }

        public string FieldName
        {
            get
            {
                if (!string.IsNullOrEmpty(_fieldName))
                    return _fieldName;

                if (FieldExpression == null)
                    return null;

                _fieldName = ReflectionExtensions.GetMemberName(FieldExpression);
                return _fieldName;
            }
        }

        public string FieldNameCamelCase => !string.IsNullOrEmpty(_fieldName) ? FieldName.Camelize() : string.Empty;
    }
}
