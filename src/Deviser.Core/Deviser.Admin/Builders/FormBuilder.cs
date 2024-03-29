﻿using Deviser.Admin.Config;
using Deviser.Admin.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Deviser.Core.Common.Extensions;

namespace Deviser.Admin.Builders
{
    /// <summary>
    /// Builds the form configuration 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class FormBuilder<TModel> : FieldBuilder<TModel>
        where TModel : class
    {
        private readonly IFormConfig _formConfig;

        public string Title
        {
            get => _formConfig.Title;
            set => _formConfig.Title = value;
        }

        public FormBuilder(IFormConfig formConfig, KeyField keyField)
        : base(formConfig.FieldConfig, formConfig.AllFields, keyField)
        {
            _formConfig = formConfig;
        }

        public FormBuilder<TModel> AddFieldSet(string groupName,
            Action<FieldBuilder<TModel>> fieldBuilderAction, string cssClass = null, string description = null)
        {
            if (_formConfig.FieldConfig.ExcludedFields.Count > 0)
                throw new InvalidOperationException(Resources.AddRemoveInvalidOperation);

            var fieldConfig = new FieldConfig();
            var fieldBuilder = new FieldBuilder<TModel>(fieldConfig, _formConfig.AllFields, _keyField);

            fieldBuilderAction.Invoke(fieldBuilder);

            var fieldSet = new FieldSet
            {
                GroupName = groupName,
                CssClasses = cssClass,
                Description = description,
                Fields = fieldConfig.Fields
            };

            _formConfig.FieldSetConfig.AddFieldSet(fieldSet);

            return this;
        }

        public FormPropertyBuilder<TModel, TProperty> Property<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return new FormPropertyBuilder<TModel, TProperty>(_formConfig, expression);
        }

        public FormPropertyBuilder<TModel, TProperty> Property<TProperty>(Expression<Func<TModel, ICollection<TProperty>>> expression)
        {
            return new FormPropertyBuilder<TModel, TProperty>(_formConfig, expression);
        }

        public FormBuilder<TModel> SetCustomValidationFor<TProperty>(Expression<Func<TModel, TProperty>> fieldExpression, Expression<Func<IServiceProvider, TProperty, Task<ValidationResult>>> validationExpression)
        {
            var fieldName = ReflectionExtensions.GetMemberName(fieldExpression);
            var field = _formConfig.AllFields.FirstOrDefault(f => f.FieldName == fieldName);
            if (field == null)
            {
                throw new InvalidOperationException(Resources.FieldNotFoundInvaidOperation);
            }
            field.FieldOption.ValidationType = ValidationType.Custom;
            field.FieldOption.ValidationExpression = validationExpression;
            return this;
        }

        public FormBuilder<TModel> AddFormAction(string actionName, string actionButtonText, Expression<Func<IServiceProvider, TModel, Task<IFormResult<TModel>>>> formActionExpression)
        {
            _formConfig.FormActions.Add(actionName, new AdminAction
            {
                ButtonText = actionButtonText,
                FormActionExpression = formActionExpression
            });

            return this;
        }

        public FormBuilder<TModel> SetFormOption(Action<FormOption> formOptionAction)
        {
            var formOption = new FormOption();
            formOptionAction?.Invoke(formOption);
            _formConfig.FormOption = formOption;
            return this;
        }
    }
}
