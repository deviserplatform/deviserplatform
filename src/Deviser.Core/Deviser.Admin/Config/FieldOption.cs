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

        public Field AddItemBy { get; set; }

        public Field AutoFillField { get; set; }

        [JsonIgnore]
        public LambdaExpression AutoFillExpression { get; set; }

        public CheckBoxMatrix CheckBoxMatrix { get; set; }

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

        //public List<RelatedField> RelatedFields { get; set; }
        public bool IsHidden { get; set; }
        public bool IsReadOnly { get; set; }
        public bool? IsRequired { get; set; }
        public int MaxLength { get; set; }

        //[JsonIgnore]
        //public ModelMetadata Metadata { get; set; }
        public string NullDisplayText { get; set; }
        /// <summary>
        /// When the FieldType is boolean and true, this text will be displayed.
        /// </summary>
        public string IsTrue { get; set; }

        /// <summary>
        /// When the FieldType is boolean and true, this text will be displayed.
        /// </summary>
        public string IsFalse { get; set; }
        public string RegExErrorMessage { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RelationType RelationType { get; set; }

        [JsonConverter(typeof(TypeJsonConverter))]
        public Type LookupModelType { get; set; }

        public string LookupModelTypeCamelCase => LookupModelType?.Name?.Camelize();

        [JsonIgnore]
        //public Expression<Func<object, string>> RelatedEntityDisplayExpression { get; set; }
        public LambdaExpression LookupDisplayExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression LookupExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression LookupKeyExpression { get; set; }
        
        [JsonIgnore]
        public LambdaExpression LookupFilterExpression { get; set; }

        public bool HasLookupFilter => LookupFilterExpression != null;

        public Field LookupFilterField { get; set;  }
        
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
