using Autofac;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.DataProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Sites
{
    public class SettingManager : ISettingManager
    {
        //Logger
        private readonly ILogger<ContentManager> logger;
        private ISiteSettingProvider siteSettingProvider;

        public SettingManager(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<ContentManager>>();
            siteSettingProvider = container.Resolve<ISiteSettingProvider>();
        }

        public SMTPSetting GetSMTPSetting()
        {
            try
            {
                var settings = siteSettingProvider.GetSettings();
                var serverPort = settings.First(s => s.SettingName == "SMTPServerAndPort").SettingValue;
                var ssl = settings.First(s => s.SettingName == "SMTPEnableSSL").SettingValue;
                var authenticationType = settings.First(s => s.SettingName == "SMTPAuthentication").SettingValue;
                var userName = settings.First(s => s.SettingName == "SMTPUsername").SettingValue;
                var password = settings.First(s => s.SettingName == "SMTPPassword").SettingValue;

                var result = new SMTPSetting()
                {
                    Server = !string.IsNullOrEmpty(serverPort.Split(':')[0]) ? serverPort.Split(':')[0] : "",
                    Port = !string.IsNullOrEmpty(serverPort.Split(':')[1]) ? int.Parse(serverPort.Split(':')[1]) : 25,
                    EnableSSL = !string.IsNullOrEmpty(ssl) ? bool.Parse(ssl) : false,
                    AuthenticationType = authenticationType,
                    Username = userName,
                    Password = password
                };
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public SiteSetting GetSiteSetting()
        {
            try
            {
                var settings = siteSettingProvider.GetSettings();
                var strRegistrationEnabled = settings.First(s => s.SettingName == "RegistrationEnabled").SettingValue;
                var strDefaultAdminLayoutId = settings.First(s => s.SettingName == "DefaultAdminLayoutId").SettingValue;
                var strHomePageId = settings.First(s => s.SettingName == "HomePageId").SettingValue;
                var strLoginPageId = settings.First(s => s.SettingName == "LoginPageId").SettingValue;
                var strRedirectAfterLogin = settings.First(s => s.SettingName == "RedirectAfterLogin").SettingValue;
                var strRedirectAfterLogout = settings.First(s => s.SettingName == "RedirectAfterLogout").SettingValue;
                var strRegistrationPageId = settings.First(s => s.SettingName == "RegistrationPageId").SettingValue;
                var strDefaultLayoutId = settings.First(s => s.SettingName == "DefaultLayoutId").SettingValue;
                var result = new SiteSetting()
                {
                    DefaultAdminLayoutId = string.IsNullOrEmpty(strDefaultAdminLayoutId) ? Guid.Parse(strDefaultAdminLayoutId) : Guid.Empty,
                    DefaultLayoutId = !string.IsNullOrEmpty(strDefaultLayoutId) ? Guid.Parse(strDefaultLayoutId) : Guid.Empty,
                    DefaultAdminTheme = settings.First(s => s.SettingName == "DefaultAdminTheme").SettingValue,
                    HomePageId = !string.IsNullOrEmpty(strHomePageId) ? Guid.Parse(strHomePageId) : Guid.Empty,
                    LoginPageId = !string.IsNullOrEmpty(strLoginPageId) ? Guid.Parse(strLoginPageId) : Guid.Empty,
                    RedirectAfterLogin = !string.IsNullOrEmpty(strRedirectAfterLogin) ? Guid.Parse(strRedirectAfterLogin) : Guid.Empty,
                    RedirectAfterLogout = !string.IsNullOrEmpty(strRedirectAfterLogout) ? Guid.Parse(strRedirectAfterLogout) : Guid.Empty,
                    RegistrationEnabled = !string.IsNullOrEmpty(strRegistrationEnabled) ? bool.Parse(strRegistrationEnabled) : false,
                    RegistrationPageId = !string.IsNullOrEmpty(strRegistrationPageId) ? Guid.Parse(strRegistrationPageId) : Guid.Empty,
                    SiteAdminEmail = settings.First(s => s.SettingName == "SiteAdminEmail").SettingValue,
                    DefaultTheme = settings.First(s => s.SettingName == "DefaultTheme").SettingValue,
                    SiteDesctiption = settings.First(s => s.SettingName == "SiteDesctiption").SettingValue,
                    SiteName = settings.First(s => s.SettingName == "SiteName").SettingValue,
                    SiteRoot = settings.First(s => s.SettingName == "SiteRoot").SettingValue,
                };
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }
    }
}
