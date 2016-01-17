using Deviser.Core.Library;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.ViewComponents
{
    [ViewComponent(Name = "ControlBar")]
    public class ControlBar : DeviserViewComponent
    {
        public IViewComponentResult Invoke()
        {            
            return View();
        }
    }
}
