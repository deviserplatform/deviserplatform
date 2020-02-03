using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Microsoft.AspNetCore.Mvc;


namespace Deviser.Modules.Blog
{
    [Module("Blog")]
    public class HomeController : ModuleController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
