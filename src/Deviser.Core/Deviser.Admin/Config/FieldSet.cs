using System.Collections.Generic;

namespace Deviser.Admin.Config
{  
    public class FieldSet
    {
        public string GroupName { get; set; }
        public string CssClasses { get; set; }
        public string Description { get; set; }
        public ICollection<ICollection<Field>> Fields { get; set; }
    }
}
