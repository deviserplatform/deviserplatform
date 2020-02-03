using Deviser.Core.Common.Extensions;
using System.Linq.Expressions;

namespace Deviser.Admin.Config
{
    public class FieldCondition
    {
        private string _fieldName;
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
        public LambdaExpression ConditionExpression { get; set; }
    }
}