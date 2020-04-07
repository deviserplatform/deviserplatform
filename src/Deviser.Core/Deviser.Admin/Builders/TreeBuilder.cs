﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Deviser.Admin.Properties;

namespace Deviser.Admin.Builders
{
    public class TreeBuilder<TModel>
        where TModel : class
    {
        private readonly IModelConfig _modelConfig;

        public TreeBuilder(IModelConfig modelConfig)
        {
            _modelConfig = modelConfig;
        }
        

        public TreeBuilder<TModel> ConfigureTree<TKey, TDisplayField>(Expression<Func<TModel, TKey>> keyExpression,
            Expression<Func<TModel, TDisplayField>> displayExpression, Expression<Func<TModel, ICollection<TModel>>> childrenExpression)
        {
            if (_modelConfig.KeyField.FieldExpression != null)
            {
                throw new InvalidOperationException(Resources.MoreKeyFieldsInvalidOperation);
            }
            _modelConfig.KeyField.FieldExpression = keyExpression;
            _modelConfig.TreeConfig.ChildrenField.FieldExpression = childrenExpression;
            _modelConfig.TreeConfig.DisplayField.FieldExpression = displayExpression;
            return this;
        }
    }
}
