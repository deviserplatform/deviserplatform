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
        private readonly FieldConditions _fieldConditions;
        private readonly LambdaExpression _fieldExpression;
        private readonly Field _field;

        public PropertyBuilder(IFormConfig formConfig, LambdaExpression fieldExpression)
        {
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

        public PropertyBuilder<TModel> HasLookup<TRelatedEntity, TProperty>(
            Expression<Func<IServiceProvider, IList<TRelatedEntity>>> lookupExpression,
            Expression<Func<TRelatedEntity, TProperty>> lookUpKeyExpression,
            Expression<Func<TRelatedEntity, string>> lookupDisplayExpression)
            where TRelatedEntity : class
        {
            _field.FieldOption.RelatedModelLookupExpression = lookupExpression;
            _field.FieldOption.RelatedModelLookupKeyExpression = lookUpKeyExpression;
            _field.FieldOption.RelatedModelDisplayExpression = lookupDisplayExpression;
            return this;
        }



        //public AdminConfig<TEntity> AdminConfig { get; set; }
        //public LambdaExpression FieldExpression { get; set; }
    }
}
