using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class Language
    {
        public Guid Id { get; set; }
        public string CultureCode { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
        public string FallbackCulture { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
