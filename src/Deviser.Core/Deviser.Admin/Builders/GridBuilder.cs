using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Deviser.Admin.Properties;
using Deviser.Core.Common.DomainTypes.Admin;

namespace Deviser.Admin.Builders
{
    public class GridBuilder<TModel>
        where TModel : class
    {
        private readonly IModelConfig _modelConfig;

        public GridBuilder(IModelConfig modelConfig)
        {
            //_fieldConfig = fieldConfig;
            _modelConfig = modelConfig;
        }

        /// <summary>
        /// Adds a new field to this grid
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">An expression to specify a field</param>
        /// <param name="fieldOptionAction">Additional options can be specified here</param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained.</returns>
        public GridBuilder<TModel> AddField<TProperty>(Expression<Func<TModel, TProperty>> expression, Action<FieldOption> fieldOptionAction = null)
        {
            var field = CreateSimpleField(expression, fieldOptionAction);
            _modelConfig.GridConfig.AddField(field); //FormConfig.FieldConfig.AddField(field);
            return this;
        }

        /// <summary>
        /// Excludes a new field to this grid. This method cannot be used together with method AddField
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">An expression to remove a field</param>
        /// <returns></returns>
        public GridBuilder<TModel> RemoveField<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (_modelConfig.FormConfig.AllFormFields.Count > 0)
                ThrowAddRemoveInvalidOperationException();

            var field = new Field()
            {
                FieldExpression = expression
            };

            _modelConfig.GridConfig.RemoveField(field);
            return this;
        }

        private Field CreateSimpleField<TProperty>(Expression<Func<TModel, TProperty>> expression, Action<FieldOption> fieldOptionAction = null)
        {
            if (_modelConfig.GridConfig.ExcludedFields.Count > 0)
                ThrowAddRemoveInvalidOperationException();

            FieldOption fieldOption = new FieldOption();
            fieldOptionAction?.Invoke(fieldOption);
            return new Field
            {
                FieldExpression = expression,
                FieldOption = fieldOption
            };
        }

        private void ThrowAddRemoveInvalidOperationException()
        {
            throw new InvalidOperationException(Resources.AddRemoveInvalidOperation);
        }
    }
}
