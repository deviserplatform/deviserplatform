using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
   public class Contact
   {
        public Guid Id { get; set; }
        public Guid PageModuleId { get; set; }
        public string Data { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
   }
    
}
