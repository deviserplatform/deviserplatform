using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class Language
    {
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CultureCode { get; set; }
        public string EnglishName { get; set; }
        public string FallbackCulture { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string NativeName { get; set; }
    }
}
