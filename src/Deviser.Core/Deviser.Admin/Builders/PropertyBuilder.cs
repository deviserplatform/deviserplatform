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

        public PropertyBuilder<TModel> HasLookup<TRelatedModel, TProperty>(
            Expression<Func<IServiceProvider, IList<TRelatedModel>>> lookupExpression,
            Expression<Func<TRelatedModel, TProperty>> lookUpKeyExpression,
            Expression<Func<TRelatedModel, string>> lookupDisplayExpression)
            where TRelatedModel : class
        {
            _field.FieldOption.LookupExpression = lookupExpression;
            _field.FieldOption.LookupKeyExpression = lookUpKeyExpression;
            _field.FieldOption.LookupDisplayExpression = lookupDisplayExpression;
            return this;
        }

        public PropertyBuilder<TModel> HasLookup<TRelatedModel, TProperty, TFilterProperty>(
            Expression<Func<IServiceProvider, TFilterProperty, IList<TRelatedModel>>> lookupExpression,
            Expression<Func<TRelatedModel, TProperty>> lookUpKeyExpression,
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
