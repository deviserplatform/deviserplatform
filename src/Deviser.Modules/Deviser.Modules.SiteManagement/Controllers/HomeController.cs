using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Microsoft.AspNetCore.Mvc;

namespace Deviser.Modules.SiteManagement.Controllers
{
    [Module("SiteManagement")]
    public class HomeController: ModuleController
    {
        //public HomeController(ILifetimeScope container)
        //    : base(container)
        //{

        //}

        public IActionResult SiteSettings()
        {
            return View();
        }
    }
}
