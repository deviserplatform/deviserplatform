using Deviser.Admin.Extensions;
using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Json;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Deviser.Admin.Config
{
    public class FieldOption
    {
        public FieldType FieldType { get; set; }

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression EnableOn { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public LabelOption LabelOption { get; set; }
        public string Format { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FormMode ShowIn { get; set; } = FormMode.Both;

        [JsonConverter(typeof(StringEnumConverter))]
        public FormMode EnableIn { get; set; } = FormMode.Both;

        public List<RelatedField> RelatedFields { get; set; }
        public bool IsHidden { get; set; }
        public bool IsReadOnly { get; set; }
        public bool? IsRequired { get; set; }
        public int MaxLength { get; set; }

        //[JsonIgnore]
        //public ModelMetadata Metadata { get; set; }
        public string NullDisplayText { get; set; }
        public string RegExErrorMessage { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RelationType RelationType { get; set; }

        [JsonConverter(typeof(TypeJsonConverter))]
        public Type RelatedModelType { get; set; }

        public string RelatedModelTypeCamelCase => RelatedModelType?.Name?.Camelize();

        [JsonIgnore]
        //public Expression<Func<object, string>> RelatedEntityDisplayExpression { get; set; }
        public LambdaExpression RelatedModelDisplayExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression RelatedModelLookupExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression RelatedModelLookupKeyExpression { get; set; }

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression ShowOn { get; set; }

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression ValidateOn { get; set; }

        [JsonIgnore]
        public LambdaExpression ValidationExpression { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ValidationType ValidationType { get; set; }
        public string ValidatorRegEx { get; set; }
    }
}
