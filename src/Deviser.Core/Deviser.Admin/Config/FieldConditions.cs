using System.Collections.Generic;

namespace Deviser.Admin.Config
{
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
