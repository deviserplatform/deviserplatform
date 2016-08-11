using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class SiteSetting
    {
        public string DefaultAdminLayoutId { get; set; }
        public string DefaultAdminTheme { get; set; }
        public string HomePageId { get; set; }
        public string LoginPageId { get; set; }
        public string RedirectAfterLogin { get; set; }
        public string RedirectAfterLogout { get; set; }
        public string RegistrationPageId { get; set; }
        public string SiteAdminEmail { get; set; }
        public string SiteDefaultLayout { get; set; }
        public string SiteDefaultTheme { get; set; }
        public string SiteDesctiption { get; set; }
        public string SiteName { get; set; }        
        public string SiteRoot { get; set; }
    }
}
