﻿using System;
using System.Collections.Generic;
using Deviser.Admin.Config;
using Deviser.Core.Common.Json;
using Newtonsoft.Json;

namespace Deviser.Admin
{
    public interface IAdminConfig : IAdminBaseConfig
    {
        AdminConfigType AdminConfigType { get; set; }
        string AdminTitle { get; set; }
        Type AdminServiceType { get; set; }

        ICollection<IChildConfig> ChildConfigs { get; }

        [JsonConverter(typeof(TypeJsonConverter))]
        Type ModelType { get; }

        LookUpDictionary LookUps { get; }
    }
}