﻿using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Microsoft.AspNetCore.Mvc;

namespace Deviser.Modules.RecycleBin.Controllers
{
    [Module("RecycleBin")]
    public class HomeController : ModuleController
    {
        //public HomeController(ILifetimeScope container)
        //    : base(container)
        //{

        //}
        public IActionResult Index()
        {
            return View();
        }
    }
}
