using Autofac;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Modules.SiteManagement.Controllers
{
    [Module("SiteManagement")]
    public class HomeController: ModuleController
    {
        public HomeController(ILifetimeScope container)
            : base(container)
        {

        }

        public IActionResult SiteSettings()
        {
            return View();
        }
    }
}
