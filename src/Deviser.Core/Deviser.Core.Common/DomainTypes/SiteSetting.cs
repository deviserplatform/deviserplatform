using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class SiteSetting
    {
        public Guid DefaultLayoutId { get; set; }
        public Guid DefaultAdminLayoutId { get; set; }
        public string DefaultAdminTheme { get; set; }        
        public Guid HomePageId { get; set; }
        public Guid LoginPageId { get; set; }
        public Guid RedirectAfterLogin { get; set; }
        public Guid RedirectAfterLogout { get; set; }
        public bool RegistrationEnabled { get; set; }
        public Guid RegistrationPageId { get; set; }
        public string SiteAdminEmail { get; set; }        
        public string DefaultTheme { get; set; }
        public string SiteDescription { get; set; }
        public string SiteLanguage { get; set; }
        public string SiteName { get; set; }        
        public string SiteRoot { get; set; }
    }
}
