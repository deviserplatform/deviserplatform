using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
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

        public FormPropertyBuilder<TModel, TProperty> AutoFillBasedOn<TPropertyField>(Expression<Func<TModel, TPropertyField>> fieldExpression, Expression<Func<IServiceProvider, TProperty, Task<TProperty>>> expression)
        {
            _field.FieldOption.AutoFillField = new Field()
            {
                FieldExpression = fieldExpression
            };
            _field.FieldOption.AutoFillExpression = expression;
            return this;
        }

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
