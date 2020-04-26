using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Multilingual;

namespace Deviser.Core.Library.Sites
{
    public class SettingManager : ISettingManager
    {
        //Logger
        private readonly ILogger<SettingManager> _logger;
        private readonly ILanguageManager _languageManager;
        private readonly ILayoutManager _layoutManager;
        private readonly IPageRepository _pageRepository;
        private readonly ISiteSettingRepository _siteSettingRepository;
        private readonly IThemeManager _themeManager;


        public SettingManager(ILogger<SettingManager> logger,
            ILanguageManager languageManager,
            ILayoutManager layoutManager,
            IPageRepository pageRepository,
            ISiteSettingRepository siteSettingRepository,
            IThemeManager themeManager)
        {
            _logger = logger;
            _languageManager = languageManager;
            _layoutManager = layoutManager;
            _pageRepository = pageRepository;
            _siteSettingRepository = siteSettingRepository;
            _themeManager = themeManager;
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
                //var settings = _siteSettingRepository.GetSettings();
                //var strRegistrationEnabled = settings.First(s => s.SettingName == "RegistrationEnabled").SettingValue;
                //var strDefaultAdminLayoutId = settings.First(s => s.SettingName == "DefaultAdminLayoutId").SettingValue;
                //var strHomePageId = settings.First(s => s.SettingName == "HomePageId").SettingValue;
                //var strLoginPageId = settings.First(s => s.SettingName == "LoginPageId").SettingValue;
                //var strRedirectAfterLogin = settings.First(s => s.SettingName == "RedirectAfterLogin").SettingValue;
                //var strRedirectAfterLogout = settings.First(s => s.SettingName == "RedirectAfterLogout").SettingValue;
                //var strRegistrationPageId = settings.First(s => s.SettingName == "RegistrationPageId").SettingValue;
                //var strDefaultLayoutId = settings.First(s => s.SettingName == "DefaultLayoutId").SettingValue;
                //var result = new SiteSettingInfo()
                //{
                //    DefaultAdminLayoutId = string.IsNullOrEmpty(strDefaultAdminLayoutId) ? Guid.Parse(strDefaultAdminLayoutId) : Guid.Empty,
                //    DefaultLayoutId = !string.IsNullOrEmpty(strDefaultLayoutId) ? Guid.Parse(strDefaultLayoutId) : Guid.Empty,
                //    DefaultAdminTheme = settings.First(s => s.SettingName == "DefaultAdminTheme").SettingValue,
                //    HomePageId = !string.IsNullOrEmpty(strHomePageId) ? Guid.Parse(strHomePageId) : Guid.Empty,
                //    LoginPageId = !string.IsNullOrEmpty(strLoginPageId) ? Guid.Parse(strLoginPageId) : Guid.Empty,
                //    RedirectAfterLogin = !string.IsNullOrEmpty(strRedirectAfterLogin) ? Guid.Parse(strRedirectAfterLogin) : Guid.Empty,
                //    RedirectAfterLogout = !string.IsNullOrEmpty(strRedirectAfterLogout) ? Guid.Parse(strRedirectAfterLogout) : Guid.Empty,
                //    RegistrationEnabled = !string.IsNullOrEmpty(strRegistrationEnabled) ? bool.Parse(strRegistrationEnabled) : false,
                //    RegistrationPageId = !string.IsNullOrEmpty(strRegistrationPageId) ? Guid.Parse(strRegistrationPageId) : Guid.Empty,
                //    SiteAdminEmail = settings.First(s => s.SettingName == "SiteAdminEmail").SettingValue,
                //    DefaultTheme = settings.First(s => s.SettingName == "DefaultTheme").SettingValue,
                //    SiteDescription = settings.First(s => s.SettingName == "SiteDescription").SettingValue,
                //    SiteLanguage = settings.First(s => s.SettingName == "SiteLanguage").SettingValue,
                //    SiteName = settings.First(s => s.SettingName == "SiteName").SettingValue,
                //    SiteRoot = settings.First(s => s.SettingName == "SiteRoot").SettingValue,
                //};

                var settings = _siteSettingRepository.GetSettingsAsDictionary();
                var allLanguages = _languageManager.GetAllLanguages();
                var allPages = GetPages();
                var allAuthenticationTypes = SMTPAuthentication.GetSmtpAuthentications();
                var allThemes = GetThemes();
                var allLayouts = _layoutManager.GetPageLayouts();


                var strSiteLanguage = settings[nameof(SiteSettingInfo.SiteLanguage)];
                var strHomePageId = settings[nameof(SiteSettingInfo.HomePageId)];
                var strLoginPageId = settings[nameof(SiteSettingInfo.LoginPageId)];
                var strRegistrationPageId = settings[nameof(SiteSettingInfo.RegistrationPageId)];
                var strRedirectAfterLoginPageId = settings[nameof(SiteSettingInfo.RedirectAfterLogin)];
                var strRedirectAfterLogoutPageId = settings[nameof(SiteSettingInfo.RedirectAfterLogout)];

                var strSMTPAuthentication = settings[nameof(SiteSettingInfo.SmtpAuthentication)];
                var strSMTPEnableSSL = settings[nameof(SiteSettingInfo.SmtpEnableSSL)];

                var strDefaultLayoutId = settings[nameof(SiteSettingInfo.DefaultLayoutId)];
                var strDefaultTheme = settings[nameof(SiteSettingInfo.DefaultTheme)];
                var strDefaultAdminLayoutId = settings[nameof(SiteSettingInfo.DefaultAdminLayoutId)];
                var strDefaultAdminTheme = settings[nameof(SiteSettingInfo.DefaultAdminTheme)];


                var strRegistrationEnabled = settings[nameof(SiteSettingInfo.RegistrationEnabled)];
                var strEnableFacebookAuth = settings[nameof(SiteSettingInfo.EnableFacebookAuth)];
                var strEnableGoogleAuth = settings[nameof(SiteSettingInfo.EnableGoogleAuth)];
                var strEnableTwitterAuth = settings[nameof(SiteSettingInfo.EnableTwitterAuth)];

                var siteSettings = new SiteSettingInfo()
                {
                    SiteName = settings[nameof(SiteSettingInfo.SiteName)],
                    SiteDescription = settings[nameof(SiteSettingInfo.SiteDescription)],
                    SiteAdminEmail = settings[nameof(SiteSettingInfo.SiteAdminEmail)],
                    SiteRoot = settings[nameof(SiteSettingInfo.SiteRoot)],
                    SiteHeaderTags = settings[nameof(SiteSettingInfo.SiteHeaderTags)],
                    SiteLanguage = allLanguages.FirstOrDefault(l =>
                        string.Equals(l.CultureCode, strSiteLanguage, StringComparison.InvariantCultureIgnoreCase)),

                    HomePage = !string.IsNullOrEmpty(strHomePageId) ? allPages.FirstOrDefault(p => p.Id == Guid.Parse(strHomePageId)) : null,
                    LoginPage = !string.IsNullOrEmpty(strLoginPageId) ? allPages.FirstOrDefault(p => p.Id == Guid.Parse(strLoginPageId)) : null,
                    RegistrationPage = !string.IsNullOrEmpty(strRegistrationPageId) ? allPages.FirstOrDefault(p => p.Id == Guid.Parse(strRegistrationPageId)) : null,
                    RedirectAfterLogin = !string.IsNullOrEmpty(strRedirectAfterLoginPageId) ? allPages.FirstOrDefault(p => p.Id == Guid.Parse(strRedirectAfterLoginPageId)) : null,
                    RedirectAfterLogout = !string.IsNullOrEmpty(strRedirectAfterLogoutPageId) ? allPages.FirstOrDefault(p => p.Id == Guid.Parse(strRedirectAfterLogoutPageId)) : null,

                    SmtpServerAndPort = settings[nameof(SiteSettingInfo.SmtpServerAndPort)],
                    SmtpAuthentication = allAuthenticationTypes.FirstOrDefault(at => string.Equals(at.Name, strSMTPAuthentication, StringComparison.InvariantCultureIgnoreCase)),
                    SmtpEnableSSL = !string.IsNullOrEmpty(strSMTPEnableSSL) && bool.Parse(strSMTPEnableSSL),
                    SmtpUsername = settings[nameof(SiteSettingInfo.SmtpUsername)],
                    SmtpPassword = settings[nameof(SiteSettingInfo.SmtpPassword)],

                    DefaultLayout = !string.IsNullOrEmpty(strDefaultLayoutId) ? allLayouts.FirstOrDefault(l => l.Id == Guid.Parse(strDefaultLayoutId)) : null,
                    DefaultAdminLayout = !string.IsNullOrEmpty(strDefaultAdminLayoutId) ? allLayouts.FirstOrDefault(l => l.Id == Guid.Parse(strDefaultAdminLayoutId)) : null,
                    DefaultTheme = !string.IsNullOrEmpty(strDefaultTheme) ? allThemes.FirstOrDefault(t => t.Key == strDefaultTheme) : null,
                    DefaultAdminTheme = !string.IsNullOrEmpty(strDefaultAdminTheme) ? allThemes.FirstOrDefault(t => t.Key == strDefaultAdminTheme) : null,

                    RegistrationEnabled = !string.IsNullOrEmpty(strRegistrationEnabled) && bool.Parse(strRegistrationEnabled),
                    EnableFacebookAuth = !string.IsNullOrEmpty(strEnableFacebookAuth) && bool.Parse(strEnableFacebookAuth),
                    EnableGoogleAuth = !string.IsNullOrEmpty(strEnableGoogleAuth) && bool.Parse(strEnableGoogleAuth),
                    EnableTwitterAuth = !string.IsNullOrEmpty(strEnableTwitterAuth) && bool.Parse(strEnableTwitterAuth),
                    FacebookAppId = settings[nameof(SiteSettingInfo.FacebookAppId)],
                    FacebookAppSecret = settings[nameof(SiteSettingInfo.FacebookAppSecret)],
                    GoogleClientId = settings[nameof(SiteSettingInfo.GoogleClientId)],
                    GoogleClientSecret = settings[nameof(SiteSettingInfo.GoogleClientSecret)],
                    TwitterConsumerKey = settings[nameof(SiteSettingInfo.TwitterConsumerKey)],
                    TwitterConsumerSecret = settings[nameof(SiteSettingInfo.TwitterConsumerSecret)],

                };

                return siteSettings;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting SiteSettings", ex);
            }
            return null;
        }

        public SiteSettingInfo UpdateSettingInfo(SiteSettingInfo settingInfo)
        {
            var settings = _siteSettingRepository.GetSettings();
            var allAuthenticationTypes = SMTPAuthentication.GetSmtpAuthentications();

            settings.First(s => s.SettingName == nameof(SiteSettingInfo.SiteName)).SettingValue = settingInfo.SiteName;
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.SiteDescription)).SettingValue = settingInfo.SiteDescription;
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.SiteAdminEmail)).SettingValue = settingInfo.SiteAdminEmail;
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.SiteRoot)).SettingValue = settingInfo.SiteRoot;
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.SiteHeaderTags)).SettingValue = settingInfo.SiteHeaderTags;

            if (settingInfo.SiteLanguage != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.SiteLanguage)).SettingValue = settingInfo.SiteLanguage.CultureCode;
            }

            if (settingInfo.HomePage != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.HomePageId)).SettingValue = settingInfo.HomePage.Id.ToString();
            }
            if (settingInfo.LoginPage != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.LoginPageId)).SettingValue = settingInfo.LoginPage.Id.ToString();
            }
            if (settingInfo.RegistrationPage != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.RegistrationPageId)).SettingValue = settingInfo.RegistrationPage.Id.ToString();
            }
            if (settingInfo.RedirectAfterLogin != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.RedirectAfterLogin)).SettingValue = settingInfo.RedirectAfterLogin.Id.ToString();
            }
            if (settingInfo.RedirectAfterLogout != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.RedirectAfterLogout)).SettingValue = settingInfo.RedirectAfterLogout.Id.ToString();
            }

            settings.First(s => s.SettingName == nameof(SiteSettingInfo.SmtpServerAndPort)).SettingValue = settingInfo.SmtpServerAndPort;
            if (settingInfo.SmtpAuthentication != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.SmtpAuthentication)).SettingValue = allAuthenticationTypes.First(at => at.Id == settingInfo.SmtpAuthentication.Id).Name;
            }
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.SmtpEnableSSL)).SettingValue = settingInfo.SmtpEnableSSL.ToString().ToLower();
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.SmtpUsername)).SettingValue = settingInfo.SmtpUsername;
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.SmtpPassword)).SettingValue = settingInfo.SmtpPassword;

            if (settingInfo.DefaultLayout != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.DefaultLayoutId)).SettingValue = settingInfo.DefaultLayout.Id.ToString();
            }
            if (settingInfo.DefaultAdminLayout != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.DefaultAdminLayoutId)).SettingValue = settingInfo.DefaultAdminLayout.Id.ToString();
            }
            if (settingInfo.DefaultTheme != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.DefaultTheme)).SettingValue = settingInfo.DefaultTheme.Key;
            }
            if (settingInfo.DefaultAdminTheme != null)
            {
                settings.First(s => s.SettingName == nameof(SiteSettingInfo.DefaultAdminTheme)).SettingValue = settingInfo.DefaultAdminTheme.Key;
            }

            settings.First(s => s.SettingName == nameof(SiteSettingInfo.RegistrationEnabled)).SettingValue = settingInfo.RegistrationEnabled.ToString().ToLower();
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.EnableFacebookAuth)).SettingValue = settingInfo.EnableFacebookAuth.ToString().ToLower();
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.EnableGoogleAuth)).SettingValue = settingInfo.EnableGoogleAuth.ToString().ToLower();
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.EnableTwitterAuth)).SettingValue = settingInfo.EnableTwitterAuth.ToString().ToLower();
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.FacebookAppId)).SettingValue = settingInfo.FacebookAppId;
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.FacebookAppSecret)).SettingValue = settingInfo.FacebookAppSecret;
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.GoogleClientId)).SettingValue = settingInfo.GoogleClientId;
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.GoogleClientSecret)).SettingValue = settingInfo.GoogleClientSecret;
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.TwitterConsumerKey)).SettingValue = settingInfo.TwitterConsumerKey;
            settings.First(s => s.SettingName == nameof(SiteSettingInfo.TwitterConsumerSecret)).SettingValue = settingInfo.TwitterConsumerSecret;

            _siteSettingRepository.UpdateSetting(settings);

            return GetSiteSetting();

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

        private IList<Page> GetPages()
        {
            var result = _pageRepository.GetPagesFlat();
            return result;
        }

        private List<Theme> GetThemes()
        {
            var themes = _themeManager.GetHostThemes().Select(kvp => new Theme() { Key = kvp.Value, Value = kvp.Key })
                .ToList();
            return themes;
        }
    }
}
