using Deviser.Admin.Properties;
using Deviser.Core.Common.DomainTypes.Admin;
using Deviser.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Deviser.Admin.Config;

namespace Deviser.Admin.Builders
{
    /// <summary>
    /// Builds the form configuration 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class FormBuilder<TModel> : FieldBuilder<TModel>
        where TModel : class
    {
        //private readonly IModelConfig _modelConfig;
        
        public FormBuilder(IFormConfig formConfig, KeyField keyField)
        : base(formConfig, keyField)
        {
            //_fieldConfig = fieldConfig;
            //_modelConfig = modelConfig;
        }

        public FormBuilder<TModel> AddFieldSet(string groupName,
            Func<FieldBuilder<TModel>, FieldBuilder<TModel>> fieldBuilderAction, string cssClass = null, string description = null)
        {
            if (_formConfig.FieldConfig.ExcludedFields.Count > 0)
                throw new InvalidOperationException(Resources.AddRemoveInvalidOperation);

            var fieldConfig = new FieldConfig();
            var fieldBuilder = new FieldBuilder<TModel>(_formConfig, _keyField);

            fieldBuilderAction.Invoke(fieldBuilder);

            var fieldSet = new FieldSet
            {
                GroupName = groupName,
                CssClasses = cssClass,
                Description = description,
                Fields = fieldConfig.Fields
            };

            _formConfig.FieldSetConfig.FieldSets.Add(fieldSet);

            return this;
        }
    }
}
