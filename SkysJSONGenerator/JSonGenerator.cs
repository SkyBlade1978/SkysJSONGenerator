using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

        private void RenderStairJSON(bool smooth)
        {
            foreach (var item in _profile.Materials)
            {
                var materialName = item;

                if (smooth)
                    materialName += "_smooth";

                var name = materialName + "_stairs";

                var fileName = "\\" + name + ".json";

                var blockstate = $@"{{
                    ""variants"": {{
                        ""facing=east,half=bottom,shape=straight"":  {{ ""model"": ""{_profile.Modid}:block/{name}"" }},
                        ""facing=west,half=bottom,shape=straight"":  {{ ""model"": ""{_profile.Modid}:block/{name}"", ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=bottom,shape=straight"": {{ ""model"": ""{_profile.Modid}:block/{name}"", ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=bottom,shape=straight"": {{ ""model"": ""{_profile.Modid}:block/{name}"", ""y"": 270, ""uvlock"": true }},
                        ""facing=east,half=bottom,shape=outer_right"":  {{ ""model"": ""{_profile.Modid}:block/{name}_outer"" }},
                        ""facing=west,half=bottom,shape=outer_right"":  {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=bottom,shape=outer_right"": {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=bottom,shape=outer_right"": {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""y"": 270, ""uvlock"": true }},
                        ""facing=east,half=bottom,shape=outer_left"":  {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""y"": 270, ""uvlock"": true }},
                        ""facing=west,half=bottom,shape=outer_left"":  {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""y"": 90, ""uvlock"": true }},
                        ""facing=south,half=bottom,shape=outer_left"": {{ ""model"": ""{_profile.Modid}:block/{name}_outer"" }},
                        ""facing=north,half=bottom,shape=outer_left"": {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""y"": 180, ""uvlock"": true }},
                        ""facing=east,half=bottom,shape=inner_right"":  {{ ""model"": ""{_profile.Modid}:block/{name}_inner"" }},
                        ""facing=west,half=bottom,shape=inner_right"":  {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=bottom,shape=inner_right"": {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=bottom,shape=inner_right"": {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""y"": 270, ""uvlock"": true }},
                        ""facing=east,half=bottom,shape=inner_left"":  {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""y"": 270, ""uvlock"": true }},
                        ""facing=west,half=bottom,shape=inner_left"":  {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""y"": 90, ""uvlock"": true }},
                        ""facing=south,half=bottom,shape=inner_left"": {{ ""model"": ""{_profile.Modid}:block/{name}_inner"" }},
                        ""facing=north,half=bottom,shape=inner_left"": {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""y"": 180, ""uvlock"": true }},
                        ""facing=east,half=top,shape=straight"":  {{ ""model"": ""{_profile.Modid}:block/{name}"", ""x"": 180, ""uvlock"": true }},
                        ""facing=west,half=top,shape=straight"":  {{ ""model"": ""{_profile.Modid}:block/{name}"", ""x"": 180, ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=top,shape=straight"": {{ ""model"": ""{_profile.Modid}:block/{name}"", ""x"": 180, ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=top,shape=straight"": {{ ""model"": ""{_profile.Modid}:block/{name}"", ""x"": 180, ""y"": 270, ""uvlock"": true }},
                        ""facing=east,half=top,shape=outer_right"":  {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""x"": 180, ""y"": 90, ""uvlock"": true }},
                        ""facing=west,half=top,shape=outer_right"":  {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""x"": 180, ""y"": 270, ""uvlock"": true }},
                        ""facing=south,half=top,shape=outer_right"": {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""x"": 180, ""y"": 180, ""uvlock"": true }},
                        ""facing=north,half=top,shape=outer_right"": {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""x"": 180, ""uvlock"": true }},
                        ""facing=east,half=top,shape=outer_left"":  {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""x"": 180, ""uvlock"": true }},
                        ""facing=west,half=top,shape=outer_left"":  {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""x"": 180, ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=top,shape=outer_left"": {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""x"": 180, ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=top,shape=outer_left"": {{ ""model"": ""{_profile.Modid}:block/{name}_outer"", ""x"": 180, ""y"": 270, ""uvlock"": true }},
                        ""facing=east,half=top,shape=inner_right"":  {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""x"": 180, ""y"": 90, ""uvlock"": true }},
                        ""facing=west,half=top,shape=inner_right"":  {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""x"": 180, ""y"": 270, ""uvlock"": true }},
                        ""facing=south,half=top,shape=inner_right"": {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""x"": 180, ""y"": 180, ""uvlock"": true }},
                        ""facing=north,half=top,shape=inner_right"": {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""x"": 180, ""uvlock"": true }},
                        ""facing=east,half=top,shape=inner_left"":  {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""x"": 180, ""uvlock"": true }},
                        ""facing=west,half=top,shape=inner_left"":  {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""x"": 180, ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=top,shape=inner_left"": {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""x"": 180, ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=top,shape=inner_left"": {{ ""model"": ""{_profile.Modid}:block/{name}_inner"", ""x"": 180, ""y"": 270, ""uvlock"": true }}
                    }}
                }}";

                WriteFile(_blockstatesPath + fileName, blockstate);

                var model = $@"{{
                            ""parent"": ""block/stairs"",
                            ""textures"": {{
                                ""bottom"": ""{_profile.Modid}:{_profile.TextureBlocksFolder}/{materialName}"",
                                ""top"": ""{_profile.Modid}:{_profile.TextureBlocksFolder}/{materialName}"",
                                ""side"": ""{_profile.Modid}:{_profile.TextureBlocksFolder}/{materialName}""
                            }}
                        }}";

                var model_inner = $@"{{
                                ""parent"": ""block/inner_stairs"",
                                ""textures"": {{
                                    ""bottom"": ""{_profile.Modid}:{_profile.TextureBlocksFolder}/{materialName}"",
                                    ""top"": ""{_profile.Modid}:{_profile.TextureBlocksFolder}/{materialName}"",
                                    ""side"": ""{_profile.Modid}:{_profile.TextureBlocksFolder}/{materialName}""
                                }}
                            }}";

                var model_outer = $@"{{
                                ""parent"": ""block/outer_stairs"",
                                ""textures"": {{
                                    ""bottom"": ""{_profile.Modid}:{_profile.TextureBlocksFolder}/{materialName}"",
                                    ""top"": ""{_profile.Modid}:{_profile.TextureBlocksFolder}/{materialName}"",
                                    ""side"": ""{_profile.Modid}:{_profile.TextureBlocksFolder}/{materialName}""
                                }}
                            }}";


                WriteFile(_modelsBlockPath + fileName, model);
                WriteFile(_modelsBlockPath + "\\" + name + "_inner.json", model_inner);
                WriteFile(_modelsBlockPath + "\\" + name + "_outer.json", model_outer);

                var itemModel = @"{
                                    ""parent"": ""{_profile.Modid}:block/{name}""
                                }";


                WriteFile(_modelsItemPath + fileName, itemModel);

            }


        }

        private void RenderBlockJSON(bool smooth)
        {
            foreach (var item in _profile.Materials)
            {
                var name = item;

                if (smooth)
                    name += "_smooth";

                var fileName = "\\" + name + ".json";

                var modelBlock = $@"{{
                                    ""parent"": ""block/cube_all"", 
                                    ""textures"": {{
                                        ""all"": ""{_profile.Modid}:{_profile.TextureBlocksFolder}/{name}""
                                    }}
                                }}";

                WriteFile(_modelsBlockPath + fileName, modelBlock);

                var blockstate = $@"{{
                                    ""variants"": {{
                                        """": [
                                            {{ ""model"": ""{_profile.Modid}:block/{name}"" }}
                                        ]
                                    }}
                                  }}";

                WriteFile(_blockstatesPath + fileName, blockstate);

                var itemModel = $@"{{
                        ""parent"": ""{_profile.Modid}:block/{name}""
                    }}";

                WriteFile(_modelsItemPath + fileName, itemModel);

                var lootTable = $@"{{
                      ""type"": ""minecraft:block"",
                      ""pools"": [
                        {{
                          ""name"": ""{name}"",
                          ""rolls"": 1,
                          ""entries"": [
                            {{
                              ""type"": ""minecraft:item"",
                              ""name"": ""{_profile.Modid}:{name}""
                            }}
                          ],
                          ""conditions"": [
                            {{
                              ""condition"": ""minecraft:survives_explosion""
                            }}
                          ]
                        }}
                      ]
                    }}";

                WriteFile(_lootTablePath + fileName, lootTable);
            }
        }

        private void WriteFile(string path, string content)
        {
            if (File.Exists(path))
                File.Delete(path);

            JToken parsedJson = JToken.Parse(content);

            var beautified = parsedJson.ToString(Formatting.Indented);

            File.WriteAllLines(path, new string[] { beautified });

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
            {
                RenderBlockJSON(false);

                if (smooth)
                    RenderBlockJSON(true);
            }

            if (stairs)
            {
                RenderStairJSON(false);

                if (smooth)
                    RenderStairJSON(true);
            }

            return _filesGenerated;
        }
    }
}
