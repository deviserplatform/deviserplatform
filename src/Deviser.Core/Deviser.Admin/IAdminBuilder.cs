using AutoMapper;
using Deviser.Admin.Builders;
using Deviser.Admin.Config;
using System;
using Deviser.Admin.Services;

namespace Deviser.Admin
{
    public interface IAdminBuilder
    {
        MapperConfiguration MapperConfiguration { get; set; }
        AdminBuilder RegisterGrid<TModel, TAdminGridService>(Action<GridBuilder<TModel>> gridBuilderAction = null)
            where TModel : class
            where TAdminGridService : IAdminGridService<TModel>;
        AdminBuilder Register<TModel>(Action<ModelBuilder<TModel>> modelBuilderAction = null) where TModel : class;
        AdminBuilder Register<TModel, TAdminService>(Action<ModelBuilder<TModel>> modelBuilderAction = null)
           where TModel : class
           where TAdminService : IAdminService<TModel>;
        AdminBuilder RegisterTreeAndForm<TModel, TAdminService>(Action<ModelBuilder<TModel>> modelBuilderAction = null)
            where TModel : class
            where TAdminService : IAdminTreeService<TModel>;
        AdminBuilder RegisterForm<TModel, TAdminService>(Action<FormBuilder<TModel>> formBuilderAction = null)
            where TModel : class
            where TAdminService : IAdminFormService<TModel>;
    }
}