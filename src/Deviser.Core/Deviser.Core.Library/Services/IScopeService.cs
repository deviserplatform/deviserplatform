using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Services
{
    public class IScopeService
    {
        public PageContext PageContext { get; set; }

        public ModuleContext ModuleContext { get; set; }
    }
}
