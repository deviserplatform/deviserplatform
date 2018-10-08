using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config
{
    public interface IAdminSiteProvider
    {
        AdminConfig<TEntity> GetAdminConfig<TEntity>()
            where TEntity : class;
    }
}
