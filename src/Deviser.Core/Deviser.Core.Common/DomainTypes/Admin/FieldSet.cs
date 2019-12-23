using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Deviser.Core.Common.DomainTypes.Admin
{  
    public class FieldSet
    {
        public string GroupName { get; set; }
        public string CssClasses { get; set; }
        public string Description { get; set; }
        public ICollection<ICollection<Field>> Fields { get; set; }
    }
}
