using Deviser.Core.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public class Field
    {

        private string _fieldName;

        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }

        [JsonIgnore]
        public LambdaExpression FieldExpression { get; set; }

        [JsonIgnore]
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

        public FieldOption FieldOption { get; set; }
    }
}
