using Deviser.Core.Library;
using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.ViewComponents
{
    [ViewComponent(Name = "ControlBar")]
    public class ControlBar : DeviserViewComponent
    {
        public ControlBar(IScopeService scopeService)
            :base(scopeService)
        {

        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {            
            return View();
        }
    }
}
