using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Json;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;

namespace Deviser.Core.Common.DomainTypes.Admin
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

                if (FieldExpression.Body.Type.IsCollectionType())
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

        public string FieldNameCamelCase
        {
            get
            {
                return FieldName.Camelize();
            }
        }
    }
}
