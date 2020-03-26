using System.Collections.Generic;
using Deviser.Admin.Config;

namespace Deviser.Admin
{
    public class GridConfig : IGridConfig
    {
        private readonly Dictionary<string, Field> _allFields;

        public ICollection<Field> AllIncludeFields
        {
            get
            {   var returnList = new List<Field>();
                
                if (_allFields != null && _allFields.Count > 0)
                {
                    returnList.AddRange(_allFields.Values);
                }
                return returnList;
            }
        }
        public ICollection<Field> ExcludedFields { get; }
        public ICollection<Field> Fields { get; }
        public bool IsEditVisible { get; set; } = true;
        public bool IsDeleteVisible { get; set; } = true;
        public IDictionary<string, AdminAction> RowActions { get; }

        public GridConfig()
        {
            _allFields = new Dictionary<string, Field>();
            Fields = new HashSet<Field>();
            ExcludedFields = new HashSet<Field>();
            RowActions = new Dictionary<string, AdminAction>();
        }

        public void AddField(Field field)
        {
            AddToDictionary(field);

            Fields.Add(field);
        }

        public void RemoveField(Field field)
        {
            ExcludedFields.Add(field);
        }

        private void AddToDictionary(Field field)
        {
            _allFields.Add(field.FieldName, field);
        }
    }
}