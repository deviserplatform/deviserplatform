using System;

namespace Deviser.Modules.ContactForm.Data
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
