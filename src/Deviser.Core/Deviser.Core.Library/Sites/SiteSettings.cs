using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Core.Common;

namespace Deviser.Core.Library.Sites
{
    public class SiteSettings
    {
        static SiteSettings()
        {
            HomeDirectoryMapPath = Globals.ApplicationMapPath + @"Portals\_default\";
        }

        public static string HomeDirectoryMapPath { get; internal set; }
    }
}
