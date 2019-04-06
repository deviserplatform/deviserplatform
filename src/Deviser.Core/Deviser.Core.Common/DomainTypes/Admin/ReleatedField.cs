using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Json;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    /// <summary>
    /// <see cref="ReleatedField"/> represents expected structure and data source of ManyToMany join entity
    /// </summary>
    public class ReleatedField : BaseField
    {
        private string _fieldName;
        private string _principalFieldName;

        public bool IsParentField { get; set; }

        //[JsonIgnore]
        //public LambdaExpression FieldExpression { get; set; }

        //public string FieldName
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(_fieldName))
        //            return _fieldName;

        //        if (FieldExpression == null)
        //            return null;

        //        _fieldName = ReflectionExtensions.GetMemberName(FieldExpression);
        //        return _fieldName;
        //    }
        //}

        //public string FieldNameCamelCase
        //{
        //    get
        //    {
        //        return FieldName.Camelize();
        //    }
        //}

        [JsonIgnore]
        public LambdaExpression SourceFieldExpression { get; set; }

        //[JsonConverter(typeof(TypeJsonConverter))]
        //public Type FieldClrType
        //{
        //    get
        //    {
        //        if (FieldExpression == null)
        //            return null;
        //        return FieldExpression.Type.GenericTypeArguments[0];
        //    }
        //}       

        [JsonConverter(typeof(TypeJsonConverter))]
        public Type SourceClrType
        {
            get
            {
                if (SourceFieldExpression == null)
                    return null;
                return SourceFieldExpression.Type.GenericTypeArguments[0];
            }
        }

        public string SourceFieldName
        {
            get
            {
                if (!string.IsNullOrEmpty(_principalFieldName))
                    return _principalFieldName;

                if (SourceFieldExpression == null)
                    return null;

                _principalFieldName = ReflectionExtensions.GetMemberName(SourceFieldExpression);
                return _principalFieldName;
            }
        }

        public string SourceFieldNameCamelCase
        {
            get
            {
                return SourceFieldName.Camelize();
            }
        }
    }
}
