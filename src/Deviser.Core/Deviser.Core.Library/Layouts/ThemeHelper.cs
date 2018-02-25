//using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Core.Common;

namespace Deviser.Core.Library.Layouts
{
    public class ThemeHelper
    {
        public static List<KeyValuePair<string, string>> GetHostThemes()
        {
            string ThemeRoot = "Themes";
            var themes = new List<KeyValuePair<string, string>>();

            string root = Globals.HostMapPath + ThemeRoot;
            if (Directory.Exists(root))
            {
                foreach (string ThemeFolder in Directory.GetDirectories(root))
                {
                    if (!ThemeFolder.EndsWith(Globals.glbHostThemeFolder))
                    {
                        AddThemeFiles(themes, ThemeRoot, ThemeFolder, false);
                    }
                }
            }
            return themes;
        }
        
        private static void AddThemeFiles(List<KeyValuePair<string, string>> themes, string themeRoot, string themeFolder, bool isPortal)
        {
            foreach (string themeFile in Directory.GetFiles(themeFolder, "*.cshtml"))
            {
                string fileName = Path.GetFileNameWithoutExtension(themeFile);
                if(!fileName.StartsWith("_"))
                {
                    string folder = themeFolder.Substring(themeFolder.LastIndexOf("\\") + 1);
                    string key = ((isPortal) ? "Site: " : "Host: ") + FormatThemeName(folder, Path.GetFileNameWithoutExtension(themeFile));
                    string prefix = (isPortal) ? "[S]" : "[G]"; //to be compliant with all versions
                    string value = prefix + themeRoot + "/" + folder + "/" + Path.GetFileName(themeFile);
                    themes.Add(new KeyValuePair<string, string>(key, value));
                }
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
