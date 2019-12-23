using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Deviser.Admin.Config;

namespace Deviser.Admin.Builders
{
    public class ModelBuilder<TModel>
        where TModel : class
    {
        private readonly IAdminConfig _adminConfig;
        public FormBuilder<TModel> FormBuilder { get; }

        public ModelBuilder(IAdminConfig adminConfig)
        {
            _adminConfig = adminConfig;
            FormBuilder = new FormBuilder<TModel>(_adminConfig.ModelConfig.FormConfig, _adminConfig.ModelConfig.KeyField);
        }

        public PropertyBuilder<TModel> Property<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return new PropertyBuilder<TModel>(_adminConfig, expression);
        }

        public ModelBuilder<TModel> AddChildConfig<TProperty>(Expression<Func<TModel, ICollection<TProperty>>> expression,
            Action<ModelBuilder<TProperty>> childFormBuilderAction)
            where TProperty : class
        {
            AdminConfig<TProperty> childConfig = new AdminConfig<TProperty>();
            var childFormBuilder = new ModelBuilder<TProperty>(childConfig);
            childFormBuilderAction(childFormBuilder);
            _adminConfig.ChildConfigs.Add(new ChildConfig
            {
                Field = new Core.Common.DomainTypes.Admin.Field
                {
                    FieldExpression = expression
                },
                ModelConfig = childConfig.ModelConfig
            });
            return this;
        }

        public ModelBuilder<TModel> AddCustomForm<TCustomModel>(string formName, Action<CustomFormBuilder<TCustomModel>> formBuilderAction)
            where TCustomModel : class
        {
            var customForm = new CustomForm();
            formBuilderAction.Invoke(new CustomFormBuilder<TCustomModel>(customForm));
            return this;
        }
    }
}
