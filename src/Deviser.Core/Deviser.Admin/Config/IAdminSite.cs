using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Deviser.Admin
{
    public interface IAdminSite
    {
        IDictionary<Type, IAdminConfig> AdminConfigs { get; }
        Type DbContextType { get; }
        IMapper Mapper { get; set; }
        string SiteName { get; set; }
        
        void Build<TEntity>(AdminConfig<TEntity> adminConfig, bool hasConfiguration = false)
            where TEntity : class;
    }
}