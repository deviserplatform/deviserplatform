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
        bool IsEditVisible { get; set; }
        bool IsDeleteVisible { get; set; }
        IDictionary<string, AdminAction> RowActions { get; }
        void AddField(Field field);
        void RemoveField(Field field);
    }
}