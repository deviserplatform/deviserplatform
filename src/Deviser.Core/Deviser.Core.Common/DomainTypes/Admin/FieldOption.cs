using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public class FieldOption
    {
        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression EnableOn { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }        
        public string Format { get; set; }
        public List<ReleatedField> ReleatedFields { get; set; }
        public bool IsHidden { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsRequired { get; set; }
        public int MaxLength { get; set; }

        [JsonIgnore]
        public ModelMetadata Metadata { get; set; }
        public string NullDisplayText { get; set; }
        public string RegExErrorMessage { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RelationType RelationType { get; set; }

        [JsonConverter(typeof(TypeJsonConverter))]
        public Type ReleatedEntityType { get; set; }

        public string ReleatedEntityTypeCamelCase
        {
            get
            {
                return ReleatedEntityType?.Name?.Camelize();
            }
        }

        [JsonIgnore]
        //public Expression<Func<object, string>> ReleatedEntityDisplayExpression { get; set; }
        public LambdaExpression ReleatedEntityDisplayExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression ReleatedEntityLookupExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression ReleatedEntityLookupKeyExpression { get; set; }

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression ShowOn { get; set; }        

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression ValidateOn { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ValidationType ValidationType { get; set; }        
        public string ValidatorRegEx { get; set; }        
    }
}
