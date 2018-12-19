using Deviser.Core.Common.DomainTypes.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Deviser.Admin.Builders
{
    public class FieldBuilder<TEntity>
        where TEntity : class
    {
        private IFieldConfig _fieldConfig;
        public FieldBuilder(IFieldConfig fieldConfig)
        {
            _fieldConfig = fieldConfig;
        }

        public FieldBuilder<TEntity> AddField<TProperty>(Expression<Func<TEntity, TProperty>> expression, Action<FieldOption> fieldOptionAction = null)
        {
            return AddField(_fieldConfig, expression, fieldOptionAction);
        }

        public FieldBuilder<TEntity> AddInlineField<TProperty>(Expression<Func<TEntity, TProperty>> expression, Action<FieldOption> fieldOptionAction = null)
        {
            return AddInlineField(_fieldConfig, expression, fieldOptionAction);
        }

        public FieldBuilder<TEntity> RemoveField<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            _fieldConfig.ExcludedFields.Add(new Field
            {
                FieldExpression = expression
            });

            return this;
        }

        private FieldBuilder<TEntity> AddField<TProperty>(IFieldConfig fieldConfig, Expression<Func<TEntity, TProperty>> expression, Action<FieldOption> fieldOptionAction = null)
        {
            FieldOption fieldOption = new FieldOption();
            fieldOptionAction?.Invoke(fieldOption);
            fieldConfig.Fields.Add(new List<Field>() {
                new Field
                {
                    FieldExpression = expression,
                    FieldOption = fieldOption
                }
            });
            return this;
        }

        private FieldBuilder<TEntity> AddInlineField<TProperty>(IFieldConfig fieldConfig, Expression<Func<TEntity, TProperty>> expression, Action<FieldOption> fieldOptionAction = null)
        {

            FieldOption fieldOption = new FieldOption();
            fieldOptionAction?.Invoke(fieldOption);

            if (fieldConfig.Fields.Count == 0)
                fieldConfig.Fields.Add(new List<Field>());

            var FieldRow = fieldConfig.Fields.Last();

            FieldRow.Add(new Field
            {
                FieldExpression = expression
            });

            return this;
        }
    }
}
