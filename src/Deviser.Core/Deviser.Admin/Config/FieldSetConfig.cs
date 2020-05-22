using System.Collections.Generic;
using System.Linq;
using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public class FieldSetConfig : IFieldSetConfig
    {
        private readonly Dictionary<string, Field> _allFields;
        public ICollection<FieldSet> FieldSets { get; }

        [JsonIgnore]
        public ICollection<Field> AllIncludeFields => _allFields?.Values?.ToList();

        public FieldSetConfig()
        {
            _allFields = new Dictionary<string, Field>();
            FieldSets = new HashSet<FieldSet>();
        }

        public void AddFieldSet(FieldSet fieldSet)
        {
            foreach (var fieldRow in fieldSet.Fields)
            {
                foreach (var field in fieldRow)
                {
                    AddToDictionary(field);
                }
            }

            FieldSets.Add(fieldSet);
        }

        private void AddToDictionary(Field field)
        {
            if (!_allFields.ContainsKey(field.FieldName))
                _allFields.Add(field.FieldName, field);
        }

    }
}