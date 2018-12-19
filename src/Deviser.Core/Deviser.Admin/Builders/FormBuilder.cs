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
        public FieldBuilder<TEntity> FieldBuilder { get; }
        public FieldSetBuilder<TEntity> FieldSetBuilder { get; }

        public FormBuilder(IAdminConfig adminConfig)
        {
            _adminConfig = adminConfig;
            FieldBuilder = new FieldBuilder<TEntity>(_adminConfig.FieldConfig);
            FieldSetBuilder = new FieldSetBuilder<TEntity>(_adminConfig.FieldSetConfig);
        }

        public PropertyBuilder<TEntity> Property<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            //var field = adminConfig?.FieldConfig?.Fields?.FirstOrDefault(f => f.)
            return new PropertyBuilder<TEntity>(_adminConfig.FieldConditions, expression);
        }

        public FormBuilder<TEntity> AddChildConfig<TProperty>(Expression<Func<TEntity, TProperty>> expression,
            Action<FormBuilder<TProperty>> childFormBuilderAction)
            where TProperty : class
        {
            AdminConfig<TProperty> childConfig = new AdminConfig<TProperty>();
            var childFormBuilder = new FormBuilder<TProperty>(childConfig);
            childFormBuilderAction(childFormBuilder);
            _adminConfig.ChildConfigs.Add(childConfig);
            return this;
        }
    }
}
