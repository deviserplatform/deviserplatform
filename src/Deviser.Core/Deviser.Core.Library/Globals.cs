using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library
{
    public class Globals
    {
        private static string applicationMapPath;
        private static string hostMapPath;

        private static List<SiteSetting> siteSettings;
        private static ISiteSettingProvider siteSettingProvider;
        private static IPageProvider pageProvider;

        public const string glbHostSkinFolder = "_default";

        public static string HostMapPath
        {
            get
            {
                if (hostMapPath == null)
                {
                    hostMapPath = Path.Combine(ApplicationMapPath, @"Portals\_default\");
                }
                return hostMapPath;
            }
        }

        public static string ApplicationMapPath
        {
            get
            {
                return applicationMapPath ?? (applicationMapPath = GetCurrentDomainDirectory());
            }
        }

        public static CultureInfo CurrentCulture
        {
            get
            {
                //return System.Threading.Thread.CurrentThread.CurrentCulture;
                return null;
            }

        }

        public static string FallbackLanguage
        {
            get
            {
                return "en-US";
            }
        }

        public static Page HomePage { get; set; }

        public static string AdminSkin
        {
            get
            {
                return "~/Portals/_default/Skins/Skyline/Admin.cshtml";
            }
        }

        private static string GetCurrentDomainDirectory()
        {
            //var dir = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\");
            //if (dir.Length > 3 && dir.EndsWith("\\"))
            //{
            //    dir = dir.Substring(0, dir.Length - 1);
            //}
            //return dir;
            return "";
        }

    }
}
