using Autofac;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.Controllers
{
    public class SitemapController : DeviserController
    {
        private readonly ILogger<SitemapController> _logger;
        private readonly ISitemapService _sitemapService;

        public SitemapController(ILifetimeScope container)
        {
            _logger = container.Resolve<ILogger<SitemapController>>();
            _sitemapService = container.Resolve<ISitemapService>();
        }

        public IActionResult Index()
        {
            var siteMap = _sitemapService.GetXmlSitemap();
            return Content(siteMap, "text/xml");
        }
    }
}
