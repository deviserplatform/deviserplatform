using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin.Config
{
    public interface IFormResult : IAdminResult
    {
        [JsonConverter(typeof(StringEnumConverter))]
        FormBehaviour FormBehaviour { get; set; }
    }

    public interface IFormResult<out TResult> : IFormResult, IAdminResult<TResult>
    {

    }
}
