using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Library.Modules;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Core.Library.Controllers
{
    public class ModuleController : DeviserController
    {
        private ModuleContext _moduleContext;

        public ModuleContext ModuleContext
        {
            get => _moduleContext ??= ScoperService.ModuleContext;
            set => _moduleContext = value;
        }
    }
}
