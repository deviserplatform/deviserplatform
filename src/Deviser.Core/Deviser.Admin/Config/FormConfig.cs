using System.Collections.Generic;
using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public class FormConfig : IFormConfig
    {
        [JsonIgnore] public ICollection<Field> AllFields
        {
            get
            {
                var fields = FieldConfig.AllIncludeFields;
                var fieldSetFields = FieldSetConfig.AllIncludeFields;
                var returnList = new List<Field>();

                if (fields != null && fields.Count > 0)
                {
                    returnList.AddRange(fields);
                }

                if (fieldSetFields != null && fieldSetFields.Count > 0)
                {
                    returnList.AddRange(fieldSetFields);
                }

                return returnList;
            }
        }
        public IFieldConfig FieldConfig { get; }
        public FieldConditions FieldConditions { get; }
        public IFieldSetConfig FieldSetConfig { get; }
        public IDictionary<string, AdminAction> FormActions { get; }
        public FormOption FormOption { get; set; }
        public string Title { get; set; }

        public FormConfig()
        {
            FieldConfig = new FieldConfig();
            FieldConditions = new FieldConditions();
            FieldSetConfig = new FieldSetConfig();
            FormActions = new Dictionary<string, AdminAction>();
        }
    }
}