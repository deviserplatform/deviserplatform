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
        void Build<TEntity>(AdminConfig<TEntity> adminConfig, bool hasConfiguration = false)
            where TEntity : class;
    }
}