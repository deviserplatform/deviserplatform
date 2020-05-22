using Deviser.Admin.Config;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public CustomFormBuilder<TModel> OnSubmit(Expression<Func<IServiceProvider, TModel, Task<IFormResult>>> submitActionExpression)
        {
            _customForm.SubmitActionExpression = submitActionExpression;
            return this;
        }
    }
}
