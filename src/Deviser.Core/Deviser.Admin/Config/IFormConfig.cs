using System.Collections.Generic;
using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public interface IFormConfig
    {
        [JsonIgnore] ICollection<Field> AllFormFields { get; }
        IFieldConfig FieldConfig { get; }
        IFieldSetConfig FieldSetConfig { get; }
        [JsonIgnore] FieldConditions FieldConditions { get; }        
        IDictionary<string, AdminAction> FormActions { get; }
        FormOption FormOption { get; set; }
    }
}