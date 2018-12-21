using Deviser.Core.Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public class ForeignKeyField
    {
        private string _fieldName;
        private string _principalFieldName;

        [JsonIgnore]
        public LambdaExpression FieldExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression PrincipalFieldExpression { get; set; }

        [JsonIgnore]
        public Type FieldClrType
        {
            get
            {
                if (FieldExpression == null)
                    return null;
                return FieldExpression.Type.GenericTypeArguments[1];
            }
        }

        [JsonIgnore]
        public Type PrincipalFieldClrType
        {
            get
            {
                if (PrincipalFieldExpression == null)
                    return null;
                return PrincipalFieldExpression.Type.GenericTypeArguments[1];
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

        public string FieldNameCamelCase
        {
            get
            {
                return FieldName.Camelize();
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
