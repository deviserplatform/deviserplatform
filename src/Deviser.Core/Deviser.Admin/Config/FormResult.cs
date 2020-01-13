using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config
{
    public class FormResult : IFormResult
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
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
        public TResult Value { get; }
        public object Result { get; }
        public bool IsSucceeded { get; set; }

        public FormResult(TResult result = default(TResult))
        {
            Result = Value = result;
        }
    }
}
