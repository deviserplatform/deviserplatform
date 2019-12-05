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
        private IModelConfig _modelConfig;

        public FieldSetBuilder(IModelConfig modelConfig)
        {
            _modelConfig = modelConfig;
        }

        public FieldSetBuilder<TEntity> AddFieldSet(string groupName,
            Func<FieldBuilder<TEntity>, FieldBuilder<TEntity>> fieldBuilderAction, string cssClass = null, string description = null)
        {
            if (_modelConfig.FormConfig.FieldConfig.ExcludedFields.Count > 0)
                throw new InvalidOperationException(Resources.AddRemoveInvalidOperation);

            var fieldConfig = new FieldConfig();
            var fieldBuilder = new FieldBuilder<TEntity>(_modelConfig);

            fieldBuilderAction.Invoke(fieldBuilder);

            var fieldSet = new FieldSet
            {
                GroupName = groupName,
                CssClasses = cssClass,
                Description = description,
                Fields = fieldConfig.Fields
            };

            _modelConfig.FormConfig.FieldSetConfig.FieldSets.Add(fieldSet);

            return this;
        }
    }
}
