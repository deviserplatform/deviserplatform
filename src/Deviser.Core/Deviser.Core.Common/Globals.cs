using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Deviser.Core.Common
{
    public class Globals
    {
        private static string applicationMapPath;
        private static string hostMapPath;

        public const string glbHostThemeFolder = "_default";

        public const string moduleRoute = "moduleRoute";

        public const string InstallConfigFile = "installconfig.json";

        public const string MigrationAssembly = "Deviser.Core.Data";

        public const string ModuleMigrationTableName = "__ModuleMigrationsHistory";

        public const string OriginalFileSuffix = ".original";

        public const string POLICY_PREFIX = "DEVISERPERMISSION";

        public const int ImageOptimizeDpi = 72;
        public const int ImageOptimizeMaxWidth = 1024;
        public const int ImageOptimizeMaxHeight = 1204;
        public const int ImageOptimizeQualityPercent = 80;

        public const int AdminDefaultPageCount = 10;

        private static Assembly _entryPointAssembly;

        public static Guid PageTypeURL => new Guid("BFEFA535-7AF1-4DDC-82C0-C906C948367A");

        public static Guid PageTypeStandard => new Guid("4C06DCFD-214F-45AF-8404-FF84B412AB01");

        public static Guid PageTypeAdmin => new Guid("5308B86C-A2FC-4220-8BA2-47E7BEC1938D");

        public static string HostMapPath
        {
            get
            {
                if (hostMapPath == null)
                {
                    hostMapPath = Path.Combine(ApplicationMapPath, @"/Sites/Default/");
                }
                return hostMapPath;
            }
        }

        public static string ApplicationMapPath => applicationMapPath ?? (applicationMapPath = GetCurrentDomainDirectory());

        public static Assembly EntryPointAssembly
        {
            get
            {
                if (_entryPointAssembly == null)
                {
                    _entryPointAssembly = System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies.First(a => a.EntryPoint != null);
                }

                return _entryPointAssembly;
            }
        }

        //public static string ApplicationEntryPoint => "Deviser.WI";

        public static string DefaultSitePath => "~/Sites/Default/";

        public static string LayoutTypesPath => "~/Sites/Default/Themes/{0}/LayoutTypes/{1}.cshtml";

        public static string ContentTypesViewPath => "~/Sites/Default/Themes/{0}/ContentTypes/View/{1}.cshtml";

        public static string ContentTypesEditPath => "~/Sites/Default/Themes/{0}/ContentTypes/Edit/";

        public static string DocumentsFolder => "documents";

        public static string MenuStylePath => "~/Sites/Default/Themes/{0}/MenuStyles/{1}.cshtml";

        public static string BreadCrumbStylePath => "~/Sites/Default/Themes/{0}/BreadCrumbStyles/{1}.cshtml";

        public static string SiteAssetsPath => "assets/";

        public static string ImagesFolder => "images";

        public static string UploadTempFolder => "temp";

        public static CultureInfo CurrentCulture
        {
            get; set;
        }

        public static string FallbackLanguage => "en-US";

        //public static Page HomePage { get; set; }

        //public static string HomePageUrl
        //{
        //    get
        //    {
        //        var homePageTranslation = HomePage.PageTranslation.FirstOrDefault(t => t.Locale == CurrentCulture.ToString());
        //        return (homePageTranslation != null) ? homePageTranslation.URL : "";
        //    }
        //}

        public static string DefaultTheme => "~/Sites/Default/Themes/Skyline/Home.cshtml";

        public static string AdminTheme => "~/Sites/Default/Themes/Skyline/Admin.cshtml";

        //public static string SiteRoot { get; set; }

        public static Guid AdministratorRoleId => new Guid("9b461499-c49e-4398-bfed-4364a176ebbd");

        public static Guid AllUsersRoleId => new Guid("086357bf-01b1-494c-a8b8-54fdfa7c4c9e");

        public static Guid PageViewPermissionId => new Guid("29cb1b57-1862-4300-b378-f3271b870148");

        public static Guid PageEditPermissionId => new Guid("2da41181-be15-4ad6-a89c-3ba8b71f993b");

        public static Guid ModuleViewPermissionId => new Guid("34b46847-80be-4099-842a-b654ad550c3e");

        public static Guid ModuleEditPermissionId => new Guid("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699");

        public static Guid ContentViewPermissionId => new Guid("491b37a3-deba-4f55-9df6-a67cdd810108");

        public static Guid ContentEditPermissionId => new Guid("461b37d9-b801-4235-b74f-0c51f35d170f");

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
