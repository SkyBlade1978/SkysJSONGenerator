using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SkysJSONGenerator
{
    public class JSonGenerator
    {
        private Profile _profile;

        private string _basePath;
        private string _blockstatesPath;
        private string _modelsPath;
        private string _modelsBlockPath;
        private string _modelsItemPath;
        private string _lootTablePath;
        private int _filesGenerated;

        public JSonGenerator(Profile profile)
        {
            _profile = profile;

            _basePath = "out\\" + _profile.Modid + "\\" + _profile.Version;
            _lootTablePath = _basePath + "\\data\\" + _profile.Modid + "\\loot_tables\\blocks";
            _blockstatesPath = _basePath + "\\assets\\" + _profile.Modid + "\\blockstates";
            _modelsPath = _basePath + "\\assets\\" + _profile.Modid + "\\models";
            _modelsBlockPath = _modelsPath + "\\block";
            _modelsItemPath = _modelsPath + "\\item";
        }

        private void RenderBlockJSON(bool smooth)
        {
            foreach (var item in _profile.Materials)
            {
                var name = item;

                if (smooth)
                    name += "_smooth";

                var fileName = "\\" + name + ".json";

                WriteFile(_modelsBlockPath + fileName, @"{
    ""parent"": ""block/cube_all"", 
    ""textures"": {
        ""all"": """ + _profile.Modid + @":" + _profile.TextureBlocksFolder + @"/" + name + @"""
    }
}");

                WriteFile(_blockstatesPath + fileName, @"{
    ""variants"": {
        """": [
            { ""model"": """ + _profile.Modid + @":block/" + name + @""" }
        ]
    }
        }");

                WriteFile(_modelsItemPath + fileName, @"{
    ""parent"": """ + _profile.Modid + @":block/" + name + @"""
}
            ");

                WriteFile(_lootTablePath + fileName, @"{
  ""type"": ""minecraft:block"",
  ""pools"": [
    {
      ""name"": """ + name + @""",
      ""rolls"": 1,
      ""entries"": [
        {
          ""type"": ""minecraft:item"",
          ""name"": """ + _profile.Modid + @":" + name + @"""
        }
      ],
      ""conditions"": [
        {
          ""condition"": ""minecraft:survives_explosion""
        }
      ]
    }
  ]
}");
            }
        }

        private void WriteFile(string path, string content)
        {
            if (File.Exists(path))
                File.Delete(path);

            File.WriteAllLines(path, new string[] { content });

            _filesGenerated++;
        }

        public int RenderJSON(bool blocks, bool stairs, bool walls, bool slabs, bool smooth)
        {
            _filesGenerated = 0;

            if (!Directory.Exists("out"))
                Directory.CreateDirectory("out");

            if (!Directory.Exists("out\\" + _profile.Modid))
                Directory.CreateDirectory("out\\" + _profile.Modid);

            if (!Directory.Exists("out\\" + _profile.Modid + "\\" + _profile.Version))
                Directory.CreateDirectory("out\\" + _profile.Modid + "\\" + _profile.Version);

            //  _basePath = "out\\" + _profile.Modid + "\\" + _profile.Version;

            if (!Directory.Exists(_basePath + "\\assets"))
                Directory.CreateDirectory(_basePath + "\\assets");
            
            if (!Directory.Exists(_basePath + "\\data"))
                Directory.CreateDirectory(_basePath + "\\data");

            if (!Directory.Exists(_basePath + "\\data\\" + _profile.Modid))
                Directory.CreateDirectory(_basePath + "\\data\\" + _profile.Modid);

            if (!Directory.Exists(_basePath + "\\data\\" + _profile.Modid + "\\loot_tables"))
                Directory.CreateDirectory(_basePath + "\\data\\" + _profile.Modid + "\\loot_tables");

            if (!Directory.Exists(_basePath + "\\data\\" + _profile.Modid + "\\loot_tables\\blocks"))
                Directory.CreateDirectory(_basePath + "\\data\\" + _profile.Modid + "\\loot_tables\\blocks");

            if (!Directory.Exists(_basePath + "\\assets\\" + _profile.Modid))
                Directory.CreateDirectory(_basePath + "\\assets\\" + _profile.Modid);

            if (!Directory.Exists(_blockstatesPath))
                Directory.CreateDirectory(_blockstatesPath);

            if (!Directory.Exists(_modelsPath))
                Directory.CreateDirectory(_modelsPath);

            if (!Directory.Exists(_modelsBlockPath))
                Directory.CreateDirectory(_modelsBlockPath);

            if (!Directory.Exists(_modelsItemPath))
                Directory.CreateDirectory(_modelsItemPath);

            if (blocks)
                RenderBlockJSON(false);

            if (smooth)
                RenderBlockJSON(true);

            return _filesGenerated;
        }
    }
}
