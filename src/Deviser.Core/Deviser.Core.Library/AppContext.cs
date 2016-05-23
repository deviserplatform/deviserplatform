using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library
{
    public class AppContext
    {
        public Page CurrentPage { get; set; }

        public int CurrentPageId { get; set; }

        public string CurrentLink { get; set; }

        public CultureInfo CurrentCulture
        {
            get; set;
        }
    }
}
