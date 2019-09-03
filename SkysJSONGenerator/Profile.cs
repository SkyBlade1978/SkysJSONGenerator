using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkysJSONGenerator
{
    public class Profile
    {
        public string Version { get; set; }
        public string Modid { get; set; }
        public List<string> Materials { get; set; }
        public string TextureBlocksFolder { get; set; }
        public string TextureItemsFolder { get; set; }

        public Profile ()
        {
            Materials = new List<string>();
        }

        public Profile(string version, string modid, List<string> materials, string textureBlocksFolder, string textureItemsFolder)
        {
            Version = version;
            Modid = modid;
            Materials = materials;
            TextureBlocksFolder = textureBlocksFolder;
            TextureItemsFolder = textureItemsFolder;
        }

        public override string ToString()
        {
            return Modid + " - " + Version;
        }
    }
}
