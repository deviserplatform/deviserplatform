using Deviser.Core.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library
{
    public class PageContext
    {
        [JsonIgnore]
        public Page CurrentPage { get; set; }

        public Guid CurrentPageId { get; set; }

        public string CurrentUrl { get; set; }

        public bool HasPageViewPermission { get; set; }

        public bool HasPageEditPermission { get; set; }

        [JsonIgnore]
        public CultureInfo CurrentCulture
        {
            get; set;
        }

        public string CurrentLocale
        {
            get
            {
                return CurrentCulture.Name;
            }
        }
    }
}
