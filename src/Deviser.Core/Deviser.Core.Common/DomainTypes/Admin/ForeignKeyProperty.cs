using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Json;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;


namespace Deviser.Core.Common.DomainTypes.Admin
{
    public class ForeignKeyProperty
    {
        private string _fieldName;
        private string _principalFieldName;

        public bool IsPKProperty { get; set; }

        [JsonIgnore]
        public LambdaExpression FieldExpression { get; set; }

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

        [JsonIgnore]
        public LambdaExpression PrincipalFieldExpression { get; set; }

        [JsonConverter(typeof(TypeJsonConverter))]
        public Type FKEntityType
        {
            get
            {
                if (FieldExpression == null)
                    return null;
                return FieldExpression.Type.GenericTypeArguments[0];
            }
        }

        [JsonConverter(typeof(TypeJsonConverter))]
        public Type FKFieldType
        {
            get
            {
                if (FieldExpression == null)
                    return null;
                return FieldExpression.Type.GenericTypeArguments[1];
            }
        }

        [JsonConverter(typeof(TypeJsonConverter))]
        public Type PKEntityType
        {
            get
            {
                if (PrincipalFieldExpression == null)
                    return null;
                return PrincipalFieldExpression.Type.GenericTypeArguments[0];
            }
        }

        [JsonConverter(typeof(TypeJsonConverter))]
        public Type PKFieldType
        {
            get
            {
                if (PrincipalFieldExpression == null)
                    return null;
                return PrincipalFieldExpression.Type.GenericTypeArguments[1];
            }
        }

        public string PrincipalFieldName
        {
            get
            {
                if (!string.IsNullOrEmpty(_principalFieldName))
                    return _principalFieldName;

                if (PrincipalFieldExpression == null)
                    return null;

                _principalFieldName = ReflectionExtensions.GetMemberName(PrincipalFieldExpression);
                return _principalFieldName;
            }
        }

        public string PrincipalFieldNameCamelCase
        {
            get
            {
                return PrincipalFieldName.Camelize();
            }
        }
    }
}
