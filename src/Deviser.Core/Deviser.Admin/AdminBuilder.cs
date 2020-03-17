using AutoMapper;
using Deviser.Admin.Builders;
using Deviser.Admin.Config;
using System;

namespace Deviser.Admin
{
    public class AdminBuilder : IAdminBuilder
    {
        private readonly IAdminSite _adminSite;

        public AdminBuilder(IAdminSite adminSite)
        {
            _adminSite = adminSite;
        }

        public MapperConfiguration MapperConfiguration { get; set; }

        public AdminBuilder Register<TEntity>(Action<ModelBuilder<TEntity>> modelBuilderAction = null)
            where TEntity : class
        {
            var adminConfig = new AdminConfig<TEntity>()
            {
                AdminConfigType = AdminConfigType.GridAndForm
            };
            BuildAdmin(adminConfig, modelBuilderAction);
            return this;
        }



        public AdminBuilder Register<TModel, TAdminService>(Action<ModelBuilder<TModel>> modelBuilderAction = null)
            where TModel : class
            where TAdminService : IAdminService<TModel>
        {
            var adminConfig = new AdminConfig<TModel>
            {
                AdminConfigType = AdminConfigType.GridAndForm,
                AdminServiceType = typeof(TAdminService)
            };
            BuildAdmin(adminConfig, modelBuilderAction);
            return this;
        }

        public AdminBuilder RegisterForm<TModel, TAdminService>(Action<FormBuilder<TModel>> formBuilderAction = null)
            where TModel : class where TAdminService : IAdminFormService<TModel>
        {
            var adminConfig = new AdminConfig<TModel>()
            {
                AdminConfigType = AdminConfigType.FormOnly,
                AdminServiceType = typeof(TAdminService)
            };
            return this;
        }

        private void BuildAdmin<TModel>(AdminConfig<TModel> adminConfig, Action<ModelBuilder<TModel>> modelBuilderAction) where TModel : class
        {
            var hasConfiguration = modelBuilderAction != null;
            _adminSite.Mapper = MapperConfiguration?.CreateMapper();

            modelBuilderAction?.Invoke(new ModelBuilder<TModel>(adminConfig));

            _adminSite.Build(adminConfig, hasConfiguration);
        }
    }
}
