using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class SiteSettingInfo
    {
        public bool EnableTwitterAuth { get; set; }
        public bool EnableGoogleAuth { get; set; }
        public bool EnableFacebookAuth { get; set; }


        //public Guid RedirectAfterLogin { get; set; }
        //public Guid RedirectAfterLogout { get; set; }
        //public string SiteLanguage { get; set; }


        public string SiteName { get; set; }
        public string SiteDescription { get; set; }
        public string SiteAdminEmail { get; set; }
        public string SiteRoot { get; set; }
        public string SiteHeaderTags { get; set; }
        public Language SiteLanguage { get; set; }


        public Guid HomePageId => HomePage?.Id ?? Guid.Empty;
        public Page HomePage { get; set; }
        public Guid LoginPageId => LoginPage?.Id ?? Guid.Empty;
        public Page LoginPage { get; set; }
        public Guid RegistrationPageId => RegistrationPage?.Id ?? Guid.Empty;
        public Page RegistrationPage { get; set; }
        public Page RedirectAfterLogout { get; set; }
        public Page RedirectAfterLogin { get; set; }

        public SMTPAuthentication SMTPAuthentication { get; set; }
        public bool SMTPEnableSSL { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPServerAndPort { get; set; }
        public string SMTPUsername { get; set; }

        public Guid DefaultLayoutId => DefaultLayout?.Id ?? Guid.Empty;
        public PageLayout DefaultLayout { get; set; }
        public Guid DefaultAdminLayoutId => DefaultAdminLayout?.Id ?? Guid.Empty;
        public PageLayout DefaultAdminLayout { get; set; }
        public Theme DefaultTheme { get; set; }
        public Theme DefaultAdminTheme { get; set; }
        public bool RegistrationEnabled { get; set; }
        public string FacebookAppId { get; set; }
        public string FacebookAppSecret { get; set; }
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }
    }
}
