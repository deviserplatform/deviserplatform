using Deviser.Core.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public class Field : BaseField
    {   

        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }

        public FieldOption FieldOption { get; set; }
    }
}
