using AutoMapper;
using Deviser.Admin.Config;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Admin
{
    public interface IAdminSite
    {
        IDictionary<Type, IAdminConfig> AdminConfigs { get; }
        AdminType AdminType { get; }
        Type DbContextType { get; }
        IMapper Mapper { get; set; }
        string SiteName { get; set; }
        
        void Build<TEntity>(AdminConfig<TEntity> adminConfig, bool hasConfiguration = false)
            where TEntity : class;

        TypeMap GetTypeMapFor(Type modelType);

        Type GetEntityClrTypeFor(Type modelType);
    }
}