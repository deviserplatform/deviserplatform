﻿using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class SiteSetting
    {
        public Guid Id { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }
}
