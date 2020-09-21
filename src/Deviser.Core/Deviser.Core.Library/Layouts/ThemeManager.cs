using Deviser.Core.Common;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Deviser.Core.Library.Layouts
{
    public class ThemeManager : IThemeManager
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IViewProvider _viewProvider;

        public ThemeManager(IWebHostEnvironment appEnvironment,
            IViewProvider viewProvider)
        {
            _hostingEnvironment = appEnvironment;
            _viewProvider = viewProvider;
        }

        public List<KeyValuePair<string, string>> GetHostThemes()
        {
            var themeRoot = "Themes";
            var themes = new List<KeyValuePair<string, string>>();

            var views = _viewProvider.GetCompiledViewDescriptors(); ;
            var root = Path.Combine(Globals.HostMapPath, themeRoot);

            var themeViews = views.Where(v => v.RelativePath.Contains(root) && 
                                              !v.RelativePath.Contains("_")&&
                                              !v.RelativePath.Contains("BreadCrumbStyles") &&
                                              !v.RelativePath.Contains("ContentTypes") &&
                                              !v.RelativePath.Contains("LayoutTypes") &&
                                              !v.RelativePath.Contains("MenuStyles") &&
                                              !v.RelativePath.Contains("_")).ToList();

            foreach (var themeView in themeViews)
            {
                if (!themeView.RelativePath.EndsWith(Globals.glbHostThemeFolder))
                {
                    AddThemeFiles(themes, themeRoot, themeView.RelativePath, false);
                }
            }

            return themes;
        }

        private void AddThemeFiles(List<KeyValuePair<string, string>> themes, string themeRoot, string themePath, bool isPortal)
        {
            var pathParts = themePath.Split('/');
            var folder = pathParts[pathParts.Length - 2];
            var key = ((isPortal) ? "Site: " : "Host: ") + FormatThemeName(folder, Path.GetFileNameWithoutExtension(themePath));
            var prefix = (isPortal) ? "[S]" : "[G]"; //to be compliant with all versions
            var value = prefix + themeRoot + "/" + folder + "/" + Path.GetFileName(themePath);
            themes.Add(new KeyValuePair<string, string>(key, value));
        }

        private string FormatThemeName(string themeFolder, string themeFile)
        {
            if (themeFolder.ToLower() == "_default")
            {
                // host folder
                return themeFile;

            }

            //portal folder
            switch (themeFile.ToLower())
            {
                case "theme":
                case "container":
                case "default":
                    return themeFolder;
                default:
                    return themeFolder + " - " + themeFile;
            }
        }
    }
}
