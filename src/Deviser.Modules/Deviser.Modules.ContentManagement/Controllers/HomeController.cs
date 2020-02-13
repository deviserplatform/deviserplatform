using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Microsoft.AspNetCore.Mvc;


namespace Deviser.Modules.ContentManagement.Controllers
{
    [Module("ContentManagement")]
    public class HomeController : ModuleController
    {
        //public HomeController(ILifetimeScope container)
        //    :base(container)
        //{

        //}
        public IActionResult ContentTypes()
        {
            return View();
        }

        public IActionResult LayoutTypes()
        {
            return View();
        }

        public IActionResult OptionList()
        {
            return View();
        }

        public IActionResult Properties()
        {
            return View();
        }
    }
}
