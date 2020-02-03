using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Microsoft.AspNetCore.Mvc;


namespace Deviser.Modules.ModuleManagement.Controllers
{
    [Module("ModuleManagement")]
    public class HomeController : ModuleController
    {
        //public HomeController(ILifetimeScope container)
        //    : base(container)
        //{

        //}
        public IActionResult Modules()
        {
            return View();
        }
    }
}
