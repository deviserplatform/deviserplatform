using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Autofac;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Core.Library.Controllers
{
    public class ModuleController : DeviserController
    {
        public ModuleContext ModuleContext
        {
            get
            {
                return ScoperService.ModuleContext;
            }
        }
    }
}
