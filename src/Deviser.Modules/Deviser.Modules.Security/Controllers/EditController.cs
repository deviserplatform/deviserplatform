using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Deviser.Modules.Security.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Modules.Security.Controllers
{
    [Module("Security")]
    public class EditController : DeviserController
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(TestViewModel model, string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                ViewBag.Result = $"You've entered Field1: {model.Field1} Field2: {model.Field2}";
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

    }
}
