using System.Collections.Generic;
using Deviser.Admin.Config;

namespace Deviser.Admin
{
    public class ModelConfig : IModelConfig
    {
        public IDictionary<string, CustomForm> CustomForms { get; }
        public IFormConfig FormConfig { get; }
        public IGridConfig GridConfig { get; }
        public KeyField KeyField { get; }
        public ITreeConfig TreeConfig { get; }
        public ModelConfig()
        {
            CustomForms = new Dictionary<string, CustomForm>();
            FormConfig = new FormConfig();
            GridConfig = new GridConfig();
            KeyField = new KeyField();
            TreeConfig = new TreeConfig();
        }
    }
}