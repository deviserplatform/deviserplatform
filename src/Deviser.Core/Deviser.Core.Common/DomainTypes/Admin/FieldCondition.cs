using Deviser.Core.Common.Extensions;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Deviser.Core.Common.DomainTypes.Admin
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

    public class FieldConditions
    {
        public List<FieldCondition> ShowOnConditions { get; }
        public List<FieldCondition> EnableOnConditions { get; }
        public List<FieldCondition> ValidateOnConditions { get; }

        public FieldConditions()
        {
            ShowOnConditions = new List<FieldCondition>();
            EnableOnConditions = new List<FieldCondition>();
            ValidateOnConditions = new List<FieldCondition>();
        }

    }
}
