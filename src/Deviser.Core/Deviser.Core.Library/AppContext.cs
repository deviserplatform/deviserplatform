using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
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

        public string CurrentCulture
        {
            get
            {
                //return System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                return "";
            }
        }
    }
}
