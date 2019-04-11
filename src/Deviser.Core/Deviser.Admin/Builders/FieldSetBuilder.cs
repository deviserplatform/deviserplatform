using Deviser.Admin.Properties;
using Deviser.Core.Common.DomainTypes.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Builders
{
    public class FieldSetBuilder<TEntity>
        where TEntity : class
    {   
        private IFormConfig _formConfig;

        public FieldSetBuilder(IFormConfig formConfig)
        {
            _formConfig = formConfig;
        }

        public FieldSetBuilder<TEntity> AddFieldSet(string groupName,
            Func<FieldBuilder<TEntity>, FieldBuilder<TEntity>> fieldBuilderAction, string cssClass = null, string description = null)
        {
            if (_formConfig.FieldConfig.ExcludedFields.Count > 0)
                throw new InvalidOperationException(Resources.AddRemoveInvalidOperation);

            var fieldConfig = new FieldConfig<TEntity>();
            var fieldBuilder = new FieldBuilder<TEntity>(_formConfig);

            fieldBuilderAction.Invoke(fieldBuilder);

            var fieldSet = new FieldSet
            {
                GroupName = groupName,
                CssClasses = cssClass,
                Description = description,
                Fields = fieldConfig.Fields
            };

            _formConfig.FieldSetConfig.FieldSets.Add(fieldSet);

            return this;
        }
    }
}
