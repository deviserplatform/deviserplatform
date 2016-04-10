using Autofac;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Microsoft.AspNet.Mvc;


namespace Deviser.Modules.FileManager.Controllers
{
    [Module("FileManager")]
    public class HomeController : ModuleController
    {
        public HomeController(ILifetimeScope container)
            : base(container)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
