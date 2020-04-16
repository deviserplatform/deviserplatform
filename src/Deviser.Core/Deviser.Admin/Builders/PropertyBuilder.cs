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
    public class PropertyBuilder<TModel>
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


        public PropertyBuilder<TModel> ShowOn(Expression<Func<TModel, bool>> predicate)
        {
            _fieldConditions.ShowOnConditions.Add(new FieldCondition
            {
                ConditionExpression = predicate,
                FieldExpression = _fieldExpression
            });
            return this;
        }

        public PropertyBuilder<TModel> EnableOn(Expression<Func<TModel, bool>> predicate)

        {
            _fieldConditions.EnableOnConditions.Add(new FieldCondition
            {
                ConditionExpression = predicate,
                FieldExpression = _fieldExpression
            });
            return this;
        }

        public PropertyBuilder<TModel> ValidateOn(Expression<Func<TModel, bool>> predicate)
        {
            _fieldConditions.ValidateOnConditions.Add(new FieldCondition
            {
                ConditionExpression = predicate,
                FieldExpression = _fieldExpression
            });
            return this;
        }

        public PropertyBuilder<TModel> HasLookup<TRelatedModel, TKey>(
            Expression<Func<IServiceProvider, IList<TRelatedModel>>> lookupExpression,
            Expression<Func<TRelatedModel, TKey>> lookUpKeyExpression,
            Expression<Func<TRelatedModel, string>> lookupDisplayExpression)
            where TRelatedModel : class
        {
            _field.FieldOption.LookupExpression = lookupExpression;
            _field.FieldOption.LookupKeyExpression = lookUpKeyExpression;
            _field.FieldOption.LookupDisplayExpression = lookupDisplayExpression;
            return this;
        }

        public PropertyBuilder<TModel> HasMatrixLookup<TRowType, TColumnType, TKey>(
            Expression<Func<IServiceProvider, IList<TRowType>>> rowExpression,
            Expression<Func<TRowType, TKey>> rowKeyExpression,
            Expression<Func<TRowType, string>> rowDisplayExpression,
            Expression<Func<IServiceProvider, IList<TColumnType>>> colExpression,
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

        public PropertyBuilder<TModel> HasLookup<TRelatedModel, TKey, TFilterProperty>(
            Expression<Func<IServiceProvider, TFilterProperty, IList<TRelatedModel>>> lookupExpression,
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
