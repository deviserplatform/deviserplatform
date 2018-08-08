using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class SitemapUrl
    {
        public string Url { get; set; }


        public DateTime LastModified { get; set; }


        public SitemapChangeFrequency ChangeFrequency { get; set; }


        public float Priority { get; set; }

        public List<AlternateUrl> AlternateUrls { get; set; }
    }

    public class AlternateUrl
    {
        public string Language { get; set; }

        public string Url { get; set; }
    }

    public enum SitemapChangeFrequency
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never
    }
}
