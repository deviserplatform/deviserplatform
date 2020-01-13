using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes.Admin;

namespace Deviser.Admin.Builders
{
    public class CustomFormBuilder<TModel> : FormBuilder<TModel>
        where TModel : class
    {
        private readonly CustomForm _customForm;
        public CustomFormBuilder(CustomForm customForm) 
            : base(customForm.FormConfig, customForm.KeyField)
        {
            _customForm = customForm;
        }

        public CustomFormBuilder<TModel> OnSubmit(Expression<Func<IServiceProvider, TModel, Task<FormResult>>> submitActionExpression)
        {
            _customForm.SubmitActionExpression = submitActionExpression;
            return this;
        }
    }
}
