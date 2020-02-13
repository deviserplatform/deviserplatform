﻿using Deviser.Core.Common;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using System.IO;

namespace Deviser.Core.Library.Layouts
{
    public class ThemeManager : IThemeManager
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ThemeManager(IWebHostEnvironment appEnvironment)
        {
            _hostingEnvironment = appEnvironment;
        }

        public List<KeyValuePair<string, string>> GetHostThemes()
        {
            string themeRoot = "Themes";
            var themes = new List<KeyValuePair<string, string>>();

            string root = _hostingEnvironment.ContentRootPath + "\\" + Globals.HostMapPath + themeRoot;
            if (Directory.Exists(root))
            {
                foreach (string themeFolder in Directory.GetDirectories(root))
                {
                    if (!themeFolder.EndsWith(Globals.glbHostThemeFolder))
                    {
                        AddThemeFiles(themes, themeRoot, themeFolder, false);
                    }
                }
            }
            return themes;
        }

        private void AddThemeFiles(List<KeyValuePair<string, string>> themes, string themeRoot, string themeFolder, bool isPortal)
        {
            foreach (string themeFile in Directory.GetFiles(themeFolder, "*.cshtml"))
            {
                string fileName = Path.GetFileNameWithoutExtension(themeFile);
                if (!fileName.StartsWith("_"))
                {
                    string folder = themeFolder.Substring(themeFolder.LastIndexOf("\\") + 1);
                    string key = ((isPortal) ? "Site: " : "Host: ") + FormatThemeName(folder, Path.GetFileNameWithoutExtension(themeFile));
                    string prefix = (isPortal) ? "[S]" : "[G]"; //to be compliant with all versions
                    string value = prefix + themeRoot + "/" + folder + "/" + Path.GetFileName(themeFile);
                    themes.Add(new KeyValuePair<string, string>(key, value));
                }
            }
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
