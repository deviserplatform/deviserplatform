using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config
{
    public interface IAdminConfigStore
    {
        AdminConfig<TEntity> GetOrAdd(Type type, AdminConfig<TEntity> element);
        bool TryGet(Type type, out AdminConfig<TEntity> element);        
    }
}
