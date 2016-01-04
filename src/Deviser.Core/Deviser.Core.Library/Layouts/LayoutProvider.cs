using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Layouts
{
    public class LayoutProvider
    {

        public static List<KeyValuePair<string, string>> GetHostSkins()
        {
            string skinRoot = "Skins";
            var skins = new List<KeyValuePair<string, string>>();

            string root = Globals.HostMapPath + skinRoot;
            if (Directory.Exists(root))
            {
                foreach (string skinFolder in Directory.GetDirectories(root))
                {
                    if (!skinFolder.EndsWith(Globals.glbHostSkinFolder))
                    {
                        AddSkinFiles(skins, skinRoot, skinFolder, false);
                    }
                }
            }
            return skins;
        }
        
        private static void AddSkinFiles(List<KeyValuePair<string, string>> skins, string skinRoot, string skinFolder, bool isPortal)
        {
            foreach (string skinFile in Directory.GetFiles(skinFolder, "*.cshtml"))
            {
                string fileName = Path.GetFileNameWithoutExtension(skinFile);
                if(!fileName.StartsWith("_"))
                {
                    string folder = skinFolder.Substring(skinFolder.LastIndexOf("\\") + 1);
                    string key = ((isPortal) ? "Site: " : "Host: ") + FormatSkinName(folder, Path.GetFileNameWithoutExtension(skinFile));
                    string prefix = (isPortal) ? "[S]" : "[G]"; //to be compliant with all versions
                    string value = prefix + skinRoot + "/" + folder + "/" + Path.GetFileName(skinFile);
                    skins.Add(new KeyValuePair<string, string>(key, value));
                }
            }
        }

        private static string FormatSkinName(string skinFolder, string skinFile)
        {
            if (skinFolder.ToLower() == "_default")
            {
                // host folder
                return skinFile;

            }

            //portal folder
            switch (skinFile.ToLower())
            {
                case "skin":
                case "container":
                case "default":
                    return skinFolder;
                default:
                    return skinFolder + " - " + skinFile;
            }
        }
    }
}
