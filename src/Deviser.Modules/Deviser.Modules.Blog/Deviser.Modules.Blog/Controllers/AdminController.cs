using Deviser.Admin.Web.Controllers;
using Deviser.Core.Library.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Modules.Blog.Controllers
{
    [Module("Blog")]
    public class AdminController : AdminController<AdminConfigurator>        
    {
        public AdminController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }
    }
}
