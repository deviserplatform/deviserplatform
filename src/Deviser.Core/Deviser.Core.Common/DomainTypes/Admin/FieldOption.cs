using Deviser.Core.Common.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq.Expressions;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public class FieldOption
    {
        [JsonIgnore]
        public ModelMetadata Metadata { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public int MaxLength { get; set; }
        public string NullDisplayText { get; set; }
        public bool IsHidden { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsRequired { get; set; }

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression ShowOn { get; set; }

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression EnableOn { get; set; }

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression ValidateOn { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ValidationType ValidationType { get; set; }
        public string ValidatorRegEx { get; set; }
        public string RegExErrorMessage { get; set; }
    }
}
