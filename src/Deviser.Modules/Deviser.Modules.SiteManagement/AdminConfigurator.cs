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
                    formBuilder
                        .AddFieldSet("General", fieldBuilder =>
                        {
                            fieldBuilder
                                .AddField(f => f.SiteName)
                                .AddField(f => f.SiteDescription, option => option.FieldType = FieldType.TextArea)
                                .AddField(f => f.SiteAdminEmail, option => option.FieldType = FieldType.EmailAddress)
                                .AddField(f => f.SiteRoot)
                                .AddField(f => f.SiteHeaderTags)
                                .AddSelectField(f => f.SiteLanguage, de => $"{de.EnglishName} ({de.NativeName})");

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
                                .AddField(f => f.SMTPServerAndPort)
                                .AddSelectField(f => f.SMTPAuthentication, de => de.Name, option => option.FieldType = FieldType.RadioButton)
                                .AddField(f => f.SMTPEnableSSL)
                                .AddField(f => f.SMTPUsername)
                                .AddField(f => f.SMTPPassword, option => option.FieldType = FieldType.Password);
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
                                .AddField(f => f.RegistrationEnabled);
                        });

                    formBuilder.Property(f => f.SiteLanguage).HasLookup(
                        sp => sp.GetService<ILanguageManager>().GetAllLanguages(false),
                        ke => ke.Id,
                        de => $"{de.EnglishName} ({de.NativeName})");

                    formBuilder.Property(f => f.HomePage).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetPages(),
                        ke => ke.Id,
                        de => de.PageTranslation.FirstOrDefault().Name);

                    formBuilder.Property(f => f.LoginPage).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetPages(),
                        ke => ke.Id,
                        de => de.PageTranslation.FirstOrDefault().Name);

                    formBuilder.Property(f => f.RegistrationPage).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetPages(),
                        ke => ke.Id,
                        de => de.PageTranslation.FirstOrDefault().Name);

                    formBuilder.Property(f => f.RedirectAfterLogin).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetPages(),
                        ke => ke.Id,
                        de => de.PageTranslation.FirstOrDefault().Name);

                    formBuilder.Property(f => f.RedirectAfterLogout).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetPages(),
                        ke => ke.Id,
                        de => de.PageTranslation.FirstOrDefault().Name);

                    formBuilder.Property(f => f.SMTPAuthentication).HasLookup(
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
                        de => de.Key);

                    formBuilder.Property(f => f.DefaultAdminTheme).HasLookup(
                        sp => sp.GetService<SiteSettingAdminService>().GetThemes(),
                        ke => ke.Key,
                        de => de.Key);
                });
        }
    }
}
