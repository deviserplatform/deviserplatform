using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deviser.Admin;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Multilingual;
using Deviser.Modules.SiteManagement.Services;
using Microsoft.Extensions.DependencyInjection;
using FieldType = Deviser.Admin.Config.FieldType;

namespace Deviser.Modules.SiteManagement
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.RegisterForm<SiteSettingInfo, SiteSettingAdminService>(formBuilder =>
            {
                formBuilder.Title = "Site Settings";
                    formBuilder
                        .AddFieldSet("General", fieldBuilder =>
                        {
                            fieldBuilder
                                .AddField(f => f.SiteName)
                                .AddField(f => f.SiteDescription, option => option.FieldType = FieldType.TextArea)
                                .AddField(f => f.SiteAdminEmail, option => option.FieldType = FieldType.EmailAddress)
                                .AddField(f => f.SiteRoot)
                                .AddField(f => f.SiteHeaderTags)
                                .AddSelectField(f => f.SiteLanguage);

                        })
                        .AddFieldSet("Page Configuration", fieldBuilder =>
                        {
                            fieldBuilder
                                .AddSelectField(f => f.HomePage)
                                .AddSelectField(f => f.LoginPage)
                                .AddSelectField(f => f.RegistrationPage)
                                .AddSelectField(f => f.RedirectAfterLogin)
                                .AddSelectField(f => f.RedirectAfterLogout);
                        })
                        .AddFieldSet("SMTP", fieldBuilder =>
                        {
                            fieldBuilder
                                .AddField(f => f.SmtpServerAndPort)
                                .AddSelectField(f => f.SmtpAuthentication, de => de.Name, option => option.FieldType = FieldType.RadioButton)
                                .AddField(f => f.SmtpEnableSSL)
                                .AddField(f => f.SmtpUsername)
                                .AddField(f => f.SmtpPassword, option => option.FieldType = FieldType.Password);
                        })
                        .AddFieldSet("Appearance", fieldBuilder =>
                        {
                            fieldBuilder
                                .AddSelectField(f => f.DefaultLayout)
                                .AddSelectField(f => f.DefaultTheme)
                                .AddSelectField(f => f.DefaultAdminLayout)
                                .AddSelectField(f => f.DefaultAdminTheme);
                        })
                        .AddFieldSet("Security", fieldBuilder =>
                        {
                            fieldBuilder
                                .AddField(f => f.RegistrationEnabled)
                                .AddField(f => f.EnableGoogleAuth)
                                .AddField(f => f.GoogleClientId)
                                .AddField(f => f.GoogleClientSecret)
                                .AddField(f => f.EnableFacebookAuth)
                                .AddField(f => f.FacebookAppId)
                                .AddField(f => f.FacebookAppSecret)
                                .AddField(f => f.EnableTwitterAuth)
                                .AddField(f => f.TwitterConsumerKey)
                                .AddField(f => f.TwitterConsumerSecret);
                        });

                    formBuilder.Property(f => f.EnableGoogleAuth)
                        .ShowOn(f => f.RegistrationEnabled)
                        .ValidateOn(f => f.RegistrationEnabled);

                    formBuilder.Property(f => f.EnableFacebookAuth)
                        .ShowOn(f => f.RegistrationEnabled)
                        .ValidateOn(f => f.RegistrationEnabled);

                    formBuilder.Property(f => f.EnableTwitterAuth)
                        .ShowOn(f => f.RegistrationEnabled)
                        .ValidateOn(f => f.RegistrationEnabled);

                    formBuilder.Property(f => f.GoogleClientId)
                        .ShowOn(f => f.EnableGoogleAuth)
                        .ValidateOn(f => f.EnableGoogleAuth);

                    formBuilder.Property(f => f.GoogleClientSecret)
                        .ShowOn(f => f.EnableGoogleAuth)
                        .ValidateOn(f => f.EnableGoogleAuth);

                    formBuilder.Property(f => f.FacebookAppId)
                        .ShowOn(f => f.EnableFacebookAuth)
                        .ValidateOn(f => f.EnableFacebookAuth);

                    formBuilder.Property(f => f.FacebookAppSecret)
                        .ShowOn(f => f.EnableFacebookAuth)
                        .ValidateOn(f => f.EnableFacebookAuth);

                    formBuilder.Property(f => f.TwitterConsumerKey)
                        .ShowOn(f => f.EnableTwitterAuth)
                        .ValidateOn(f => f.EnableTwitterAuth);

                    formBuilder.Property(f => f.TwitterConsumerSecret)
                        .ShowOn(f => f.EnableTwitterAuth)
                        .ValidateOn(f => f.EnableTwitterAuth);


                    formBuilder.Property(f => f.SiteLanguage).HasLookup(
                        sp => sp.GetService<ILanguageManager>().GetAllLanguages(false),
                        ke => ke.CultureCode,
                        de => $"{de.EnglishName}");

                    formBuilder.Property(f => f.HomePage).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetPages(),
                        ke => ke.Id,
                        de => de.PageTranslation.Count > 0 ? de.PageTranslation.FirstOrDefault().Name : string.Empty);

                    formBuilder.Property(f => f.LoginPage).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetPages(),
                        ke => ke.Id,
                        de => de.PageTranslation.Count > 0 ? de.PageTranslation.FirstOrDefault().Name : string.Empty);

                    formBuilder.Property(f => f.RegistrationPage).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetPages(),
                        ke => ke.Id,
                        de => de.PageTranslation.Count > 0 ? de.PageTranslation.FirstOrDefault().Name : string.Empty);

                    formBuilder.Property(f => f.RedirectAfterLogin).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetPages(),
                        ke => ke.Id,
                        de => de.PageTranslation.Count > 0 ? de.PageTranslation.FirstOrDefault().Name : string.Empty);

                    formBuilder.Property(f => f.RedirectAfterLogout).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetPages(),
                        ke => ke.Id,
                        de => de.PageTranslation.Count > 0 ? de.PageTranslation.FirstOrDefault().Name : string.Empty);

                    formBuilder.Property(f => f.SmtpAuthentication).HasLookup(
                        sp => SMTPAuthentication.GetSmtpAuthentications(),
                        ke => ke.Id,
                        de => de.Name);

                    formBuilder.Property(f => f.DefaultLayout).HasLookup(
                        sp => sp.GetService<ILayoutManager>().GetPageLayouts(),
                        ke => ke.Id,
                        de => de.Name);

                    formBuilder.Property(f => f.DefaultAdminLayout).HasLookup(
                        sp => sp.GetService<ILayoutManager>().GetPageLayouts(),
                        ke => ke.Id,
                        de => de.Name);

                    formBuilder.Property(f => f.DefaultTheme).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetThemes(),
                        ke => ke.Key,
                        de => de.Value);

                    formBuilder.Property(f => f.DefaultAdminTheme).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetThemes(),
                        ke => ke.Key,
                        de => de.Value);
                });
        }
    }
}
