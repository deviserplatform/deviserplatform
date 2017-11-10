using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class PageContext
    {
        [JsonIgnore]
        public virtual Page CurrentPage { get; set; }

        public Guid CurrentPageId { get; set; }

        public string CurrentUrl { get; set; }

        [JsonIgnore]
        public CultureInfo CurrentCulture
        {
            get; set;
        }

        [JsonIgnore]
        public PageTranslation CurrentTranslation
        {
            get
            {
                var translation = CurrentPage.PageTranslation.FirstOrDefault(t => t.Locale == CurrentLocale);
                return translation;
            }
        }

        public string CurrentLocale
        {
            get
            {
                return CurrentCulture.Name;
            }
        }

        public bool HasPageViewPermission { get; set; }

        public bool HasPageEditPermission { get; set; }

        [JsonIgnore]
        public Page HomePage { get; set; }

        public string HomePageUrl
        {
            get
            {
                var homePageTranslation = HomePage.PageTranslation.FirstOrDefault(t => t.Locale == CurrentCulture.ToString());
                return (homePageTranslation != null) ? homePageTranslation.URL : "";
            }
        }

        public string HomePageFullUrl
        {
            get
            {
                return SiteSettingInfo.SiteRoot + HomePageUrl;
            }
        }

        public bool IsMultilingual { get; set; }

        public string SiteRoot
        {
            get
            {
                return SiteSettingInfo.SiteRoot; //Expose it to json
            }
        }

        public string SiteLanguage
        {
            get
            {
                return SiteSettingInfo.SiteLanguage;
            }
        }

        public string SelectedTheme
        {
            get
            {
                var selectedTheme = CurrentPage != null ? Regex.Match(CurrentPage.SkinSrc, @"\/([^)]*)\/").Groups[1].Value : "";
                return selectedTheme;
            }
        }

        [JsonIgnore]
        public SiteSettingInfo SiteSettingInfo { get; set; }
    }
}
