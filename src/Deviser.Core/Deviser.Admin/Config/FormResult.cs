using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config
{
    public class FormResult
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public object ResultModel { get; set; }
        public FormResultStatus FormResultStatus { get; set; }
    }

    public enum FormResultStatus
    {
        Unknown = 0,
        Success = 1,
        Error = 2
    }
}
