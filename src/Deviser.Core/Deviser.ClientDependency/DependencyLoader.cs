using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.ClientDependency
{
    public class DependencyLoader
    {
        public HashSet<DependencyFile> DependencyFiles { get; internal set; }

        internal DependencyLoader()
        {
            DependencyFiles = new HashSet<DependencyFile>();
        }
    }
}
