using Deviser.Admin.Config;
using Deviser.Admin.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Deviser.Admin.Data;
using Deviser.Core.Common.Extensions;

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
        /// Adds a KeyField to this grid. Use this method only for GridOnly AdminConfigType
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">An expression to specify the key field</param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained.</returns>
        public GridBuilder<TModel> AddKeyField<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (_modelConfig.KeyField.FieldExpression != null)
            {
                throw new InvalidOperationException(Resources.MoreKeyFieldsInvalidOperation);
            }
            _modelConfig.KeyField.FieldExpression = expression;
            return this;
        }

        public GridBuilder<TModel> AddRowAction(string actionName, string actionButtonText, Expression<Func<IServiceProvider, TModel, Task<IAdminResult>>> formActionExpression)
        {
            _modelConfig.GridConfig.RowActions.Add(actionName, new AdminAction
            {
                ButtonText = actionButtonText,
                FormActionExpression = formActionExpression
            });
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
            if (_modelConfig.GridConfig.AllIncludeFields.Count > 0)
                ThrowAddRemoveInvalidOperationException();

            var field = new Field()
            {
                FieldExpression = expression
            };

            _modelConfig.GridConfig.RemoveField(field);
            return this;
        }

        public GridBuilder<TModel> DisplayFieldAs<TProperty, TParamFieldProperty>(Expression<Func<TModel, TProperty>> fieldExpression, LabelType labelType, Expression<Func<TModel, TParamFieldProperty>> paramFieldExpression)
        {
            var field = _modelConfig.GridConfig.AllIncludeFields.FirstOrDefault(f => f.FieldName == ReflectionExtensions.GetMemberName(fieldExpression));
            var paramField = CreateSimpleField(paramFieldExpression);
            
            if (field == null)
            {
                throw new InvalidOperationException(Resources.FieldNotFoundInvaidOperation);
            }

            field.FieldOption.LabelOption = new LabelOption {LabelType = labelType};

            if (paramField == null) return this;
            
            field.FieldOption.LabelOption.Parameters.Add("ParamFieldName", paramField.FieldName);
            field.FieldOption.LabelOption.Parameters.Add("ParamFieldNameCamelCase", paramField.FieldNameCamelCase);
            return this;
        }

        public GridBuilder<TModel> DisplayFieldAs<TProperty>(Expression<Func<TModel, TProperty>> fieldExpression, LabelType labelType)
        {
            var field = _modelConfig.GridConfig.AllIncludeFields.FirstOrDefault(f => f.FieldName == ReflectionExtensions.GetMemberName(fieldExpression));

            if (field == null)
            {
                throw new InvalidOperationException(Resources.FieldNotFoundInvaidOperation);
            }

            field.FieldOption.LabelOption = new LabelOption { LabelType = labelType };
            return this;
        }

        public GridBuilder<TModel> EnableSortingBy(Expression<Func<TModel, int>> sortFieldExpression, 
            Expression<Func<IServiceProvider, int, int, IList<TModel>, Task<PagedResult<TModel>>>> sortingExpression)
        {
            if (sortFieldExpression == null || sortingExpression == null)
            {
                throw new InvalidOperationException(Resources.SortExpressionCannotBeNull);
            }

            _modelConfig.GridConfig.SortField = CreateSimpleField(sortFieldExpression);
            _modelConfig.GridConfig.OnSortExpression = sortingExpression;
            return this;
        }

        public GridBuilder<TModel> HideDeleteButton()
        {
            _modelConfig.GridConfig.IsDeleteVisible = false;
            return this;
        }

        public GridBuilder<TModel> HideEditButton()
        {
            _modelConfig.GridConfig.IsEditVisible = false;
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
