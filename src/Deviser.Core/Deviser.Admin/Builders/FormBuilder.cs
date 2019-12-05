using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Deviser.Admin.Builders
{
    public class FormBuilder<TEntity>
        where TEntity : class
    {
        private IAdminConfig _adminConfig;
        public FieldBuilder<TEntity> Fields { get; }
        public FieldSetBuilder<TEntity> FieldSetBuilder { get; }

        public FormBuilder(IAdminConfig adminConfig)
        {
            _adminConfig = adminConfig;
            Fields = new FieldBuilder<TEntity>(_adminConfig.ModelConfig);
            FieldSetBuilder = new FieldSetBuilder<TEntity>(_adminConfig.ModelConfig);
        }

        public PropertyBuilder<TEntity> Property<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            //var field = adminConfig?.FieldConfig?.Fields?.FirstOrDefault(f => f.)
            return new PropertyBuilder<TEntity>(_adminConfig, expression);
        }

        public FormBuilder<TEntity> AddChildConfig<TProperty>(Expression<Func<TEntity, ICollection<TProperty>>> expression,
            Action<FormBuilder<TProperty>> childFormBuilderAction)
            where TProperty : class
        {
            AdminConfig<TProperty> childConfig = new AdminConfig<TProperty>();
            var childFormBuilder = new FormBuilder<TProperty>(childConfig);
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
    }
}
