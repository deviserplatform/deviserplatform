using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Deviser.Core.Common.DomainTypes.Admin;
using Newtonsoft.Json;

namespace Deviser.Admin.Config
{
    public class CustomForm
    {
        public string FormName { get; set; }
        public IFormConfig FormConfig { get; }        
        public KeyField KeyField { get; }
        public Type ModelType { get; set; }

        [JsonIgnore]
        public LambdaExpression SubmitActionExpression { get; set; }

        public CustomForm()
        {
            KeyField = new KeyField();
            FormConfig = new FormConfig();
        }

    }
}
