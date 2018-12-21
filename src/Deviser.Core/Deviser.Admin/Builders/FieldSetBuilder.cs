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
        private IFieldSetConfig _fieldSetConfig;
        private IAdminConfig _adminConfig;

        public FieldSetBuilder(IFieldSetConfig fieldSetConfig, IAdminConfig adminConfig)
        {
            _fieldSetConfig = fieldSetConfig;
            _adminConfig = adminConfig;
        }

        public FieldSetBuilder<TEntity> AddFieldSet(string groupName,
            Func<FieldBuilder<TEntity>, FieldBuilder<TEntity>> fieldBuilderAction, string cssClass = null, string description = null)
        {
            if (_adminConfig.FieldConfig.ExcludedFields.Count > 0)
                throw new InvalidOperationException(Resources.AddRemoveInvalidOperation);

            var fieldConfig = new FieldConfig<TEntity>();
            var fieldBuilder = new FieldBuilder<TEntity>(fieldConfig, _adminConfig);

            fieldBuilderAction.Invoke(fieldBuilder);

            var fieldSet = new FieldSet
            {
                GroupName = groupName,
                CssClasses = cssClass,
                Description = description,
                Fields = fieldConfig.Fields
            };

            _fieldSetConfig.FieldSets.Add(fieldSet);

            return this;
        }
    }
}
