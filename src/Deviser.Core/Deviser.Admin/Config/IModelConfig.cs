using System.Collections.Generic;
using Deviser.Admin.Config;

namespace Deviser.Admin
{
    public interface IModelConfig
    {
        IDictionary<string, CustomForm> CustomForms { get; }
        IFormConfig FormConfig { get; }
        IGridConfig GridConfig { get; }
        KeyField KeyField { get; }        
    }
}