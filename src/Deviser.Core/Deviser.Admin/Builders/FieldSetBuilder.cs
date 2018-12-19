using Deviser.Core.Common.DomainTypes.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Builders
{
    public class FieldSetBuilder<TEntity>
        where TEntity : class
    {
        private IFieldSetConfig _fieldSetConfig;
        public FieldSetBuilder(IFieldSetConfig fieldSetConfig)
        {
            _fieldSetConfig = fieldSetConfig;
        }

        public FieldSetBuilder<TEntity> AddFieldSet(string groupName,
            Func<FieldBuilder<TEntity>, FieldBuilder<TEntity>> fieldBuilderAction, string cssClass = null, string description = null)
        {
            var fieldConfig = new FieldConfig<TEntity>();
            var fieldBuilder = new FieldBuilder<TEntity>(fieldConfig);

            fieldBuilderAction.Invoke(fieldBuilder);

            _fieldSetConfig.FieldSets.Add(new FieldSet
            {
                GroupName = groupName,
                CssClasses = cssClass,
                Description = description,
                Fields = fieldConfig.Fields
            });

            return this;
        }
    }
}
