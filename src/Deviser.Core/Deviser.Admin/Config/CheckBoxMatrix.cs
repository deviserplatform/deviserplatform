using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Deviser.Core.Common.Json;
using Deviser.Core.Common.Lambda2Js;
using Newtonsoft.Json;

namespace Deviser.Admin.Config
{
    public class CheckBoxMatrix
    {
        //public BaseField RowField { get; }
        //public BaseField ColumnField { get; }
        [JsonConverter(typeof(TypeJsonConverter))]
        public Type RowType { get; set; }

        public string RowTypeCamelCase => RowType?.Name.Camelize();

        [JsonConverter(typeof(TypeJsonConverter))]
        public Type ColumnType { get; set; }

        public string ColumnTypeCamelCase => ColumnType?.Name.Camelize();
        public KeyField MatrixKeyField { get; }
        public KeyField RowKeyField { get; }
        public KeyField ColumnKeyField { get; }
        public KeyField ContextKeyField { get; }

        [JsonIgnore]
        //public Expression<Func<object, string>> RelatedEntityDisplayExpression { get; set; }
        public LambdaExpression RowLookupDisplayExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression RowLookupExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression RowLookupKeyExpression { get; set; }

        [JsonIgnore]
        //public Expression<Func<object, string>> RelatedEntityDisplayExpression { get; set; }
        public LambdaExpression ColLookupDisplayExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression ColLookupExpression { get; set; }

        [JsonIgnore]
        public LambdaExpression ColLookupKeyExpression { get; set; }

        public CheckBoxMatrix()
        {
            //RowField = new BaseField();
            //ColumnField = new BaseField();
            MatrixKeyField = new KeyField();
            RowKeyField = new KeyField();
            ColumnKeyField = new KeyField();
            ContextKeyField = new KeyField();
        }
    }
}
