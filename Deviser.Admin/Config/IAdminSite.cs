using System;

namespace Deviser.Admin
{
    public interface IAdminSite
    {
        void Register<TEntity>(Action<AdminConfig<TEntity>> adminConfig=null)
            where TEntity : class;
    }
}