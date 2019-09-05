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

        // interpolated fields
        private string modid;
        private string blocktexturefolder;
        private string version;

        public JSonGenerator(Profile profile, string basePath)
        {
            modid = profile.Modid;
            blocktexturefolder = profile.TextureBlocksFolder;
            version = profile.Version;

            _profile = profile;

            _basePath = basePath;
            _lootTablePath = $"{_basePath}\\data\\{modid}\\loot_tables\\blocks";
            _blockstatesPath = $"{_basePath}\\assets\\{modid}\\blockstates";
            _modelsPath = $"{_basePath}\\assets\\{modid}\\models";
            _modelsBlockPath = _modelsPath + "\\block";
            _modelsItemPath = _modelsPath + "\\item";
        }

        private void RenderStairJSON(bool smooth, bool brick)
        {
            foreach (var item in _profile.Materials)
            {
                var materialname = item;

                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

                var blockname = materialname + "_stairs";

                var fileName = $"\\{blockname}.json";
  
                var blockstate = $@"{{
                    ""variants"": {{
                        ""facing=east,half=bottom,shape=straight"":  {{ ""model"": ""{modid}:block/{blockname}"" }},
                        ""facing=west,half=bottom,shape=straight"":  {{ ""model"": ""{modid}:block/{blockname}"", ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=bottom,shape=straight"": {{ ""model"": ""{modid}:block/{blockname}"", ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=bottom,shape=straight"": {{ ""model"": ""{modid}:block/{blockname}"", ""y"": 270, ""uvlock"": true }},
                        ""facing=east,half=bottom,shape=outer_right"":  {{ ""model"": ""{modid}:block/{blockname}_outer"" }},
                        ""facing=west,half=bottom,shape=outer_right"":  {{ ""model"": ""{modid}:block/{blockname}_outer"", ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=bottom,shape=outer_right"": {{ ""model"": ""{modid}:block/{blockname}_outer"", ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=bottom,shape=outer_right"": {{ ""model"": ""{modid}:block/{blockname}_outer"", ""y"": 270, ""uvlock"": true }},
                        ""facing=east,half=bottom,shape=outer_left"":  {{ ""model"": ""{modid}:block/{blockname}_outer"", ""y"": 270, ""uvlock"": true }},
                        ""facing=west,half=bottom,shape=outer_left"":  {{ ""model"": ""{modid}:block/{blockname}_outer"", ""y"": 90, ""uvlock"": true }},
                        ""facing=south,half=bottom,shape=outer_left"": {{ ""model"": ""{modid}:block/{blockname}_outer"" }},
                        ""facing=north,half=bottom,shape=outer_left"": {{ ""model"": ""{modid}:block/{blockname}_outer"", ""y"": 180, ""uvlock"": true }},
                        ""facing=east,half=bottom,shape=inner_right"":  {{ ""model"": ""{modid}:block/{blockname}_inner"" }},
                        ""facing=west,half=bottom,shape=inner_right"":  {{ ""model"": ""{modid}:block/{blockname}_inner"", ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=bottom,shape=inner_right"": {{ ""model"": ""{modid}:block/{blockname}_inner"", ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=bottom,shape=inner_right"": {{ ""model"": ""{modid}:block/{blockname}_inner"", ""y"": 270, ""uvlock"": true }},
                        ""facing=east,half=bottom,shape=inner_left"":  {{ ""model"": ""{modid}:block/{blockname}_inner"", ""y"": 270, ""uvlock"": true }},
                        ""facing=west,half=bottom,shape=inner_left"":  {{ ""model"": ""{modid}:block/{blockname}_inner"", ""y"": 90, ""uvlock"": true }},
                        ""facing=south,half=bottom,shape=inner_left"": {{ ""model"": ""{modid}:block/{blockname}_inner"" }},
                        ""facing=north,half=bottom,shape=inner_left"": {{ ""model"": ""{modid}:block/{blockname}_inner"", ""y"": 180, ""uvlock"": true }},
                        ""facing=east,half=top,shape=straight"":  {{ ""model"": ""{modid}:block/{blockname}"", ""x"": 180, ""uvlock"": true }},
                        ""facing=west,half=top,shape=straight"":  {{ ""model"": ""{modid}:block/{blockname}"", ""x"": 180, ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=top,shape=straight"": {{ ""model"": ""{modid}:block/{blockname}"", ""x"": 180, ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=top,shape=straight"": {{ ""model"": ""{modid}:block/{blockname}"", ""x"": 180, ""y"": 270, ""uvlock"": true }},
                        ""facing=east,half=top,shape=outer_right"":  {{ ""model"": ""{modid}:block/{blockname}_outer"", ""x"": 180, ""y"": 90, ""uvlock"": true }},
                        ""facing=west,half=top,shape=outer_right"":  {{ ""model"": ""{modid}:block/{blockname}_outer"", ""x"": 180, ""y"": 270, ""uvlock"": true }},
                        ""facing=south,half=top,shape=outer_right"": {{ ""model"": ""{modid}:block/{blockname}_outer"", ""x"": 180, ""y"": 180, ""uvlock"": true }},
                        ""facing=north,half=top,shape=outer_right"": {{ ""model"": ""{modid}:block/{blockname}_outer"", ""x"": 180, ""uvlock"": true }},
                        ""facing=east,half=top,shape=outer_left"":  {{ ""model"": ""{modid}:block/{blockname}_outer"", ""x"": 180, ""uvlock"": true }},
                        ""facing=west,half=top,shape=outer_left"":  {{ ""model"": ""{modid}:block/{blockname}_outer"", ""x"": 180, ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=top,shape=outer_left"": {{ ""model"": ""{modid}:block/{blockname}_outer"", ""x"": 180, ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=top,shape=outer_left"": {{ ""model"": ""{modid}:block/{blockname}_outer"", ""x"": 180, ""y"": 270, ""uvlock"": true }},
                        ""facing=east,half=top,shape=inner_right"":  {{ ""model"": ""{modid}:block/{blockname}_inner"", ""x"": 180, ""y"": 90, ""uvlock"": true }},
                        ""facing=west,half=top,shape=inner_right"":  {{ ""model"": ""{modid}:block/{blockname}_inner"", ""x"": 180, ""y"": 270, ""uvlock"": true }},
                        ""facing=south,half=top,shape=inner_right"": {{ ""model"": ""{modid}:block/{blockname}_inner"", ""x"": 180, ""y"": 180, ""uvlock"": true }},
                        ""facing=north,half=top,shape=inner_right"": {{ ""model"": ""{modid}:block/{blockname}_inner"", ""x"": 180, ""uvlock"": true }},
                        ""facing=east,half=top,shape=inner_left"":  {{ ""model"": ""{modid}:block/{blockname}_inner"", ""x"": 180, ""uvlock"": true }},
                        ""facing=west,half=top,shape=inner_left"":  {{ ""model"": ""{modid}:block/{blockname}_inner"", ""x"": 180, ""y"": 180, ""uvlock"": true }},
                        ""facing=south,half=top,shape=inner_left"": {{ ""model"": ""{modid}:block/{blockname}_inner"", ""x"": 180, ""y"": 90, ""uvlock"": true }},
                        ""facing=north,half=top,shape=inner_left"": {{ ""model"": ""{modid}:block/{blockname}_inner"", ""x"": 180, ""y"": 270, ""uvlock"": true }}
                    }}
                }}";

                WriteFile(_blockstatesPath + fileName, blockstate);

                var model = $@"{{
                            ""parent"": ""block/stairs"",
                            ""textures"": {{
                                ""bottom"": ""{modid}:{blocktexturefolder}/{materialname}"",
                                ""top"": ""{modid}:{blocktexturefolder}/{materialname}"",
                                ""side"": ""{modid}:{blocktexturefolder}/{materialname}""
                            }}
                        }}";

                var model_inner = $@"{{
                                ""parent"": ""block/inner_stairs"",
                                ""textures"": {{
                                    ""bottom"": ""{modid}:{blocktexturefolder}/{materialname}"",
                                    ""top"": ""{modid}:{blocktexturefolder}/{materialname}"",
                                    ""side"": ""{modid}:{blocktexturefolder}/{materialname}""
                                }}
                            }}";

                var model_outer = $@"{{
                                ""parent"": ""block/outer_stairs"",
                                ""textures"": {{
                                    ""bottom"": ""{modid}:{blocktexturefolder}/{materialname}"",
                                    ""top"": ""{modid}:{blocktexturefolder}/{materialname}"",
                                    ""side"": ""{modid}:{blocktexturefolder}/{materialname}""
                                }}
                            }}";

                WriteFile(_modelsBlockPath + fileName, model);
                WriteFile($"{_modelsBlockPath}\\{blockname}_inner.json", model_inner);
                WriteFile($"{_modelsBlockPath}\\{blockname}_outer.json", model_outer);

                var itemModel = $@"{{
                                    ""parent"": ""{modid}:block/{blockname}""
                                }}";


                WriteFile(_modelsItemPath + fileName, itemModel);

                var lootTable = $@"{{
                      ""type"": ""minecraft:block"",
                      ""pools"": [
                        {{
                          ""name"": ""{blockname}"",
                          ""rolls"": 1,
                          ""entries"": [
                            {{
                              ""type"": ""minecraft:item"",
                              ""name"": ""{modid}:{blockname}""
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
                                        ""all"": ""{modid}:{blocktexturefolder}/{name}""
                                    }}
                                }}";

                WriteFile(_modelsBlockPath + fileName, modelBlock);

                var blockstate = $@"{{
                                    ""variants"": {{
                                        """": [
                                            {{ ""model"": ""{modid}:block/{name}"" }}
                                        ]
                                    }}
                                  }}";

                WriteFile(_blockstatesPath + fileName, blockstate);

                var itemModel = $@"{{
                        ""parent"": ""{modid}:block/{name}""
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
                              ""name"": ""{modid}:{name}""
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

        private void RenderBrickBlockJSON(bool smooth)
        {
            foreach (var item in _profile.Materials)
            {
                var name = item;

                if (smooth)
                    name += "_smooth";

                name += "_brick";

                var fileName = "\\" + name + ".json";

                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                Block block = _profile.Blocks.Find(b => b.Name == "Brick");

                if (!smooth) /// TODO: this logic is very minealogy specific, make generic and/or make _side and _top textures
                {
                    if (block.Side)
                        sideSuffix = "_side";

                    if (block.Side)
                        topSuffix = "_top";
                }

                var modelBlock = $@"{{
                        ""parent"": ""block/cube_column"",
                        ""textures"": {{
                            ""end"": ""{modid}:{blocktexturefolder}/{name}{topSuffix}"",
                            ""side"": ""{modid}:{blocktexturefolder}/{name}{sideSuffix}""
                        }}
                    }}";


                WriteFile(_modelsBlockPath + fileName, modelBlock);

                var blockstate = $@"{{
                                    ""variants"": {{
                                        """": [
                                            {{ ""model"": ""{modid}:block/{name}"" }}
                                        ]
                                    }}
                                  }}";

                WriteFile(_blockstatesPath + fileName, blockstate);

                var itemModel = $@"{{
                        ""parent"": ""{modid}:block/{name}""
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
                              ""name"": ""{modid}:{name}""
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

        public int RenderJSON(bool blocks, bool stairs, bool walls, bool slabs, bool smooth, bool brick)
        {
            _filesGenerated = 0;

            if (!Directory.Exists("out"))
                Directory.CreateDirectory("out");

            if (!Directory.Exists("out\\" + modid))
                Directory.CreateDirectory("out\\" + modid);

            if (!Directory.Exists("out\\" + modid + "\\" + _profile.Version))
                Directory.CreateDirectory("out\\" + modid + "\\" + _profile.Version);

            //  _basePath = "out\\" + modid + "\\" + _profile.Version;

            if (!Directory.Exists(_basePath + "\\assets"))
                Directory.CreateDirectory(_basePath + "\\assets");
            
            if (!Directory.Exists(_basePath + "\\data"))
                Directory.CreateDirectory(_basePath + "\\data");

            if (!Directory.Exists(_basePath + "\\data\\" + modid))
                Directory.CreateDirectory(_basePath + "\\data\\" + modid);

            if (!Directory.Exists(_basePath + "\\data\\" + modid + "\\loot_tables"))
                Directory.CreateDirectory(_basePath + "\\data\\" + modid + "\\loot_tables");

            if (!Directory.Exists(_basePath + "\\data\\" + modid + "\\loot_tables\\blocks"))
                Directory.CreateDirectory(_basePath + "\\data\\" + modid + "\\loot_tables\\blocks");

            if (!Directory.Exists(_basePath + "\\assets\\" + modid))
                Directory.CreateDirectory(_basePath + "\\assets\\" + modid);

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

                if (brick)
                    RenderBrickBlockJSON(false);

                if (brick && smooth)
                    RenderBrickBlockJSON(true);
            }

            if (stairs)
            {
                RenderStairJSON(false, false);

                if (smooth)
                    RenderStairJSON(true, false);

                if (brick)
                    RenderStairJSON(false, true);

                if (brick && smooth)
                    RenderStairJSON(true, true);
            }

            return _filesGenerated;
        }
    }
}
