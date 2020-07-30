using System.Collections.Generic;
using System.Linq.Expressions;
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
        bool IsSortable { get; }
        IDictionary<string, AdminAction> RowActions { get; }
        public Field SortField { get; set; }
        [JsonIgnore] public LambdaExpression OnSortExpression { get; set; }
        void AddField(Field field);
        void RemoveField(Field field);
    }
}