using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin.Config
{
    public interface IFormResult
    {
        string SuccessMessage { get; set; }
        string ErrorMessage { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        FormBehaviour FormBehaviour { get; set; }
        object Result { get; }
        bool IsSucceeded { get; set; }
    }

    public interface IFormResult<out TResult> : IFormResult
    {
        TResult Value { get; }
    }
}
