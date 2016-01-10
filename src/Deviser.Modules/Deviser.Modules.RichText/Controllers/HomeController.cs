using Autofac;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Modules.RichText.Controllers
{
    [Module("PageManagement")]
    public class HomeController  : ModuleController
    {
        public HomeController(ILifetimeScope container)
            :base(container)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
