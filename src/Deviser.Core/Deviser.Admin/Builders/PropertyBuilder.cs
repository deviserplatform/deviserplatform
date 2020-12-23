using Deviser.Admin.Config;
using Deviser.Admin.Properties;
using Deviser.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Deviser.Admin.Builders
{
    public class PropertyBuilder<TModel, TProperty>
        where TModel : class
    {
        private readonly IFormConfig _formConfig;
        private readonly FieldConditions _fieldConditions;
        private readonly LambdaExpression _fieldExpression;
        private readonly Field _field;

        public PropertyBuilder(IFormConfig formConfig, LambdaExpression fieldExpression)
        {
            _formConfig = formConfig;
            _fieldConditions = formConfig.FieldConditions;
            _fieldExpression = fieldExpression;
            var fieldName = ReflectionExtensions.GetMemberName(fieldExpression);
            _field = formConfig.AllFormFields.FirstOrDefault(f => f.FieldName == fieldName);

            if (_field == null)
            {
                throw new InvalidOperationException(Resources.FieldNotFoundInvaidOperation);
            }
        }

        /// <summary>
        /// On enabling this field new item(s) can be added in multi select field. Applicable only for multi select field.
        /// </summary>
        /// <returns></returns>
        public PropertyBuilder<TModel, TProperty> AddItemBy(Expression<Func<TProperty, string>> expression)
        {
            _field.FieldOption.AddItemBy = new Field()
            {
                FieldExpression = expression
            };
            return this;
        }

        public PropertyBuilder<TModel, TProperty> AutoFillBasedOn<TPropertyField>(Expression<Func<TModel, TPropertyField>> fieldExpression, Expression<Func<IServiceProvider, TProperty, Task<TProperty>>> expression)
        {
            _field.FieldOption.AutoFillField = new Field()
            {
                FieldExpression = fieldExpression
            };
            _field.FieldOption.AutoFillExpression = expression;
            return this;
        }

    public PropertyBuilder<TModel, TProperty> ShowOn(Expression<Func<TModel, bool>> predicate)
    {
        _fieldConditions.ShowOnConditions.Add(new FieldCondition
        {
            ConditionExpression = predicate,
            FieldExpression = _fieldExpression
        });
        return this;
    }

    public PropertyBuilder<TModel, TProperty> EnableOn(Expression<Func<TModel, bool>> predicate)

    {
        _fieldConditions.EnableOnConditions.Add(new FieldCondition
        {
            ConditionExpression = predicate,
            FieldExpression = _fieldExpression
        });
        return this;
    }

    public PropertyBuilder<TModel, TProperty> ValidateOn(Expression<Func<TModel, bool>> predicate)
    {
        _fieldConditions.ValidateOnConditions.Add(new FieldCondition
        {
            ConditionExpression = predicate,
            FieldExpression = _fieldExpression
        });
        return this;
    }

    public PropertyBuilder<TModel, TProperty> HasLookup<TRelatedModel, TKey>(
        Expression<Func<IServiceProvider, ICollection<TRelatedModel>>> lookupExpression,
        Expression<Func<TRelatedModel, TKey>> lookUpKeyExpression,
        Expression<Func<TRelatedModel, string>> lookupDisplayExpression)
        where TRelatedModel : class
    {
        _field.FieldOption.LookupExpression = lookupExpression;
        _field.FieldOption.LookupKeyExpression = lookUpKeyExpression;
        _field.FieldOption.LookupDisplayExpression = lookupDisplayExpression;
        return this;
    }

    public PropertyBuilder<TModel, TProperty> HasMatrixLookup<TRowType, TColumnType, TKey>(
        Expression<Func<IServiceProvider, ICollection<TRowType>>> rowExpression,
        Expression<Func<TRowType, TKey>> rowKeyExpression,
        Expression<Func<TRowType, string>> rowDisplayExpression,
        Expression<Func<IServiceProvider, ICollection<TColumnType>>> colExpression,
        Expression<Func<TColumnType, TKey>> colKeyExpression,
        Expression<Func<TColumnType, string>> colDisplayExpression)
        where TRowType : class
    {
        _field.FieldOption.CheckBoxMatrix.RowLookupExpression = rowExpression;
        _field.FieldOption.CheckBoxMatrix.RowLookupKeyExpression = rowKeyExpression;
        _field.FieldOption.CheckBoxMatrix.RowLookupDisplayExpression = rowDisplayExpression;
        _field.FieldOption.CheckBoxMatrix.ColLookupExpression = colExpression;
        _field.FieldOption.CheckBoxMatrix.ColLookupKeyExpression = colKeyExpression;
        _field.FieldOption.CheckBoxMatrix.ColLookupDisplayExpression = colDisplayExpression;
        return this;
    }

    public PropertyBuilder<TModel, TProperty> HasLookup<TRelatedModel, TKey, TFilterProperty>(
        Expression<Func<IServiceProvider, TFilterProperty, ICollection<TRelatedModel>>> lookupExpression,
        Expression<Func<TRelatedModel, TKey>> lookUpKeyExpression,
        Expression<Func<TRelatedModel, string>> lookupDisplayExpression,
        Expression<Func<TModel, TFilterProperty>> lookupFilterExpression)
        where TRelatedModel : class
    {
        var filterFieldName = ReflectionExtensions.GetMemberName(lookupFilterExpression);
        var filterField = _formConfig.AllFormFields.FirstOrDefault(f => f.FieldName == filterFieldName);
        if (filterField == null)
        {
            throw new InvalidOperationException(Resources.FieldNotFoundInvaidOperation);
        }

        _field.FieldOption.LookupExpression = lookupExpression;
        _field.FieldOption.LookupKeyExpression = lookUpKeyExpression;
        _field.FieldOption.LookupDisplayExpression = lookupDisplayExpression;
        _field.FieldOption.LookupFilterExpression = lookupFilterExpression;
        _field.FieldOption.LookupFilterField = filterField;
        return this;
    }



    //public AdminConfig<TEntity> AdminConfig { get; set; }
    //public LambdaExpression FieldExpression { get; set; }
}

}
