using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deviser.WI.Controllers
{
    public class SitemapController : DeviserController
    {
        private readonly ILogger<SitemapController> _logger;
        private readonly ISitemapService _sitemapService;

        public SitemapController(ILogger<SitemapController> logger, 
            ISitemapService sitemapService)
        {
            _logger = logger;
            _sitemapService = sitemapService;
        }

        public IActionResult Index()
        {
            var siteMap = _sitemapService.GetXmlSitemap();
            return Content(siteMap, "text/xml");
        }
    }
}
