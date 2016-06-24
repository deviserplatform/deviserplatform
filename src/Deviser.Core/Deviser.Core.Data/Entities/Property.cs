using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.Entities
{
    public class Property
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        //Value property is not in DB, It is here to maintain JSON strucuture
        public string Value { get; set; }        
        public Guid? PropertyOptionListId { get; set; }
        public ICollection<LayoutTypeProperty> LayoutTypeProperties { get; set; }
        public ICollection<ContentTypeProperty> ContentTypeProperties { get; set; }
        public PropertyOptionList PropertyOptionList { get; set; }
        

    }
}
