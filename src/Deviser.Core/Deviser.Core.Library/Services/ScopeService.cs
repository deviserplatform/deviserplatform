using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Deviser.Core.Library.Sites;

namespace Deviser.Core.Library.Services
{
    public class ScopeService : IScopeService
    {
        public ScopeService()
        {
            AppContext AppContext = new AppContext();
        }
        public AppContext AppContext { get; set; }
    }
}
