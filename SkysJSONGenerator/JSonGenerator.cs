﻿using System;
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

        private string _blockstatesPath;
        private string _modelsPath;
        private string _modelsBlockPath;
        private string _modelsItemPath;
        private string _lootTablePath;

        public JSonGenerator(Profile profile)
        {
            _profile = profile;

            _lootTablePath = "out\\data\\" + _profile.Modid + "\\loot_tables\\blocks";
            _blockstatesPath = "out\\assets\\" + _profile.Modid + "\\blockstates";
            _modelsPath = "out\\assets\\" + _profile.Modid + "\\models";
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
        }

        public void RenderJSON(bool blocks, bool stairs, bool walls, bool slabs, bool smooth)
        {
            if (!Directory.Exists("out"))
                Directory.CreateDirectory("out");
            
            if (!Directory.Exists("out\\assets"))
                Directory.CreateDirectory("out\\assets");
            
            if (!Directory.Exists("out\\data"))
                Directory.CreateDirectory("out\\data");

            if (!Directory.Exists("out\\data\\" + _profile.Modid))
                Directory.CreateDirectory("out\\data\\" + _profile.Modid);

            if (!Directory.Exists("out\\data\\" + _profile.Modid + "\\loot_tables"))
                Directory.CreateDirectory("out\\data\\" + _profile.Modid + "\\loot_tables");

            if (!Directory.Exists("out\\data\\" + _profile.Modid + "\\loot_tables\\blocks"))
                Directory.CreateDirectory("out\\data\\" + _profile.Modid + "\\loot_tables\\blocks");

            if (!Directory.Exists("out\\assets\\" + _profile.Modid))
                Directory.CreateDirectory("out\\assets\\" + _profile.Modid);

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
        }
    }
}
