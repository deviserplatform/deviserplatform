using System.Collections.Generic;
using System.Linq;
using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public class FieldConfig : IFieldConfig
    {
        private readonly Dictionary<string, Field> _allFields;
        public ICollection<ICollection<Field>> Fields { get; }
        
        public ICollection<Field> ExcludedFields { get; }

        public ICollection<Field> AllIncludeFields => _allFields?.Values?.ToList();

        public FieldConfig()
        {
            _allFields = new Dictionary<string, Field>();
            Fields = new List<ICollection<Field>>();
            ExcludedFields = new HashSet<Field>();
        }

        public void AddField(Field field)
        {
            AddToDictionary(field);

            Fields.Add(new List<Field>() {
                field
            });
        }

        public void AddInLineField(Field field)
        {
            AddToDictionary(field);

            if (Fields.Count == 0)
                Fields.Add(new List<Field>());

            var fieldRow = Fields.Last();

            fieldRow.Add(field);
        }

        private void AddToDictionary(Field field)
        {
            _allFields.Add(field.FieldName, field);
        }

        public void RemoveField(Field field)
        {
            ExcludedFields.Add(field);
        }
    }
}