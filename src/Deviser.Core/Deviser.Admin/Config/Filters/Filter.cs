using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Deviser.Admin.Config.Filters
{
    public class Filter
    {
        [BindRequired]
        public string FieldName { get; set; }
    }
}
