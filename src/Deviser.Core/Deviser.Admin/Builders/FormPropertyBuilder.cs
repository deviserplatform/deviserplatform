using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.Extensions;

namespace Deviser.Admin.Builders
{
    public class FormPropertyBuilder<TModel, TProperty> : PropertyBuilder<TModel, TProperty> where TModel : class

    {
        private readonly FieldConditions _fieldConditions;
        private readonly Field _field;
        private readonly LambdaExpression _fieldExpression;

        public FormPropertyBuilder(IFormConfig formConfig, LambdaExpression fieldExpression)
            : base(formConfig.AllFields, fieldExpression)
        {
            _fieldConditions = formConfig.FieldConditions;
            _fieldExpression = fieldExpression;
            var fieldName = ReflectionExtensions.GetMemberName(fieldExpression);
            _field = formConfig.AllFields.FirstOrDefault(f => f.FieldName == fieldName);
        }

        /// <summary>
        /// On enabling this field new item(s) can be added in multi select field. Applicable only for multi select field.
        /// </summary>
        /// <returns></returns>
        public FormPropertyBuilder<TModel, TProperty> AddItemBy(Expression<Func<TProperty, string>> expression)
        {
            _field.FieldOption.AddItemBy = new Field()
            {
                FieldExpression = expression
            };
            return this;
        }

        public FormPropertyBuilder<TModel, TProperty> AutoFillBasedOn<TPropertyField>(Expression<Func<TModel, TPropertyField>> fieldExpression, 
            Expression<Func<IServiceProvider, TPropertyField, Task<TProperty>>> expression)
        {
            _field.FieldOption.AutoFillField = new Field()
            {
                FieldExpression = fieldExpression
            };
            _field.FieldOption.AutoFillExpression = expression;
            return this;
        }

        public FormPropertyBuilder<TModel, TProperty> CalculateWith<T1>(Expression<Func<TModel, T1>> fieldSelection,
            Expression<Func<T1, TProperty>> expression)
        {
            var selectProperties = ExpressionHelper.GetProperties(fieldSelection.Body.Type);
            foreach (var selectProperty in selectProperties)
            {
                _field.FieldOption.CalculateSelectedFields.Add(new Field()
                {
                    FieldExpression = ExpressionHelper.GetPropertyExpression(typeof(TModel), selectProperty.Name)
                });
            }
            
            _field.FieldOption.CalculateExpression = expression;
            return this;
        }

        //public FormPropertyBuilder<TModel, TProperty> CalculateWith<T1, T2>(
        //    (Expression<Func<TModel, T1>> fieldExpression1,
        //    Expression<Func<TModel, T2>> fieldExpression2) paramTuple,
        //    Expression<Func<IServiceProvider, T1, T2, Task<TProperty>>> expression)
        //{
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression1
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression2
        //    });
        //    _field.FieldOption.CalculateExpression = expression;
        //    return this;
        //}

        //public FormPropertyBuilder<TModel, TProperty> CalculateWith<T1, T2, T3>(
        //    (Expression<Func<TModel, T1>> fieldExpression1,
        //    Expression<Func<TModel, T2>> fieldExpression2,
        //    Expression<Func<TModel, T3>> fieldExpression3) paramTuple,
        //    Expression<Func<IServiceProvider, T1, T2, T3, Task<TProperty>>> expression)
        //{
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression1
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression2
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression3
        //    });
        //    _field.FieldOption.CalculateExpression = expression;
        //    return this;
        //}

        //public FormPropertyBuilder<TModel, TProperty> CalculateWith<T1, T2, T3, T4>(
        //    (Expression<Func<TModel, T1>> fieldExpression1,
        //    Expression<Func<TModel, T2>> fieldExpression2,
        //    Expression<Func<TModel, T3>> fieldExpression3,
        //    Expression<Func<TModel, T4>> fieldExpression4) paramTuple,
        //    Expression<Func<IServiceProvider, T1, T2, T3, T4, Task<TProperty>>> expression)
        //{
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression1
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression2
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression3
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression4
        //    });
        //    _field.FieldOption.CalculateExpression = expression;
        //    return this;
        //}

        //public FormPropertyBuilder<TModel, TProperty> CalculateWith<T1, T2, T3, T4, T5>(
        //    (Expression<Func<TModel, T1>> fieldExpression1,
        //    Expression<Func<TModel, T2>> fieldExpression2,
        //    Expression<Func<TModel, T3>> fieldExpression3,
        //    Expression<Func<TModel, T4>> fieldExpression4,
        //    Expression<Func<TModel, T5>> fieldExpression5) paramTuple,
        //    Expression<Func<IServiceProvider, T1, T2, T3, T4, T5, Task<TProperty>>> expression)
        //{
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression1
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression2
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression3
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression4
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression5
        //    });
        //    _field.FieldOption.CalculateExpression = expression;
        //    return this;
        //}

        //public FormPropertyBuilder<TModel, TProperty> CalculateWith<T1, T2, T3, T4, T5, T6>(
        //    (Expression<Func<TModel, T1>> fieldExpression1,
        //    Expression<Func<TModel, T2>> fieldExpression2,
        //    Expression<Func<TModel, T3>> fieldExpression3,
        //    Expression<Func<TModel, T4>> fieldExpression4,
        //    Expression<Func<TModel, T5>> fieldExpression5,
        //    Expression<Func<TModel, T6>> fieldExpression6) paramTuple,
        //    Expression<Func<IServiceProvider, T1, T2, T3, T4, T5, T6, Task<TProperty>>> expression)
        //{
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression1
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression2
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression3
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression4
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression5
        //    });
        //    _field.FieldOption.CalculateFields.Add(new Field()
        //    {
        //        FieldExpression = paramTuple.fieldExpression6
        //    });
        //    _field.FieldOption.CalculateExpression = expression;
        //    return this;
        //}

        public FormPropertyBuilder<TModel, TProperty> ShowOn(Expression<Func<TModel, bool>> predicate)
        {
            _fieldConditions.ShowOnConditions.Add(new FieldCondition
            {
                ConditionExpression = predicate,
                FieldExpression = _fieldExpression
            });
            return this;
        }

        public FormPropertyBuilder<TModel, TProperty> EnableOn(Expression<Func<TModel, bool>> predicate)

        {
            _fieldConditions.EnableOnConditions.Add(new FieldCondition
            {
                ConditionExpression = predicate,
                FieldExpression = _fieldExpression
            });
            return this;
        }

        public FormPropertyBuilder<TModel, TProperty> ValidateOn(Expression<Func<TModel, bool>> predicate)
        {
            _fieldConditions.ValidateOnConditions.Add(new FieldCondition
            {
                ConditionExpression = predicate,
                FieldExpression = _fieldExpression
            });
            return this;
        }
    }
}
