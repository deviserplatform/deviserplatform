using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class ContentTypeControl
    {
        public Guid Id { get; set; }
        public Guid ContentControlId { get; set; }
        public Guid ContentTypeId { get; set; }
        public Guid OptionListId { get; set; }
        public virtual ContentType ContentType { get; set; }
        public virtual ContentControl ContentControl { get; set; }
        public OptionList OptionList { get; set; }

    }
}
