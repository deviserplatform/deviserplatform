using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Admin.Web.Controllers;
using Deviser.Core.Library.Modules;

namespace Deviser.Modules.SecurityRoles.Controllers
{
    [Module("SecurityRoles")]
    public class AdminController : AdminController<AdminConfigurator>
    {
        public AdminController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }
    }
}
