using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class ContentControl
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string IconImage { get; set; }
        public string IconClass { get; set; }
        public Guid? OptionListId { get; set; }
        public ICollection<ContentTypeControl> ContentTypeControls { get; set; }        
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
