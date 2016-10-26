using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Internal
{
    public interface ITypeActivatorCache
    {
        TInstance CreateInstance<TInstance>(IServiceProvider serviceProvider, Type optionType);
    }
}
