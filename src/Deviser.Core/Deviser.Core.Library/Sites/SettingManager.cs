using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Deviser.Core.Library.Sites
{
    public class SettingManager : ISettingManager
    {
        //Logger
        private readonly ILogger<SettingManager> _logger;
        private ISiteSettingRepository _siteSettingRepository;

        public SettingManager(ILogger<SettingManager> logger,
            ISiteSettingRepository siteSettingRepository)
        {
            _logger = logger;
            _siteSettingRepository = siteSettingRepository;
        }

        public SMTPSetting GetSMTPSetting()
        {
            try
            {
                var settings = _siteSettingRepository.GetSettings();
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
                _logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public SiteSettingInfo GetSiteSetting()
        {
            try
            {
                var settings = _siteSettingRepository.GetSettings();
                var strRegistrationEnabled = settings.First(s => s.SettingName == "RegistrationEnabled").SettingValue;
                var strDefaultAdminLayoutId = settings.First(s => s.SettingName == "DefaultAdminLayoutId").SettingValue;
                var strHomePageId = settings.First(s => s.SettingName == "HomePageId").SettingValue;
                var strLoginPageId = settings.First(s => s.SettingName == "LoginPageId").SettingValue;
                var strRedirectAfterLogin = settings.First(s => s.SettingName == "RedirectAfterLogin").SettingValue;
                var strRedirectAfterLogout = settings.First(s => s.SettingName == "RedirectAfterLogout").SettingValue;
                var strRegistrationPageId = settings.First(s => s.SettingName == "RegistrationPageId").SettingValue;
                var strDefaultLayoutId = settings.First(s => s.SettingName == "DefaultLayoutId").SettingValue;
                var result = new SiteSettingInfo()
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
                    SiteDescription = settings.First(s => s.SettingName == "SiteDescription").SettingValue,
                    SiteLanguage = settings.First(s => s.SettingName == "SiteLanguage").SettingValue,
                    SiteName = settings.First(s => s.SettingName == "SiteName").SettingValue,
                    SiteRoot = settings.First(s => s.SettingName == "SiteRoot").SettingValue,
                };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public string GetSettingValue(string settingName)
        {
            try
            {
                return _siteSettingRepository.GetSettingValue(settingName);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetSettings", ex);
            }
            return null;
        }
    }
}
