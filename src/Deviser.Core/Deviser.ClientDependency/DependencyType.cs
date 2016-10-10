using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.ClientDependency
{
    public enum DependencyType
    {
        Script, Css
    }

    public enum ScriptLocation
    {
        BodyEnd = 0, BodyStart = 1, Head = 2
    }
}
