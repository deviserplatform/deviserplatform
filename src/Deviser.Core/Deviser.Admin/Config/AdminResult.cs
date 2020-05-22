using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin.Config
{
    public class AdminResult : IAdminResult
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; }
        public bool IsSucceeded { get; set; }

        public IClientAction SuccessAction { get; set; }

        public AdminResult(object result = null)
        {
            Result = result;
        }
    }

    public class AdminResult<TResult> : IAdminResult<TResult>
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        
        public TResult Value { get; }
        public object Result { get; }
        public bool IsSucceeded { get; set; }
        public IClientAction SuccessAction { get; set; }

        public AdminResult(TResult result = default(TResult))
        {
            Result = Value = result;
        }
    }
}
