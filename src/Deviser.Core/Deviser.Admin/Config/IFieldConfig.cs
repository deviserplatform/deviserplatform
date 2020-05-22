using System.Collections.Generic;
using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public interface IFieldConfig
    {
        ICollection<ICollection<Field>> Fields { get; }
        
        [JsonIgnore]
        ICollection<Field> ExcludedFields { get; }

        [JsonIgnore]
        ICollection<Field> AllIncludeFields { get; }
        void AddField(Field field);
        void AddInLineField(Field field);
        void RemoveField(Field field);

    }
}