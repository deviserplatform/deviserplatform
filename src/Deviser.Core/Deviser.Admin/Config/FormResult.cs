using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin.Config
{
    public class FormResult : IFormResult
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public FormBehaviour FormBehaviour { get; set; } = FormBehaviour.RedirectToGrid;
        public object Result { get; }
        public bool IsSucceeded { get; set; }

        public FormResult(object result = null)
        {
            Result = result;
        }
    }

    public class FormResult<TResult> : IFormResult<TResult>
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public FormBehaviour FormBehaviour { get; set; } = FormBehaviour.RedirectToGrid;
        public TResult Value { get; }
        public object Result { get; }
        public bool IsSucceeded { get; set; }

        public FormResult(TResult result = default(TResult))
        {
            Result = Value = result;
        }
    }
}
