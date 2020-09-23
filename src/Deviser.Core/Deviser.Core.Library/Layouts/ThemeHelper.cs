//using Microsoft.Extensions.PlatformAbstractions;
using Deviser.Core.Common;
using System.Collections.Generic;
using System.IO;

namespace Deviser.Core.Library.Layouts
{
    public class ThemeHelper
    {
        public static List<KeyValuePair<string, string>> GetHostThemes()
        {
            var ThemeRoot = "Themes";
            var themes = new List<KeyValuePair<string, string>>();

            var root = Globals.HostMapPath + ThemeRoot;
            if (!Directory.Exists(root)) return themes;

            foreach (var themeFolder in Directory.GetDirectories(root))
            {
                if (!themeFolder.EndsWith(Globals.glbHostThemeFolder))
                {
                    AddThemeFiles(themes, ThemeRoot, themeFolder, false);
                }
            }
            return themes;
        }
        
        private static void AddThemeFiles(List<KeyValuePair<string, string>> themes, string themeRoot, string themeFolder, bool isPortal)
        {
            foreach (var themeFile in Directory.GetFiles(themeFolder, "*.cshtml"))
            {
                var fileName = Path.GetFileNameWithoutExtension(themeFile);
                if (fileName.StartsWith("_")) continue;

                var folder = themeFolder.Substring(themeFolder.LastIndexOf("\\") + 1);
                var key = ((isPortal) ? "Site: " : "Host: ") + FormatThemeName(folder, Path.GetFileNameWithoutExtension(themeFile));
                var prefix = (isPortal) ? "[S]" : "[G]"; //to be compliant with all versions
                var value = prefix + themeRoot + "/" + folder + "/" + Path.GetFileName(themeFile);
                themes.Add(new KeyValuePair<string, string>(key, value));
            }
        }

        private static string FormatThemeName(string themeFolder, string themeFile)
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
