using Deviser.Admin.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Deviser.Admin.Config.Filters;
using Newtonsoft.Json.Converters;

namespace Deviser.Admin
{
    public class AdminConfig<TModel> : IAdminConfig
        where TModel : class
    {
        private string _adminTitle;

        [JsonConverter(typeof(StringEnumConverter))]
        public AdminConfigType AdminConfigType { get; set; }

        public string AdminTitle
        {
            get => string.IsNullOrEmpty(_adminTitle) ? ModelType.Name : _adminTitle;
            set => _adminTitle = value;
        }

        [JsonIgnore]
        public Type AdminServiceType { get; set; }

        public ICollection<IChildConfig> ChildConfigs { get; }

        public Type ModelType { get; }

        [JsonIgnore]
        public EntityConfig EntityConfig { get; set; }

        public FilterOperator FilterOperator { get; }

        public IModelConfig ModelConfig { get; }

        public LookUpDictionary LookUps { get; }

        public AdminConfig()
        {
            ChildConfigs = new HashSet<IChildConfig>();
            FilterOperator = new FilterOperator();
            ModelType = typeof(TModel);
            ModelConfig = new ModelConfig();
            LookUps = new LookUpDictionary();
        }
    }
}