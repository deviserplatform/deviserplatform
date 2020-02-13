using AutoMapper;
using Deviser.Admin.Builders;
using Deviser.Admin.Config;
using System;

namespace Deviser.Admin
{
    public interface IAdminBuilder
    {
        MapperConfiguration MapperConfiguration { get; set; }
        AdminBuilder Register<TEntity>(Action<ModelBuilder<TEntity>> modelBuilderAction = null) where TEntity : class;
        AdminBuilder Register<TEntity, TAdminService>(Action<ModelBuilder<TEntity>> modelBuilderAction = null)
           where TEntity : class
           where TAdminService : IAdminService<TEntity>;
    }
}