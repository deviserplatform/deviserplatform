using Deviser.Admin.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Deviser.Admin.Properties;
using Deviser.Core.Common.Extensions;
using Microsoft.DotNet.Scaffolding.Shared.Project;

namespace Deviser.Admin.Builders
{
    public class ModelBuilder<TModel>
        where TModel : class
    {
        private readonly IAdminConfig _adminConfig;
        public FormBuilder<TModel> FormBuilder { get; }
        public GridBuilder<TModel> GridBuilder { get; }

        public TreeBuilder<TModel> TreeBuilder { get; }

        public string AdminTitle
        {
            get => _adminConfig.AdminTitle;
            set => _adminConfig.AdminTitle = value;
        }

        public ModelBuilder(IAdminConfig adminConfig)
        {
            _adminConfig = adminConfig;
            FormBuilder = new FormBuilder<TModel>(_adminConfig.ModelConfig.FormConfig, _adminConfig.ModelConfig.KeyField);
            GridBuilder = new GridBuilder<TModel>(_adminConfig.ModelConfig);
            TreeBuilder = new TreeBuilder<TModel>(_adminConfig.ModelConfig);
        }

        public ModelBuilder<TModel> AddChildConfig<TProperty>(Expression<Func<TModel, ICollection<TProperty>>> expression,
            Action<ModelBuilder<TProperty>> childFormBuilderAction)
            where TProperty : class
        {
            var childConfig = new AdminConfig<TProperty>();
            var childFormBuilder = new ModelBuilder<TProperty>(childConfig);
            childFormBuilderAction(childFormBuilder);
            _adminConfig.ChildConfigs.Add(new ChildConfig(new Field
            {
                FieldExpression = expression
            }, childConfig.ModelConfig, typeof(TProperty)));
            return this;
        }

        /// <summary>
        /// This is a static configuration used to show/hide child grid and form based on info from any "Service(s)".
        /// Note: If dynamic show/hide child grid and form is required, a separate method can be implemented.
        /// This will be evaluvated when admin config / meta info is requested from client.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="fieldExpression"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public ModelBuilder<TModel> ShowChildConfigOn<TProperty>(Expression<Func<TModel, ICollection<TProperty>>> fieldExpression, Expression<Func<IServiceProvider, bool>> predicate)
        {
            var fieldName = ReflectionExtensions.GetMemberName(fieldExpression);
            var childConfig = _adminConfig.ChildConfigs.FirstOrDefault(c => c.Field.FieldName == fieldName);
            if (childConfig == null)
            {
                throw new InvalidOperationException(Resources.FieldNotFoundInvaidOperation);
            }

            childConfig.ShowOnStaticExpression = predicate;

            return this;
        }

        public ModelBuilder<TModel> AddCustomForm<TCustomModel>(string formName, Action<CustomFormBuilder<TCustomModel>> formBuilderAction)
            where TCustomModel : class
        {
            var customForm = new CustomForm()
            {
                FormName = formName,
                ModelType = typeof(TCustomModel)
            };
            formBuilderAction.Invoke(new CustomFormBuilder<TCustomModel>(customForm));
            _adminConfig.ModelConfig.CustomForms.Add(formName, customForm);
            return this;
        }
    }
}
