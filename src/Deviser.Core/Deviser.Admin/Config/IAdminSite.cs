using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Deviser.Admin
{
    public interface IAdminSite
    {
        string SiteName { get; set; }
        Type DbContextType { get; }
        IDictionary<Type, IAdminConfig> AdminConfigs { get; }
        void Register<TEntity>(Action<AdminConfig<TEntity>> adminConfig=null)
            where TEntity : class;
    }
}