using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Admin.Web.Controllers;
using Deviser.Core.Library.Modules;

namespace Deviser.Modules.ContentManagement.Controllers
{
    [Module("ContentManagement")]
    public class AdminController : AdminController<AdminConfigurator>
    {
        public AdminController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }
    }
}
