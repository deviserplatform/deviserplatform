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

        public const string glbHostThemeFolder = "_default";

        public const string moduleRoute = "moduleRoute";
        
        public const string InstallConfigFile = "installconfig.json";

        public const string PlatformAssembly = "Deviser.WI";

        public const string ModuleMigrationTableName = "__ModuleMigrationsHistory";

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

        public static string ApplicationEntryPoint => "Deviser.WI";

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
                return "~/Sites/Default/Themes/{0}/LayoutTypes/{1}.cshtml";
            }
        }

        public static string ContentTypesViewPath
        {
            get
            {
                return "~/Sites/Default/Themes/{0}/ContentTypes/View/{1}.cshtml";
            }
        }

        public static string ContentTypesEditPath
        {
            get
            {
                return "~/Sites/Default/Themes/{0}/ContentTypes/Edit/";
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

        //public static Page HomePage { get; set; }

        //public static string HomePageUrl
        //{
        //    get
        //    {
        //        var homePageTranslation = HomePage.PageTranslation.FirstOrDefault(t => t.Locale == CurrentCulture.ToString());
        //        return (homePageTranslation != null) ? homePageTranslation.URL : "";
        //    }
        //}

        public static string DefaultTheme
        {
            get
            {
                return "~/Sites/Default/Themes/Skyline/Home.cshtml";
            }
        }

        public static string AdminTheme
        {
            get
            {
                return "~/Sites/Default/Themes/Skyline/Admin.cshtml";
            }
        }

        //public static string SiteRoot { get; set; }

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

        public static Guid ModuleViewPermissionId
        {
            get
            {
                return new Guid("34b46847-80be-4099-842a-b654ad550c3e");
            }
        }

        public static Guid ModuleEditPermissionId
        {
            get
            {
                return new Guid("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699");
            }
        }

        public static Guid ContentViewPermissionId
        {
            get
            {
                return new Guid("491b37a3-deba-4f55-9df6-a67cdd810108");
            }
        }

        public static Guid ContentEditPermissionId
        {
            get
            {
                return new Guid("461b37d9-b801-4235-b74f-0c51f35d170f");
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
