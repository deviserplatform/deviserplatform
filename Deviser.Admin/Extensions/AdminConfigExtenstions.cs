using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Deviser.Admin.Extensions
{
    public static class AdminConfigExtenstions
    {
        public static FieldConfig<TEntity> AddInlineField<TEntity, TProperty>(this FieldConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression)
            where TEntity : class
        {
            return AddInlineField(config, expression, null);
        }

        public static FieldConfig<TEntity> AddInlineField<TEntity, TProperty>(this FieldConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression, FieldOption fieldOption)
            where TEntity : class
        {
            if (config.Fields.Count == 0)
                config.Fields.Add(new List<Field>());

            var FieldRow = config.Fields.Last();

            FieldRow.Add(new Field
            {
                FieldExpression = expression
            });

            return config;
        }

        public static FieldConfig<TEntity> AddField<TEntity, TProperty>(this FieldConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression)
            where TEntity : class
        {
            return AddField(config, expression, null);
        }

        public static FieldConfig<TEntity> AddField<TEntity, TProperty>(this FieldConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression, FieldOption fieldOption)
            where TEntity : class
        {

            config.Fields.Add(new List<Field>() {
                new Field
                {
                    FieldExpression = expression,
                    FieldOption = fieldOption
                }
            });


            return config;
        }

        public static FieldConfig<TEntity> RemoveField<TEntity, TProperty>(this FieldConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression)
            where TEntity : class
        {
            config.ExcludedFields.Add(new Field
            {
                FieldExpression = expression
            });

            return config;
        }

        public static FieldSetConfig<TEntity> AddFieldSet<TEntity>(this FieldSetConfig<TEntity> config, string groupName,
            Expression<Func<FieldConfig<TEntity>, FieldConfig<TEntity>>> expression, string cssClass = null, string description = null)
           where TEntity : class
        {
            var fieldConfig = new FieldConfig<TEntity>();
            var func = expression.Compile();
            fieldConfig = func(fieldConfig);

            config.FieldSets.Add(new FieldSet
            {
                GroupName = groupName,
                CssClasses = cssClass,
                Description = description,
                Fields = fieldConfig.Fields
            });

            return config;
        }

        public static ListConfig<TEntity> AddField<TEntity, TProperty>(this ListConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression)
           where TEntity : class
        {
            config.Fields.Add(new Field
            {
                FieldExpression = expression

            });
            return config;
        }
    }
}
