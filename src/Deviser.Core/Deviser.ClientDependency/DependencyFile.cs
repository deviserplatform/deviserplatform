using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.ClientDependency
{
    public class DependencyFile
    {
        public string FilePath { get; set; }
        public DependencyType DependencyType { get; set; }
        public int Priority { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}
