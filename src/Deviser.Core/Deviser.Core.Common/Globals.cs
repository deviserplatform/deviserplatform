using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common
{
    public class Globals
    {
        private static string applicationMapPath;
        private static string hostMapPath;

        public const string glbHostSkinFolder = "_default";

        public const string moduleRoute = "moduleRoute";

        public static string HostMapPath
        {
            get
            {
                if (hostMapPath == null)
                {
                    hostMapPath = Path.Combine(ApplicationMapPath, @"Sites\Default\");
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

        public static string DefaultSitePath
        {
            get
            {
                return "~/Sites/Default/";
            }
        }

        public static string LayoutTypesPath
        {
            get
            {
                return "~/Views/Shared/LayoutTypes/{0}.cshtml";
            }
        }

        public static string ContentTypesViewPath
        {
            get
            {
                return "~/Views/Shared/ContentTypes/View/{0}.cshtml";
            }
        }

        public static string ContentTypesEditPath
        {
            get
            {
                return "~/Views/Shared/ContentTypes/Edit/";
            }
        }

        public static string MenuStylePath
        {
            get
            {
                return "~/Views/Shared/MenuStyles/{0}.cshtml";
            }
        }

        public static string BreadCrumbStylePath
        {
            get
            {
                return "~/Views/Shared/BreadCrumbStyles/{0}.cshtml";
            }
        }

        public static string SiteAssetsPath
        {
            get
            {
                return "~/assets/";
            }
        }

        public static string ImagesFolder
        {
            get
            {
                return "images";
            }
        }

        public static string UploadTempFolder
        {
            get
            {
                return "temp";
            }
        }

        public static CultureInfo CurrentCulture
        {
            get; set;
        }

        public static string FallbackLanguage
        {
            get
            {
                return "en-US";
            }
        }

        public static Page HomePage { get; set; }

        public static string HomePageUrl
        {
            get
            {
                var homePageTranslation = HomePage.PageTranslation.FirstOrDefault(t => t.Locale == CurrentCulture.ToString());
                return (homePageTranslation != null) ? homePageTranslation.URL : "";
            }
        }

        public static string DefaultSkin
        {
            get
            {
                return "~/Sites/Default/Skins/Skyline/Home.cshtml";
            }
        }

        public static string AdminSkin
        {
            get
            {
                return "~/Sites/Default/Skins/Skyline/Admin.cshtml";
            }
        }

        public static string SiteRoot { get; set; }

        public static Guid AdministratorRoleId
        {
            get
            {
                return new Guid("9b461499-c49e-4398-bfed-4364a176ebbd");
            }
        }

        public static Guid AllUsersRoleId
        {
            get
            {
                return new Guid("086357bf-01b1-494c-a8b8-54fdfa7c4c9e");
            }
        }

        public static Guid PageViewPermissionId
        {
            get
            {
                return new Guid("29cb1b57-1862-4300-b378-f3271b870148");
            }
        }

        public static Guid PageEditPermissionId
        {
            get
            {
                return new Guid("2da41181-be15-4ad6-a89c-3ba8b71f993b");
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
