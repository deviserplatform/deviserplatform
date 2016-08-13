using Deviser.Core.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class PageContext
    {
        [JsonIgnore]
        public Page CurrentPage { get; set; }

        public Guid CurrentPageId { get; set; }

        public string CurrentUrl { get; set; }
        [JsonIgnore]
        public CultureInfo CurrentCulture
        {
            get; set;
        }

        [JsonIgnore]
        public Deviser.Core.Data.Entities.PageTranslation CurrentTranslation
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
                return SiteSetting.SiteRoot + HomePageUrl;
            }
        }

        public string SiteRoot
        {
            get
            {
                return SiteSetting.SiteRoot; //Expose it to json
            }
        }

        [JsonIgnore]
        public SiteSetting SiteSetting { get; set; }
    }
}
