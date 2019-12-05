using Deviser.Core.Common.DomainTypes.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Deviser.Admin.Extensions
{
    public static class AdminConfigExtenstions
    {
        //public static FieldConfig<TEntity> AddInlineField<TEntity, TProperty>(this FieldConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression)
        //    where TEntity : class
        //{
        //    return AddInlineField(config, expression, null);
        //}

        //public static FieldConfig<TEntity> AddInlineField<TEntity, TProperty>(this FieldConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression, FieldOption fieldOption)
        //    where TEntity : class
        //{
        //    if (config.Fields.Count == 0)
        //        config.Fields.Add(new List<Field>());

        //    var FieldRow = config.Fields.Last();

        //    FieldRow.Add(new Field
        //    {
        //        FieldExpression = expression
        //    });

        //    return config;
        //}

        //public static FieldConfig<TEntity> AddField<TEntity, TProperty>(this FieldConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression)
        //    where TEntity : class
        //{
        //    return AddField(config, expression, null);
        //}

        //public static FieldConfig<TEntity> AddField<TEntity, TProperty>(this FieldConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression, FieldOption fieldOption)
        //    where TEntity : class
        //{

        //    config.Fields.Add(new List<Field>() {
        //        new Field
        //        {
        //            FieldExpression = expression,
        //            FieldOption = fieldOption
        //        }
        //    });


        //    return config;
        //}

        //public static FieldConfig<TEntity> RemoveField<TEntity, TProperty>(this FieldConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression)
        //    where TEntity : class
        //{
        //    config.ExcludedFields.Add(new Field
        //    {
        //        FieldExpression = expression
        //    });

        //    return config;
        //}

        //public static FieldSetConfig<TEntity> AddFieldSet<TEntity>(this FieldSetConfig<TEntity> config, string groupName,
        //    Expression<Func<FieldConfig<TEntity>, FieldConfig<TEntity>>> expression, string cssClass = null, string description = null)
        //   where TEntity : class
        //{
        //    var fieldConfig = new FieldConfig<TEntity>();
        //    var func = expression.Compile();
        //    fieldConfig = func(fieldConfig);

        //    config.FieldSets.Add(new FieldSet
        //    {
        //        GroupName = groupName,
        //        CssClasses = cssClass,
        //        Description = description,
        //        Fields = fieldConfig.Fields
        //    });

        //    return config;
        //}

        //public static GridConfig<TEntity> AddField<TEntity, TProperty>(this GridConfig<TEntity> config, Expression<Func<TEntity, TProperty>> expression)
        //   where TEntity : class
        //{
        //    config.Fields.Add(new Field
        //    {
        //        FieldExpression = expression

        //    });
        //    return config;
        //}

        

        //public static PropertyBuilder<TEntity> ShowOn<TEntity>(this PropertyBuilder<TEntity> propertyBuilder, Expression<Func<TEntity, bool>> predicate)
        //    where TEntity : class
        //{
        //    propertyBuilder.AdminConfig.FieldConditions.ShowOnConditions.Add(new FieldCondition
        //    {
        //        ConditionExpression = predicate,
        //        FieldExpression = propertyBuilder.FieldExpression
        //    });
        //    return propertyBuilder;
        //}

        //public static PropertyBuilder<TEntity> EnableOn<TEntity>(this PropertyBuilder<TEntity> propertyBuilder, Expression<Func<TEntity, bool>> predicate)
        //    where TEntity : class
        //{
        //    propertyBuilder.AdminConfig.FieldConditions.EnableOnConditions.Add(new FieldCondition
        //    {
        //        ConditionExpression = predicate,
        //        FieldExpression = propertyBuilder.FieldExpression
        //    });
        //    return propertyBuilder;
        //}

        //public static PropertyBuilder<TEntity> ValidateOn<TEntity>(this PropertyBuilder<TEntity> propertyBuilder, Expression<Func<TEntity, bool>> predicate)
        //    where TEntity : class
        //{
        //    propertyBuilder.AdminConfig.FieldConditions.ValidateOnConditions.Add(new FieldCondition
        //    {
        //        ConditionExpression = predicate,
        //        FieldExpression = propertyBuilder.FieldExpression
        //    });
        //    return propertyBuilder;
        //}

        //public static AdminConfig<TEntity> AddChildConfig<TEntity, TProperty>(this AdminConfig<TEntity> adminConfig,
        //    Expression<Func<TEntity, TProperty>> expression,
        //    Action<AdminConfig<TProperty>> childConfigBuilder)
        //    where TEntity : class
        //    where TProperty : class
        //{
        //    AdminConfig<TProperty> childConfig = new AdminConfig<TProperty>();
        //    childConfigBuilder(childConfig);
        //    adminConfig.ChildConfigs.Add(childConfig);
        //    return adminConfig;
        //}
    }
}
