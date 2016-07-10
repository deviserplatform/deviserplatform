using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public enum SystemPageFilter
    {
        All,
        PublicOnly,
        SystemOnly
    }

    public enum FileItemType
    {
        Folder = 1,
        File = 2
    }
}