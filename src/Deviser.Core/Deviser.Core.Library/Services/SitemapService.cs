using Autofac;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Xml;
using System.Globalization;
using Deviser.Core.Common;

namespace Deviser.Core.Library.Services
{
    public class SitemapService : ISitemapService
    {
        private const string SITEMAP_VERSION = "0.9";
        private readonly INavigation _navigation;
        private readonly ILanguageRepository _languageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private List<string> _enabledLanguages;
        private bool _isMultilingual;
        private string _requestHost;
        private string _protocolHost;

        public SitemapService(ILifetimeScope container)
        {
            _navigation = container.Resolve<INavigation>();
            _languageRepository = container.Resolve<ILanguageRepository>();
            _httpContextAccessor = container.Resolve<IHttpContextAccessor>();
        }

        public string GetXmlSitemap()
        {
            var activeLanguages = _languageRepository.GetActiveLanguages();
            _isMultilingual = activeLanguages != null && activeLanguages.Count > 0;
            _enabledLanguages = _isMultilingual ? activeLanguages.Select(al => al.CultureCode).ToList() : null;
            var isSecureConnection = _httpContextAccessor.HttpContext.Request.IsHttps;

            _requestHost = _httpContextAccessor.HttpContext.Request.Host.Host;
            var scheme = isSecureConnection ? "https://" : "http://";
            _protocolHost = scheme + _requestHost;


            var publicPages = _navigation.GetPublicPages();
            List<SitemapUrl> sitemapUrls = new List<SitemapUrl>();

            if (publicPages != null && publicPages.Count > 0)
            {
                foreach (var page in publicPages)
                {
                    sitemapUrls.Add(GetSitemapUrl(page));
                }
            }

            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;

            using (var stringWriter = new StringWriterUtf8())
            {
                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    // build header
                    writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/" + SITEMAP_VERSION);
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");
                    var schemaLocation = "http://www.sitemaps.org/schemas/sitemap/" + SITEMAP_VERSION;
                    writer.WriteAttributeString("xsi", "schemaLocation", null, string.Format("{0} {0}/sitemap.xsd", schemaLocation));

                    // write urls to output
                    foreach (SitemapUrl url in sitemapUrls)
                    {
                        AddURL(url, writer);
                    }

                    writer.WriteEndElement();
                    writer.Close();
                }
                return stringWriter.ToString();
            }
        }

        private SitemapUrl GetSitemapUrl(Page page)
        {
            try
            {
                SitemapUrl sitemapUrl = new SitemapUrl();
                sitemapUrl.Url = _protocolHost + _navigation.NavigateUrl(page); ;
                sitemapUrl.Priority = GetPriority(page);
                sitemapUrl.LastModified = page.LastModifiedDate != null ? (DateTime)page.LastModifiedDate : DateTime.Now;
                sitemapUrl.ChangeFrequency = SitemapChangeFrequency.Daily;

                if (_isMultilingual)
                {
                    sitemapUrl.AlternateUrls = new List<AlternateUrl>();
                    foreach (var language in _enabledLanguages)
                    {

                        var alternativeUrl = _navigation.NavigateUrl(page, language);
                        if (!string.IsNullOrEmpty(alternativeUrl))
                        {
                            sitemapUrl.AlternateUrls.Add(new AlternateUrl()
                            {
                                Language = language.ToLower(),
                                Url = _protocolHost + alternativeUrl
                            });
                        }
                    }
                }
                return sitemapUrl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private float GetPriority(Page page)
        {
            float priority = page.SiteMapPriority > 0 ? page.SiteMapPriority : (float)0.5;

            //if (useLevelBasedPagePriority)
            //{
            //    if (page.Level >= 9)
            //    {
            //        priority = 0.1F;
            //    }
            //    else
            //    {
            //        priority = Convert.ToSingle(1 - (page.Level * 0.1));
            //    }

            //    if (priority < minPagePriority)
            //    {
            //        priority = minPagePriority;
            //    }
            //}

            return priority;
        }

        private void AddURL(SitemapUrl sitemapUrl, XmlWriter writer)
        {
            writer.WriteStartElement("url");
            writer.WriteElementString("loc", sitemapUrl.Url);
            writer.WriteElementString("lastmod", sitemapUrl.LastModified.ToString("yyyy-MM-dd"));
            writer.WriteElementString("changefreq", sitemapUrl.ChangeFrequency.ToString().ToLower());
            writer.WriteElementString("priority", sitemapUrl.Priority.ToString("F01", CultureInfo.InvariantCulture));

            if (sitemapUrl.AlternateUrls != null)
            {
                foreach (AlternateUrl alternate in sitemapUrl.AlternateUrls)
                {
                    writer.WriteStartElement("link", "http://www.w3.org/1999/xhtml");
                    writer.WriteAttributeString("rel", "alternate");
                    writer.WriteAttributeString("hreflang", alternate.Language);
                    writer.WriteAttributeString("href", alternate.Url);
                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement();
        }
    }
}
