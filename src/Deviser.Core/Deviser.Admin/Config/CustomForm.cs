using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Deviser.Core.Common.DomainTypes.Admin;

namespace Deviser.Admin.Config
{
    public class CustomForm
    {
        public string FormName { get; set; }
        public IFormConfig FormConfig { get; }
        public FormOption FormOption { get; set; }
        public KeyField KeyField { get; }
        public Type ModelType { get; set; }
        public LambdaExpression SubmitActionExpression { get; set; }

        public CustomForm()
        {
            KeyField = new KeyField();
            FormConfig = new FormConfig();
        }

    }
}
