using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class SiteSetting
    {
        public Guid Id { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }
}
