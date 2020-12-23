using AutoMapper;
using Deviser.Admin.Builders;
using Deviser.Admin.Config;
using System;
using Deviser.Admin.Services;

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

        public AdminBuilder RegisterGrid<TModel, TAdminGridService>(Action<GridBuilder<TModel>> gridBuilderAction = null)
            where TModel : class
            where TAdminGridService : IAdminGridService<TModel>
        {
            var adminConfig = new AdminConfig<TModel>()
            {
                AdminConfigType = AdminConfigType.GridOnly,
                AdminServiceType = typeof(TAdminGridService)
            };
            BuildAdmin(adminConfig, gridBuilderAction);
            return this;
        }

        public AdminBuilder Register<TModel>(Action<ModelBuilder<TModel>> modelBuilderAction = null)
            where TModel : class
        {
            var adminConfig = new AdminConfig<TModel>()
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

        public AdminBuilder RegisterTreeAndForm<TModel, TAdminService>(Action<ModelBuilder<TModel>> modelBuilderAction = null)
            where TModel : class
            where TAdminService : IAdminTreeService<TModel>
        {
            var adminConfig = new AdminConfig<TModel>
            {
                AdminConfigType = AdminConfigType.TreeAndForm,
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
            BuildAdmin(adminConfig, formBuilderAction);
            return this;
        }

        private void BuildAdmin<TModel>(AdminConfig<TModel> adminConfig, Action<ModelBuilder<TModel>> modelBuilderAction) where TModel : class
        {
            var hasConfiguration = modelBuilderAction != null;
            _adminSite.Mapper = MapperConfiguration?.CreateMapper();

            modelBuilderAction?.Invoke(new ModelBuilder<TModel>(adminConfig));

            _adminSite.Build(adminConfig, hasConfiguration);
        }

        private void BuildAdmin<TModel>(AdminConfig<TModel> adminConfig, Action<FormBuilder<TModel>> formBuilderAction) where TModel : class
        {
            var hasConfiguration = formBuilderAction != null;
            _adminSite.Mapper = MapperConfiguration?.CreateMapper();

            formBuilderAction?.Invoke(new FormBuilder<TModel>(adminConfig.ModelConfig.FormConfig, adminConfig.ModelConfig.KeyField));

            _adminSite.Build(adminConfig, hasConfiguration);
        }

        private void BuildAdmin<TModel>(AdminConfig<TModel> adminConfig, Action<GridBuilder<TModel>> gridBuilderAction) where TModel : class
        {
            var hasConfiguration = gridBuilderAction != null;
            _adminSite.Mapper = MapperConfiguration?.CreateMapper();

            gridBuilderAction?.Invoke(new GridBuilder<TModel>(adminConfig.ModelConfig));

            _adminSite.Build(adminConfig, hasConfiguration);
        }
    }
}
