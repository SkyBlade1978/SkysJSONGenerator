using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkysJSONGenerator
{
    public class Profile //v2
    {
        public int ProfileVersion { get; set; }
        public string Version { get; set; }
        public string Modid { get; set; }
        public List<string> Materials { get; set; }
        public string TextureBlocksFolder { get; set; }
        public string TextureItemsFolder { get; set; }
        public List<Block> Blocks { get; set; }
        public Profile ()
        {
            Materials = new List<string>();
            Blocks = new List<Block>();
        }

        public Profile(string version, string modid, List<string> materials, string textureBlocksFolder, string textureItemsFolder, int profileVersion, List<Block> blocks)
        {
            Version = version;
            Modid = modid;
            Materials = materials;
            TextureBlocksFolder = textureBlocksFolder;
            TextureItemsFolder = textureItemsFolder;
            ProfileVersion = profileVersion;
            Blocks = blocks;
        }

        public override string ToString()
        {
            return Modid + " - " + Version;
        }
    }
}
