using System.Collections.Generic;
using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public interface IFieldSetConfig
    {
        ICollection<FieldSet> FieldSets { get; }

        [JsonIgnore]
        ICollection<Field> AllIncludeFields { get; }

        void AddFieldSet(FieldSet fieldSet);
    }
}