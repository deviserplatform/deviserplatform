using Deviser.Core.Common.DomainTypes.Admin;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Deviser.Admin.Builders
{
    public class PropertyBuilder<TEntity>
        where TEntity : class
    {

        private FieldConditions _fieldConditions;
        private LambdaExpression _fieldExpression;

        public PropertyBuilder(FieldConditions fieldConditions, LambdaExpression fieldExpression)
        {
            _fieldConditions = fieldConditions;
            _fieldExpression = fieldExpression;
        }


        public PropertyBuilder<TEntity> ShowOn(Expression<Func<TEntity, bool>> predicate)
        {
            _fieldConditions.ShowOnConditions.Add(new FieldCondition
            {
                ConditionExpression = predicate,
                FieldExpression = _fieldExpression
            });
            return this;
        }

        public PropertyBuilder<TEntity> EnableOn(Expression<Func<TEntity, bool>> predicate)

        {
            _fieldConditions.EnableOnConditions.Add(new FieldCondition
            {
                ConditionExpression = predicate,
                FieldExpression = _fieldExpression
            });
            return this;
        }

        public PropertyBuilder<TEntity> ValidateOn(Expression<Func<TEntity, bool>> predicate)
        {
            _fieldConditions.ValidateOnConditions.Add(new FieldCondition
            {
                ConditionExpression = predicate,
                FieldExpression = _fieldExpression
            });
            return this;
        }


        public AdminConfig<TEntity> AdminConfig { get; set; }
        public LambdaExpression FieldExpression { get; set; }
    }
}
