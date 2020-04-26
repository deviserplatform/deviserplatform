using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class Language
    {
        public Guid Id { get; set; }
        
        //Only used in Language Admin UI
        public Language SelectedLanguage { get; set; }
        public string CultureCode { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
        public string FallbackCulture { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveText => IsActive ? "Active" : "In Active";
        public string IsActiveBadgeClass => IsActive ? "badge-primary" : "badge-secondary";
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
