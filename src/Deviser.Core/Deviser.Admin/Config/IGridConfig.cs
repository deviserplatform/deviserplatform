using System.Collections.Generic;
using Deviser.Admin.Config;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public interface IGridConfig
    {
        [JsonIgnore] ICollection<Field> AllIncludeFields { get; }
        [JsonIgnore] ICollection<Field> ExcludedFields { get; }
        ICollection<Field> Fields { get; }
        IDictionary<string, AdminAction> RowActions { get; }
        void AddField(Field field);
        void RemoveField(Field field);
    }
}