using Deviser.Admin.Config;
using Deviser.Admin.Properties;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Deviser.Admin.Builders
{
    /// <summary>
    /// Builds the filed configuration 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class FieldBuilder<TModel>
        where TModel : class
    {
        private IFieldConfig _fieldConfig;
        //private readonly IModelConfig _modelConfig;
        //protected readonly IFormConfig _formConfig;
        protected readonly KeyField _keyField;
        private readonly ICollection<Field> _allFormFields;

        public FieldBuilder(IFieldConfig fieldConfig, ICollection<Field> allFormFields, KeyField keyField)
        {
            //_fieldConfig = fieldConfig;
            //_modelConfig = modelConfig;

            //_formConfig = formConfig;
            _allFormFields = allFormFields;
            _fieldConfig = fieldConfig;
            _keyField = keyField;
        }

        /// <summary>
        /// Adds a KeyField to this form
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">An expression to specify the key field</param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained.</returns>
        public FieldBuilder<TModel> AddKeyField<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (_keyField.FieldExpression != null)
            {
                throw new InvalidOperationException(Resources.MoreKeyFieldsInvalidOperation);
            }
            _keyField.FieldExpression = expression;
            return this;
        }

        /// <summary>
        /// Adds a new field (new row) to this form
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">An expression to specify a field</param>
        /// <param name="fieldOptionAction">Additional options can be specified here</param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained.</returns>
        public FieldBuilder<TModel> AddField<TProperty>(Expression<Func<TModel, TProperty>> expression, Action<FieldOption> fieldOptionAction = null)
        {
            var field = CreateSimpleField(expression, fieldOptionAction);
            _fieldConfig.AddField(field);
            return this;
        }

        /// <summary>
        /// Adds a new field (in-line) to this form
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">An expression to specify a field</param>
        /// <param name="fieldOptionAction">Additional options can be specified here</param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained.</returns>
        public FieldBuilder<TModel> AddInlineField<TProperty>(Expression<Func<TModel, TProperty>> expression, Action<FieldOption> fieldOptionAction = null)
        {
            var field = CreateSimpleField(expression, fieldOptionAction);
            _fieldConfig.AddInLineField(field);
            return this;
        }

        /// <summary>
        /// Adds a new select field (new row) to this form. This method assumes that an Entity in EFCore for the TModel has been configured in MapperConfiguration 
        /// </summary>
        /// <typeparam name="TRelatedModel"></typeparam>
        /// <param name="expression">An expression to specify a field</param>
        /// <param name="displayExpression">An expression to specify display property of select items</param>
        /// <param name="fieldOptionAction">Additional options can be specified here</param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained.</returns>
        public FieldBuilder<TModel> AddSelectField<TRelatedModel>(Expression<Func<TModel, TRelatedModel>> expression,
            Expression<Func<TRelatedModel, string>> displayExpression = null,
            Action<FieldOption> fieldOptionAction = null)
            where TRelatedModel : class
        {
            var field = CreateComplexField(expression, RelationType.ManyToOne, typeof(TRelatedModel), displayExpression, fieldOptionAction);
            _fieldConfig.AddField(field);
            return this;
        }

        /// <summary>
        /// Adds a new select field (in-line) to this form. This method assumes that an Entity in EFCore for the TModel has been configured in MapperConfiguration 
        /// </summary>
        /// <typeparam name="TRelatedModel"></typeparam>
        /// <param name="expression">An expression to specify a field</param>
        /// <param name="displayExpression">An expression to specify display property of select items</param>
        /// <param name="fieldOptionAction">Additional options can be specified here</param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained.</returns>
        public FieldBuilder<TModel> AddInlineSelectField<TRelatedModel>(Expression<Func<TModel, TRelatedModel>> expression,
            Expression<Func<TRelatedModel, string>> displayExpression = null,
            Action<FieldOption> fieldOptionAction = null)
            where TRelatedModel : class
        {
            var field = CreateComplexField(expression, RelationType.ManyToOne, typeof(TRelatedModel), displayExpression, fieldOptionAction);
            _fieldConfig.AddInLineField(field);
            return this;
        }

        /// <summary>
        /// Adds a new multi-select field (new row) to this form. This method assumes that an Entity in EFCore for the TModel has been configured in MapperConfiguration 
        /// </summary>
        /// <typeparam name="TRelatedModel"></typeparam>
        /// <param name="expression">An expression to specify a field</param>
        /// <param name="displayExpression">An expression to specify display property of select items</param>
        /// <param name="fieldOptionAction">Additional options can be specified here</param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained.</returns>
        public FieldBuilder<TModel> AddMultiselectField<TRelatedModel>(Expression<Func<TModel, ICollection<TRelatedModel>>> expression,
            Expression<Func<TRelatedModel, string>> displayExpression = null,
            Action<FieldOption> fieldOptionAction = null)
            where TRelatedModel : class
        {
            var field = CreateComplexField(expression, RelationType.ManyToMany, typeof(TRelatedModel), displayExpression, fieldOptionAction);
            _fieldConfig.AddField(field);
            return this;
        }

        /// <summary>
        /// Adds a new multi-select field (in-line) to this form. This method assumes that an Entity in EFCore for the TModel has been configured in MapperConfiguration 
        /// </summary>
        /// <typeparam name="TRelatedModel"></typeparam>
        /// <param name="expression">An expression to specify a field</param>
        /// <param name="displayExpression">An expression to specify display property of select items</param>
        /// <param name="fieldOptionAction">Additional options can be specified here</param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained.</returns>
        public FieldBuilder<TModel> AddInlineMultiSelectField<TRelatedModel>(Expression<Func<TModel, ICollection<TRelatedModel>>> expression,
            Expression<Func<TRelatedModel, string>> displayExpression = null,
            Action<FieldOption> fieldOptionAction = null)
            where TRelatedModel : class
        {
            var field = CreateComplexField(expression, RelationType.ManyToMany, typeof(TRelatedModel), displayExpression, fieldOptionAction);
            _fieldConfig.AddInLineField(field);
            return this;
        }

        /// <summary>
        /// Remove Field cannot be combined with AddField/AddInlineField/AddComplexField/AddInlineComplexField
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">An expression to remove a field</param>
        /// <returns></returns>
        public FieldBuilder<TModel> RemoveField<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (_allFormFields.Count > 0)
                ThrowAddRemoveInvalidOperationException();

            var field = new Field()
            {
                FieldExpression = expression
            };

            _fieldConfig.RemoveField(field);
            return this;
        }

        /// <summary>
        /// Creates a complex field from given expression
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="lookupDisplayExpression"></param>
        /// <param name="fieldOptionAction"></param>
        /// <param name="relatedModelType"></param>
        /// <returns></returns>
        private Field CreateComplexField<TProperty>(Expression<Func<TModel, TProperty>> expression,
            RelationType releationType,
            Type relatedModelType,
            LambdaExpression lookupDisplayExpression,
            Action<FieldOption> fieldOptionAction = null)
            where TProperty : class
        {
            if (_fieldConfig.ExcludedFields.Count > 0)
                ThrowAddRemoveInvalidOperationException();

            FieldOption fieldOption = new FieldOption();
            fieldOptionAction?.Invoke(fieldOption);
            fieldOption.LookupDisplayExpression = lookupDisplayExpression;
            fieldOption.RelationType = releationType;
            fieldOption.LookupModelType = relatedModelType;

            return new Field
            {
                FieldExpression = expression,
                FieldOption = fieldOption
            };
        }

        /// <summary>
        /// Create a simple filed from given expression
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="fieldOptionAction"></param>
        /// <returns></returns>
        private Field CreateSimpleField<TProperty>(Expression<Func<TModel, TProperty>> expression, Action<FieldOption> fieldOptionAction = null)
        {
            if (_fieldConfig.ExcludedFields.Count > 0)
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
