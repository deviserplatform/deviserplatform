using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config
{
    public interface IAdminConfigStore
    {
        object GetOrAdd(Type type, object element);
        bool TryGet(Type type, out object element);
    }
}
