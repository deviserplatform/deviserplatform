using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class ContentTypeControl
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Placeholder { get; set; }        
        public Guid ContentControlId { get; set; }
        public Guid ContentTypeId { get; set; }
        public Guid OptionListId { get; set; }
        public Guid? ValidatorId { get; set; }
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
        public string Properties { get; set; }
        public virtual ContentType ContentType { get; set; }
        public virtual ContentControl ContentControl { get; set; }
        public OptionList OptionList { get; set; }
        public virtual Validator Validator { get; set; }

    }
}
