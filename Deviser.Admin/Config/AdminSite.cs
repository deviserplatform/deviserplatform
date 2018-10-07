using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config
{
    public class AdminSite : IAdminSite
    {
        public void Register<TEntity>(Action<AdminConfig<TEntity>> adminConfig = null) where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}
