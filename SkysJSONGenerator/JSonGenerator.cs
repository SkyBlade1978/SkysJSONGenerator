using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SkysJSONGenerator
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($@"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }

    public class JSonGenerator
    {
        public delegate void FileProcessed(int counter);
        public event FileProcessed FileProcessedEvent;

        private readonly Profile _profile;

        private readonly string _baseTemplateFolder;
        private readonly string _blockModelTemplateFolder;
        private readonly string _itemModelTemplateFolder;
        private readonly string _blockstateTemplateFolder;
        private readonly string _lootTableTemplateFolder;
        private readonly string _langTemplateFolder;
        private readonly string _tagTemplateFolder;
        private readonly string _advancementTemplateFolder;
        private readonly string _recipeTemplateFolder;
        private readonly string _wallItemTagPath;
        private readonly string _wallTagPath;
        private readonly string _basePath;
        private readonly string _blockstatesPath;
        private readonly string _modelsPath;
        private readonly string _modelsBlockPath;
        private readonly string _modelsItemPath;
        private readonly string _lootTablePath;
        private readonly string _langPath;
        private readonly string _recipeAdvancementsPath;
        private readonly string _recipePath;
        private readonly string _advancementsPath;
        private int _filesGenerated;

        // interpolated fields
        private readonly string modid;
        private readonly string blocktexturefolder;
        private readonly string version;

        string _wallTags = "";

        private bool _renderAdvancement;
        private bool _renderRecipe;

        public JSonGenerator(Profile profile, string basePath)
        {
            modid = profile.Modid;
            blocktexturefolder = profile.TextureBlocksFolder;
            version = profile.Version;

            _profile = profile;

            _basePath = basePath;
            _lootTablePath = $"{_basePath}\\data\\{modid}\\loot_tables\\blocks";
            _blockstatesPath = $"{_basePath}\\assets\\{modid}\\blockstates";
            _advancementsPath = $"{_basePath}\\assets\\{modid}\\advancements\\{modid}";
            _recipeAdvancementsPath = $"{_basePath}\\assets\\{modid}\\advancements\\recipes";


            double versionNumber;

            double.TryParse(version, out versionNumber);

            if (versionNumber >= 1.14)
                _recipePath = $"{_basePath}\\data\\{modid}\\recipes";
            else
                _recipePath = $"{_basePath}\\assets\\{modid}\\recipes";


            _langPath = $"{_basePath}\\assets\\{modid}\\lang";
            _modelsPath = $"{_basePath}\\assets\\{modid}\\models";
            _modelsBlockPath = _modelsPath + "\\block";
            _modelsItemPath = _modelsPath + "\\item";
            _wallTagPath = $"{_basePath}\\data\\minecraft\\tags\\blocks";
            _wallItemTagPath = $"{_basePath}\\data\\minecraft\\tags\\items";

            _baseTemplateFolder = $"templates\\{version}";
            _blockModelTemplateFolder = _baseTemplateFolder + "\\blockmodels";
            _itemModelTemplateFolder = _baseTemplateFolder + "\\itemmodels";
            _blockstateTemplateFolder = _baseTemplateFolder + "\\blockstates";
            _lootTableTemplateFolder = _baseTemplateFolder + "\\loot_table";
            _langTemplateFolder = _baseTemplateFolder + "\\lang";
            _tagTemplateFolder = _baseTemplateFolder + "\\tags";
            _advancementTemplateFolder = _baseTemplateFolder + "\\advancement";
            _recipeTemplateFolder = _baseTemplateFolder + "\\recipe";


        }

        private string LoadTemplate(TemplateData data)
        {
            var overridePath = data.Path + "\\" + modid + "_" + data.Name + ".template";
            var fullPath = data.Path + "\\" + data.Name + ".template";

            var texture1 = string.Empty;
            var texture2 = string.Empty;
            var texture3 = string.Empty;
            var texture4 = string.Empty;


            var ingredient1 = new KeyValuePair<string, int>("", 0);
            var ingredient2 = new KeyValuePair<string, int>("", 0);
            var ingredient3 = new KeyValuePair<string, int>("", 0);

            if (File.Exists(overridePath))
                fullPath = overridePath;

            if (!File.Exists(fullPath) && fullPath.Contains("conditional"))
                fullPath = fullPath.Replace("_conditional", "");

            if (File.Exists(fullPath))
            {
                var template = File.ReadAllText(fullPath).Replace("{modid}", "{0}").Replace("{blockname}", "{1}")
                    .Replace("{materialname}", "{2}").Replace("{topsuffix}", "{3}")
                    .Replace("{sidesuffix}", "{4}").Replace("{blocktexturefolder}", "{5}")
                    .Replace("{walllist}", "{6}").Replace("{langname}", "{7}")
                    .Replace("{texture1}", "{8}").Replace("{texture2}", "{9}")
                    .Replace("{texture3}", "{10}").Replace("{smoothsuffix}", "{11}")
                    .Replace("{bricksuffix}", "{12}").Replace("{ingredient1}", "{13}").Replace("{meta1}", "{14}")
                    .Replace("{ingredient2}", "{15}").Replace("{meta2}", "{16}")
                    .Replace("{ingredient3}", "{17}").Replace("{meta3}", "{18}")
                    .Replace("{conditions}", "{19}").Replace("{ingredientdomain}", "{20}")
                    .Replace("{domain}", "{21}").Replace("{colourMeta}", "{22}")
                    .Replace("{texture4}", "{23}");
                
                if (data.Textures.Length > 0)
                    texture1 = data.Textures[0];

                if (data.Textures.Length > 1)
                    texture2 = data.Textures[1];

                if (data.Textures.Length > 2)
                    texture3 = data.Textures[2];

                if (data.Textures.Length > 3)
                    texture4 = data.Textures[3];

                if (data.Ingredients.Count > 0)
                    ingredient1 = data.Ingredients[0];

                if (data.Ingredients.Count > 1)
                    ingredient2 = data.Ingredients[1];

                if (data.Ingredients.Count > 2)
                    ingredient3 = data.Ingredients[2];

                var ingredientDomain = string.Empty;
                
                if (data.IngredientDomain != "" && data.IngredientDomain != "minecraft")
                    ingredientDomain = data.IngredientDomain + "_";

                try
                {
                    return string.Format(@template, modid, data.BlockName, data.MaterialName, data.TopSuffix, data.SideSuffix, blocktexturefolder, 
                        data.WallList, data.LangName, texture1, texture2, texture3, data.SmoothSuffix, data.BrickSuffix, 
                        ingredient1.Key, ingredient1.Value, ingredient2.Key, ingredient2.Value, ingredient3.Key, ingredient3.Value,
                        data.Conditions, ingredientDomain, data.IngredientDomain, data.ColourMeta, texture4);
                }
                catch (Exception)
                {
                    MessageBox.Show(@"Whoops, there appears to be an error in template file:" + fullPath);
                    throw;
                }
            }
            else
            {
                Debug.Print("Template not found: " + fullPath);
                return string.Empty;
            }
        }

        private async Task RenderStairJSON(bool smooth, bool brick)
        {
            await RenderStairJSON(smooth, brick, false);
        }

        private async Task RenderStairJSON(bool smooth, bool brick, bool wood)
        {
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var smoothSuffix = string.Empty;
                var brickSuffix = string.Empty;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);
                
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                if (smooth) materialname += "_smooth";
                if (brick) materialname += "_brick";

                var block = _profile.Blocks.Find(b => b.Name == "Brick");

                if (brick)
                {
                    if (block.Side)
                        sideSuffix = "_side";

                    if (block.Side)
                        topSuffix = "_top";
                }

                if (wood)
                {
                    sideSuffix = "_planks";
                    topSuffix = "_planks";
                }

                var blockname = materialname + "_stairs";

                var fileName = $"\\{blockname}.json";

                var data = new TemplateData
                {
                    Path = _blockstateTemplateFolder, Name = "stairs", BlockName = blockname,
                    MaterialName = materialname, TopSuffix = topSuffix, SideSuffix = sideSuffix, Textures = textures,
                    SmoothSuffix = smoothSuffix, BrickSuffix = brickSuffix, Ingredients = ingredients
                };
                
                tasks.Add(WriteFile(_blockstatesPath + fileName, @LoadTemplate(data)));
                    tasks.Add(WriteFile(_modelsBlockPath + fileName, @LoadTemplate(data.WithPath(_blockModelTemplateFolder))));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{blockname}_inner.json",
                        @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("inner_stairs"))));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{blockname}_outer.json",
                        @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("outer_stairs"))));
                tasks.Add(WriteFile(_modelsItemPath + fileName, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));
                tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));
                
                if (_renderAdvancement)
                    tasks.Add(WriteFile(_recipeAdvancementsPath + fileName, @LoadTemplate(data.WithPath(_advancementTemplateFolder).WithName("recipe"))));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task RenderDoorJSON()
        {
            foreach (var item in _profile.Materials)
            {

                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);
                
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                var blockname = materialname + "_door";

                var fileName = $"\\{blockname}.json";

                var data = new TemplateData
                {
                    Path = _blockstateTemplateFolder,
                    Name = "door",
                    BlockName = blockname,
                    MaterialName = materialname,
                    TopSuffix = topSuffix,
                    SideSuffix = sideSuffix,
                    Textures = textures,
                    Ingredients = ingredients
                };

                var tasks = new List<Task>
                {
                    WriteFile(_blockstatesPath + fileName, @LoadTemplate(data)),
                    WriteFile(_modelsBlockPath + fileName, @LoadTemplate(data.WithPath(_blockModelTemplateFolder))),
                    WriteFile($"{_modelsBlockPath}\\{blockname}_bottom.json",
                        @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("door_bottom"))),
                    WriteFile($"{_modelsBlockPath}\\{blockname}_bottom_rh.json",
                        @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("door_bottom_rh"))),
                    WriteFile($"{_modelsBlockPath}\\{blockname}_top.json",
                        @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("door_top"))),
                    WriteFile($"{_modelsBlockPath}\\{blockname}_top_rh.json",
                        @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("door_top_rh"))),
                    WriteFile(_modelsItemPath + fileName, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))),
                    WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder)))
                };
                
                await Task.WhenAll(tasks.ToArray());
            }
        }

        private async Task RenderSlabJSON(bool smooth, bool brick, bool wood)
        {
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);
                
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

                var block = _profile.Blocks.Find(b => b.Name == "Slabs");

                if (brick)
                {
                    if (block.Side)
                        sideSuffix = "_side";

                    if (block.Side)
                        topSuffix = "_top";
                }

                if (wood)
                {
                    topSuffix = "_planks";
                    sideSuffix = "_planks";
                }

                var blockname = materialname + "_slab";

                var fileName = $"\\{blockname}.json";

                var data = new TemplateData
                {
                    Path = _blockstateTemplateFolder,
                    Name = "slab",
                    BlockName = blockname,
                    MaterialName = materialname,
                    TopSuffix = topSuffix,
                    SideSuffix = sideSuffix,
                    Textures = textures,
                    Ingredients = ingredients
                };

                tasks.Add(WriteFile(_blockstatesPath + fileName, @LoadTemplate(data)));

                tasks.Add(WriteFile(_modelsBlockPath + fileName,
                    @LoadTemplate(data.WithPath(_blockModelTemplateFolder))));

                tasks.Add(WriteFile($"{_modelsBlockPath}\\upper_{blockname}.json",
                    @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("upper_slab"))));

                tasks.Add(WriteFile($"{_modelsBlockPath}\\half_{blockname}.json",
                    @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("half_slab"))));

                tasks.Add(WriteFile(_modelsItemPath + fileName,
                    @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));

                tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));

                tasks.Add(WriteFile(_recipeAdvancementsPath + fileName,
                    @LoadTemplate(data.WithPath(_advancementTemplateFolder).WithName("recipe"))));

            }

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task RenderReleifJSON(bool langs)
        {
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);
                
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;
                
                var blockname = materialname + "_relief";
                
                foreach (var file in Directory.EnumerateFiles(_blockModelTemplateFolder))
                {
                    if (!file.Contains("relief"))
                        continue;

                    var filenameArray = file.Split('_');
                    var reliefName = filenameArray[1].Split('.');
                    var templateName = $"relief_{reliefName[0]}";

                    string blockModelFilename;
                    if (filenameArray.Length > 2)
                    {
                        blockModelFilename = $"\\{blockname}_{reliefName[0]}_inventory.json";
                        templateName = $"relief_{reliefName[0]}_inventory";
                    }
                    else
                        blockModelFilename = $"\\{blockname}_{reliefName[0]}.json";

                    var data = new TemplateData
                    {
                        Path = _blockModelTemplateFolder,
                        Name = templateName,
                        BlockName = blockname,
                        MaterialName = materialname,
                        TopSuffix = topSuffix,
                        SideSuffix = sideSuffix,
                        Textures = textures,
                        LangName = materialname.FirstCharToUpper() + " " + reliefName[0].FirstCharToUpper() + " Relief",
                        SmoothSuffix = "_smooth",
                        Ingredients = ingredients
                    };

                    await WriteFile(_modelsBlockPath + blockModelFilename, @LoadTemplate(data));

                    if (filenameArray.Length >= 3)
                        continue;

                    tasks.Add(WriteFile(_blockstatesPath + blockModelFilename,
                        @LoadTemplate(data.WithPath(_blockstateTemplateFolder).WithName("relief")
                            .WithBlockName(blockname + $"_{reliefName[0]}"))));

                    tasks.Add(WriteFile(_modelsItemPath + blockModelFilename, @LoadTemplate(data.WithPath(_itemModelTemplateFolder).WithName("relief")
                        .WithBlockName(blockname + $"_{ reliefName[0]}"))));

                    if (langs)
                        AppendFile(_langPath + "\\en_EN.lang", @LoadTemplate(data.WithPath(_langTemplateFolder).WithName("lang").WithBlockName(blockname + $"_{ reliefName[0]}")));

                    tasks.Add(WriteFile(_recipeAdvancementsPath + blockModelFilename,
                        @LoadTemplate(data.WithPath(_advancementTemplateFolder).WithName("recipe").WithBlockName($"{blockname}_{reliefName[0]}"))));
                }
                
                //WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "relief", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task RenderFurnaceJSON(bool smooth, bool brick)
        {
            var tasks = new List<Task>();
            
            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);

                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

                var block = _profile.Blocks.Find(b => b.Name == "Furnace");

                if (brick)
                {
                    if (block.Side)
                        sideSuffix = "_side";

                    if (block.Side)
                        topSuffix = "_top";
                }

                var blockname = materialname + "_furnace";

                var fileName = $"\\{blockname}.json";
                var fileNameLit = $"\\lit_{blockname}.json";

                var data = new TemplateData
                {
                    Path = _blockstateTemplateFolder,
                    Name = "furnace",
                    BlockName = blockname,
                    MaterialName = materialname,
                    TopSuffix = topSuffix,
                    SideSuffix = sideSuffix,
                    Textures = textures,
                    Ingredients = ingredients
                };

                tasks.Add(WriteFile(_blockstatesPath + fileName, @LoadTemplate(data)));
                tasks.Add(WriteFile(_modelsBlockPath + fileName, @LoadTemplate(data.WithPath(_blockModelTemplateFolder))));
                tasks.Add(WriteFile(_blockstatesPath + fileNameLit, @LoadTemplate(data.WithName("lit_furnace"))));
                tasks.Add(WriteFile(_modelsBlockPath + fileNameLit, @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("lit_furnace"))));
                tasks.Add(WriteFile(_modelsItemPath + fileName, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));
                tasks.Add(WriteFile(_modelsItemPath + fileNameLit, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));
                tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));
                tasks.Add(WriteFile(_recipeAdvancementsPath + fileName,
                    @LoadTemplate(data.WithPath(_advancementTemplateFolder).WithName("recipe"))));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task RenderWallJSON(bool smooth, bool brick)
        {
            TemplateData data;
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);

                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;
                
                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

                var block = _profile.Blocks.Find(b => b.Name == "Walls");

                if (block.Side && brick)
                    sideSuffix = "_side";

                if (block.Side && brick)
                    topSuffix = "_top";

                var blockname = materialname + "_wall";
                var fileName = $"\\{blockname}.json";

                if (_wallTags != "")
                    _wallTags += ", ";

                _wallTags += "\"" + modid + ":" + blockname + "\"";

                data = new TemplateData
                {
                    Path = _blockstateTemplateFolder,
                    Name = "wall",
                    BlockName = blockname,
                    MaterialName = materialname,
                    TopSuffix = topSuffix,
                    SideSuffix = sideSuffix,
                    Textures = textures,
                    WallList = _wallTags,
                    Ingredients = ingredients
                };


                tasks.Add(WriteFile(_blockstatesPath + fileName, @LoadTemplate(data)));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{blockname}_inventory.json", @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("wall_inventory"))));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{blockname}_post.json", @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("wall_post"))));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{blockname}_side.json", @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("wall_side"))));
                tasks.Add(WriteFile(_modelsItemPath + fileName, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));
                tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));
                tasks.Add(WriteFile(_recipeAdvancementsPath + fileName,
                        @LoadTemplate(data.WithPath(_advancementTemplateFolder).WithName("recipe"))));
            }

            data = new TemplateData
            {
                Path = _blockstateTemplateFolder,
                Name = "wall",
                WallList = _wallTags
            };

            tasks.Add(WriteFile(_wallTagPath + "\\walls.json", @LoadTemplate(data.WithPath(_tagTemplateFolder))));
            tasks.Add(WriteFile(_wallItemTagPath + "\\walls.json", @LoadTemplate(data.WithPath(_tagTemplateFolder))));

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task RenderFenceJSON()
        {
            TemplateData data;
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);
                
                var blockname = materialname + "_fence";
                var fileName = $"\\{blockname}.json";

                if (_wallTags != "")
                    _wallTags += ", ";

                _wallTags += "\"" + modid + ":" + blockname + "\"";

                data = new TemplateData
                {
                    Path = _blockstateTemplateFolder,
                    Name = "fence",
                    BlockName = blockname,
                    MaterialName = materialname,
                    Textures = textures,
                    WallList = _wallTags,
                    Ingredients = ingredients
                };


                tasks.Add(WriteFile(_blockstatesPath + fileName, @LoadTemplate(data)));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{blockname}_inventory.json", @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("fence_inventory"))));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{blockname}_post.json", @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("fence_post"))));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{blockname}_side.json", @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("fence_side"))));
                tasks.Add(WriteFile(_modelsItemPath + fileName, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));
                tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));
                tasks.Add(WriteFile(_recipeAdvancementsPath + fileName,
                        @LoadTemplate(data.WithPath(_advancementTemplateFolder).WithName("recipe"))));
            }

            data = new TemplateData
            {
                Path = _blockstateTemplateFolder,
                Name = "fence",
                WallList = _wallTags
            };

            tasks.Add(WriteFile(_wallTagPath + "\\walls.json", @LoadTemplate(data.WithPath(_tagTemplateFolder))));
            tasks.Add(WriteFile(_wallItemTagPath + "\\walls.json", @LoadTemplate(data.WithPath(_tagTemplateFolder))));

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task RenderGateJSON()
        {
            TemplateData data;
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);

                var blockname = materialname + "_fence_gate";
                var fileName = $"\\{blockname}.json";

                var alternateBlockname = materialname + "_fence_gate";

                if (_wallTags != "")
                    _wallTags += ", ";

                _wallTags += "\"" + modid + ":" + blockname + "\"";

                data = new TemplateData
                {
                    Path = _blockstateTemplateFolder,
                    Name = "fence_gate",
                    BlockName = blockname,
                    MaterialName = materialname,
                    Textures = textures,
                    WallList = _wallTags,
                    Ingredients = ingredients
                };


                tasks.Add(WriteFile(_blockstatesPath + fileName, @LoadTemplate(data)));
                
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{blockname}_closed.json", @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("fence_gate_closed"))));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{blockname}_open.json", @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("fence_gate_open"))));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{alternateBlockname}_closed.json", @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("wall_gate_closed"))));
                tasks.Add(WriteFile($"{_modelsBlockPath}\\{alternateBlockname}_open.json", @LoadTemplate(data.WithPath(_blockModelTemplateFolder).WithName("wall_gate_open"))));
                tasks.Add(WriteFile(_modelsItemPath + fileName, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));
                tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));
                tasks.Add(WriteFile(_recipeAdvancementsPath + fileName,
                        @LoadTemplate(data.WithPath(_advancementTemplateFolder).WithName("recipe"))));
            }

            data = new TemplateData
            {
                Path = _blockstateTemplateFolder,
                Name = "wall",
                WallList = _wallTags
            };

            tasks.Add(WriteFile(_wallTagPath + "\\walls.json", @LoadTemplate(data.WithPath(_tagTemplateFolder))));
            tasks.Add(WriteFile(_wallItemTagPath + "\\walls.json", @LoadTemplate(data.WithPath(_tagTemplateFolder))));

            await Task.WhenAll(tasks.ToArray());
        }

        private static string GetNaturaBlockFolder(string input, string materialname)
        { 
            var overUnder = "overworld";

            if (materialname.Contains("bloodwood") || materialname.Contains("darkwood") ||
                materialname.Contains("fusewood") || materialname.Contains("ghostwood"))
                overUnder = "nether";

            if (input.Contains("bark"))
                return $"blocks/logs/{overUnder}";

            if (input.Contains("log"))
               return $"blocks/logs/{overUnder}";

            if (input.Contains("heart"))
                return $"blocks/logs/{overUnder}";

            if (input.Contains("plank"))
                return $"blocks/planks/{overUnder}";

            return  "blocks";
        }

        private List<KeyValuePair<string, int>> GetIngredients(string[] arrayIn, int startPos, string materialname)
        {
            var returnList = new List<KeyValuePair<string, int>>();

            for (var i = startPos; i < arrayIn.Length; i++)
            {
                if (arrayIn.Length > i + 2)
                    returnList.Add(new KeyValuePair<string, int>(arrayIn[i] + ":" + arrayIn[i+1].Replace("{materialname}", materialname), int.Parse(arrayIn[i+2])));
                else
                    returnList.Add(new KeyValuePair<string, int>(arrayIn[i] + ":" + arrayIn[i + 1].Replace("{materialname}", materialname), -1));
                
                i++; i++;
            }

            return returnList;
        }

        private string[] GetTextureOverrides(string materialname, out string materialNameOut, out string domainOut, out List<KeyValuePair<string, int>> ingredientsOut)
        {
            var domain = modid;

            var texture1 = domain + ":" + blocktexturefolder + "\\" + materialname;
            var texture2 = domain + ":" + blocktexturefolder + "\\" + materialname;
            var texture3 = domain + ":" + blocktexturefolder + "\\" + materialname;
            var texture4 = domain + ":" + blocktexturefolder + "\\" + materialname;


            var ingredients = new List<KeyValuePair<string, int>>();

            if (materialname.Contains(":"))
            {
                var materialNameArray = materialname.Split(':');

                domain = materialNameArray[0];
                materialname = materialNameArray[1];

                double decVersion;

                double.TryParse(this.version, out decVersion);

                // natura has it's own "special" way of structuring it's assets :(
                if (domain == "natura")
                {
                    Debug.Print("natura");

                    if (materialNameArray.Length > 2)
                        if (materialNameArray[2] == "ingredients")
                            ingredients = GetIngredients(materialNameArray, 3, materialname);
                        else
                            texture1 = domain + ":" + GetNaturaBlockFolder(materialNameArray[2], materialname) + "/" +
                                   materialNameArray[2].Replace("{materialname}", materialname);
                    else
                        texture1 = string.Empty;

                    if (materialNameArray.Length > 3)
                        if (materialNameArray[3] == "ingredients")
                            ingredients = GetIngredients(materialNameArray, 4, materialname);
                        else
                        if (ingredients.Count == 0)
                            texture2 = domain + ":" + GetNaturaBlockFolder(materialNameArray[3], materialname) + "/" +
                                   materialNameArray[3].Replace("{materialname}", materialname);
                    else
                        texture2 = string.Empty;

                    if (materialNameArray.Length > 4)
                        if (materialNameArray[4] == "ingredients")
                            ingredients = GetIngredients(materialNameArray, 5, materialname);
                        else
                        if (ingredients.Count == 0)
                            texture3 = domain + ":" + GetNaturaBlockFolder(materialNameArray[4], materialname) + "/" +
                                   materialNameArray[4].Replace("{materialname}", materialname);
                    else
                        texture3 = string.Empty;

                    if (materialNameArray.Length > 5)
                        if (materialNameArray[5] == "ingredients")
                            ingredients = GetIngredients(materialNameArray, 6, materialname);
                        else
                        if (ingredients.Count == 0)
                            texture4 = domain + ":" + GetNaturaBlockFolder(materialNameArray[5], materialname) + "/" +
                                       materialNameArray[5].Replace("{materialname}", materialname);
                        else
                            texture4 = string.Empty;
                } else
                {
                    string workingBlockTextureFolder;


                    if (domain == "minecraft" && decVersion >= 1.14)
                        workingBlockTextureFolder = "block";
                    else
                    {
                        if (domain == "byg" && decVersion == 1.15)
                            workingBlockTextureFolder = "blocks";
                        else
                            workingBlockTextureFolder = blocktexturefolder;
                    }

                    if (domain == "forestry")
                    {
                        workingBlockTextureFolder = blocktexturefolder + "/wood";
                    }

                    if (materialNameArray.Length > 2)
                        if (materialNameArray[2] == "ingredients")
                            ingredients = GetIngredients(materialNameArray, 3, materialname);
                        else
                        if (ingredients.Count == 0)
                            texture1 = domain + ":" + workingBlockTextureFolder + "/" + materialNameArray[2].Replace("{materialname}", materialname);
                    else
                        texture1 = string.Empty;

                    if (materialNameArray.Length > 3)
                        if (materialNameArray[3] == "ingredients")
                            ingredients = GetIngredients(materialNameArray, 4, materialname);
                        else
                        if (ingredients.Count == 0)
                            texture2 = domain + ":" + workingBlockTextureFolder + "/" + materialNameArray[3].Replace("{materialname}", materialname);
                    else
                        texture2 = string.Empty;

                    if (materialNameArray.Length > 4)
                        if (materialNameArray[4] == "ingredients")
                            ingredients = GetIngredients(materialNameArray, 5, materialname);
                        else
                        if (ingredients.Count == 0)
                            texture3 = domain + ":" + workingBlockTextureFolder + "/" + materialNameArray[4].Replace("{materialname}", materialname);
                    else
                        texture3 = string.Empty;

                    if (materialNameArray.Length > 5)
                        if (materialNameArray[5] == "ingredients")
                            ingredients = GetIngredients(materialNameArray, 6, materialname);
                        else
                        if (ingredients.Count == 0)
                            texture4 = domain + ":" + workingBlockTextureFolder + "/" + materialNameArray[5].Replace("{materialname}", materialname);
                        else
                            texture4 = string.Empty;
                }
            }

            domainOut = domain;
            materialNameOut = materialname;
            ingredientsOut = ingredients;

            return new[] {texture1, texture2, texture3, texture4 };
        }

        private async Task RenderChairJSON(bool langs)
        {
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;
                var conditions = string.Empty;
                var ingredientDomain = string.Empty;

                foreach (var file in Directory.EnumerateFiles(_blockModelTemplateFolder))
                {
                    var materialname = item;
                    
                    if (!file.Contains("\\chair_wood_") || file.Contains("_inventory") || file.Contains("_conditional"))
                        continue;

                    var filePathArray = file.Split('\\');
                    var fileNameArray = filePathArray[filePathArray.Length - 1].Split('.');

                    var chairName = fileNameArray[0];

                    string domain;

                    List<KeyValuePair<string, int>> ingredients;
                    var textures = GetTextureOverrides(materialname, out materialname, out domain, out ingredients);

                    if (ingredients.Any())
                    {
                        ingredientDomain = ingredients.First().Key.Split(':')[0];

                        if (ingredientDomain != "minecraft")
                            conditions = @", ""conditions"":[{ ""type"":""forge:mod_loaded"", ""modid"":""" + ingredientDomain + @"""}]";
                    }

                    if (domain == "minecraft")
                        chairName += "_"  + materialname;
                    else
                    {
                        chairName += "_" + domain + "_" + materialname;
                        ingredientDomain = domain;
                    }
                    //blockModelFilename = $"\\{chairName}_inventory.json";
                    //var inventoryTemplateName = $"{fileNameArray[0]}_inventory";

                    var blockModelFilename = $"\\{chairName}.json";
                    var blockModelInventoryFilename = $"\\{chairName}_inventory.json";

                    var data = new TemplateData
                    {
                        Path = _blockModelTemplateFolder,
                        Name = fileNameArray[0],
                        BlockName = chairName,
                        MaterialName = materialname,
                        TopSuffix = topSuffix,
                        SideSuffix = sideSuffix,
                        Textures = textures,
                        Ingredients = ingredients,
                        Conditions = conditions,
                        IngredientDomain = ingredientDomain
                    };

                    var fileName = "\\" + chairName + ".json";

                    tasks.Add(WriteFile(_modelsBlockPath + blockModelFilename, @LoadTemplate(data)));
                    tasks.Add(WriteFile(_modelsBlockPath + blockModelInventoryFilename, @LoadTemplate(data.WithName(fileNameArray[0] + "_inventory"))));
                    tasks.Add(WriteFile(_blockstatesPath + blockModelFilename, @LoadTemplate(data.WithPath(_blockstateTemplateFolder))));
                    tasks.Add(WriteFile(_modelsItemPath + blockModelFilename, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));
                    tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));
                    if (_renderAdvancement)
                        tasks.Add(WriteFile(_recipeAdvancementsPath + blockModelFilename, @LoadTemplate(data.WithPath(_advancementTemplateFolder).WithName("recipe"))));

                    if (_renderRecipe)
                    {
                        if (chairName.Contains("padded") && chairName.Contains("single"))
                        {
                            for (var i = 0; i < 16; i++)
                            {
                                data.ColourMeta = i.ToString();
                                
                                var recipeName = $"\\{chairName}_{i.ToString()}.json";

                                if (domain == "minecraft")
                                    tasks.Add(WriteFile(_recipePath + recipeName, @LoadTemplate(data.WithPath(_recipeTemplateFolder))));
                                else
                                    tasks.Add(WriteFile(_recipePath + recipeName, @LoadTemplate(data.WithPath(_recipeTemplateFolder).WithName(fileNameArray[0] + "_conditional"))));
                            }
                        }
                        else
                        {
                            if (domain == "minecraft")
                                tasks.Add(WriteFile(_recipePath + blockModelFilename, @LoadTemplate(data.WithPath(_recipeTemplateFolder))));
                            else
                                tasks.Add(WriteFile(_recipePath + blockModelFilename, @LoadTemplate(data.WithPath(_recipeTemplateFolder).WithName(fileNameArray[0] + "_conditional"))));
                        }

                        
                    }

                    if (!langs)
                        continue;

                    var templateNameArray = fileNameArray[0].Split('_');
                    var langName = materialname.FirstCharToUpper();

                    for (var i = 0; i < templateNameArray.Length; i++)
                        if (i > 1)
                            langName +=  " " + templateNameArray[i].FirstCharToUpper();

                    langName += " " + "Chair";

                    data.LangName = langName;

                    AppendFile(_langPath + "\\en_EN.lang", @LoadTemplate(data.WithPath(_langTemplateFolder).WithName("lang")));
                }
            }

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task RenderLogJSON()
        {
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);

                var blockname = materialname + "_log";
                var barkname = materialname + "_bark";
                
                var fileName = "\\" + blockname + ".json";
                var fileNameBark = "\\" + barkname + ".json";

                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                var block = _profile.Blocks.Find(b => b.Name == "Log");

                //if (block.Side)
                //    sideSuffix = "_side";

                if (block.Top)
                    topSuffix = "_top";

                var data = new TemplateData
                {
                    Path = _blockModelTemplateFolder,
                    Name = "log",
                    BlockName = blockname,
                    MaterialName = materialname,
                    TopSuffix = topSuffix,
                    SideSuffix = sideSuffix,
                    Textures = textures,
                    Ingredients = ingredients
                };

                tasks.Add(WriteFile(_modelsBlockPath + fileName, @LoadTemplate(data)));
                tasks.Add(WriteFile(_modelsBlockPath + fileNameBark, @LoadTemplate(data.WithName("bark"))));
                tasks.Add(WriteFile(_blockstatesPath + fileName, @LoadTemplate(data.WithPath(_blockstateTemplateFolder))));
                tasks.Add(WriteFile(_modelsItemPath + fileName, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));
                tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        
        private async Task RenderBlockJSON(string template)
        {
            await RenderBlockJSON(false, template);
        }

        private async Task RenderBlockJSON(bool smooth)
        {
            await RenderBlockJSON(smooth, string.Empty);
        }

        private async Task RenderDoubleSlabJSON(bool smooth, bool brick)
        {
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);

                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

                var block = _profile.Blocks.Find(b => b.Name == "DoubleSlab");

                if (brick)
                {
                    if (block.Side)
                        sideSuffix = "_side";

                    if (block.Side)
                        topSuffix = "_top";
                }

                var blockname = materialname + "_double_slab";

                var fileName = $"\\{blockname}.json";

                var data = new TemplateData
                {
                    Path = _blockstateTemplateFolder,
                    Name = "double_slab",
                    BlockName = blockname,
                    MaterialName = materialname,
                    TopSuffix = topSuffix,
                    SideSuffix = sideSuffix,
                    Textures = textures,
                    Ingredients = ingredients
                };

                tasks.Add(WriteFile(_blockstatesPath + fileName, @LoadTemplate(data)));

                tasks.Add(WriteFile(_modelsBlockPath + fileName,
                    @LoadTemplate(data.WithPath(_blockModelTemplateFolder))));

                tasks.Add(WriteFile(_modelsItemPath + fileName,
                    @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));

                //tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task RenderBlockJSON(bool smooth, string template)
        {
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);

                var blockname = materialname;

                if (smooth)
                    blockname += "_smooth";

                if (template != string.Empty && template != "block")
                    blockname += "_" + template;
                else
                    template = "block";

                var fileName = "\\" + blockname + ".json";

                var data = new TemplateData
                {
                    Path = _blockModelTemplateFolder,
                    Name = template,
                    BlockName = blockname,
                    MaterialName = materialname,
                    Textures = textures,
                    Ingredients = ingredients
                };

                tasks.Add(WriteFile(_modelsBlockPath + fileName, @LoadTemplate(data)));
                tasks.Add(WriteFile(_blockstatesPath + fileName, @LoadTemplate(data.WithPath(_blockstateTemplateFolder))));
                tasks.Add(WriteFile(_modelsItemPath + fileName, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));
                tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));

                if ((smooth || template != "block") && _renderAdvancement)
                    tasks.Add(WriteFile(_recipeAdvancementsPath + fileName,
                        @LoadTemplate(data.WithPath(_advancementTemplateFolder).WithName("recipe"))));
                
            }

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task RenderBrickBlockJSON(bool smooth)
        {
            var tasks = new List<Task>();

            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;
                List<KeyValuePair<string, int>> ingredients;

                var textures = GetTextureOverrides(item, out materialname, out domain, out ingredients);

                var blockname = materialname;
                var smoothSuffix = string.Empty;

                if (smooth)
                {
                    blockname += "_smooth";
                    smoothSuffix = "_smooth";
                }

                blockname += "_brick";

                var fileName = "\\" + blockname + ".json";

                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                var block = _profile.Blocks.Find(b => b.Name == "Brick");

                if (block.Side)
                    sideSuffix = "_side";

                if (block.Top)
                    topSuffix = "_top";
                

                var data = new TemplateData
                {
                    Path = _blockModelTemplateFolder,
                    Name = "brick",
                    BlockName = blockname,
                    MaterialName = materialname,
                    TopSuffix = topSuffix,
                    SideSuffix = sideSuffix,
                    Textures = textures,
                    SmoothSuffix = smoothSuffix,
                    Ingredients = ingredients
                };

                tasks.Add(WriteFile(_modelsBlockPath + fileName, @LoadTemplate(data)));
                tasks.Add(WriteFile(_blockstatesPath + fileName, @LoadTemplate(data.WithPath(_blockstateTemplateFolder))));
                tasks.Add(WriteFile(_modelsItemPath + fileName, @LoadTemplate(data.WithPath(_itemModelTemplateFolder))));
                tasks.Add(WriteFile(_lootTablePath + fileName, @LoadTemplate(data.WithPath(_lootTableTemplateFolder))));
                tasks.Add(WriteFile(_recipeAdvancementsPath + fileName, @LoadTemplate(data.WithPath(_advancementTemplateFolder).WithName("recipe"))));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task WriteFile(string path, string content)
        {
            if (content == string.Empty) return;
            
            if (File.Exists(path))
                File.Delete(path);

            var parsedJson = JToken.Parse(content);

            var beautified = parsedJson.ToString(Formatting.Indented);

           // File.WriteAllLines(path, new[] { beautified });

            using (var writer = File.CreateText(path))
            {
                await writer.WriteAsync(beautified);
            }

            _filesGenerated++;
            FileProcessedEvent?.Invoke(_filesGenerated);
        }

        private static void AppendFile(string path, string content)
        {
            using (var w = File.AppendText(path))
            {
                w.WriteLine(content);
            }
        }

        public async Task<int> RenderJSON(bool blocks, bool stairs, bool walls, bool slabs, bool smooth, bool brick, 
            bool furnace, bool releifs, bool langs, bool chairs, bool leaves, bool log, bool planks, bool woodStairs, 
            bool renderDoor, bool renderDoubleSlab, bool renderAdvancement, bool renderWoodSlabs, bool renderGate, 
            bool renderFence, bool renderRecipe)
        {
            _filesGenerated = 0;
            _renderAdvancement = renderAdvancement;
            _renderRecipe = renderRecipe;

            if (!Directory.Exists("out"))
                Directory.CreateDirectory("out");

            if (!Directory.Exists("out\\" + modid))
                Directory.CreateDirectory("out\\" + modid);

            if (!Directory.Exists("out\\" + modid + "\\" + _profile.Version))
                Directory.CreateDirectory("out\\" + modid + "\\" + _profile.Version);
            
            if (!Directory.Exists(_basePath + "\\assets"))
                Directory.CreateDirectory(_basePath + "\\assets");
            
            if (!Directory.Exists(_basePath + "\\data"))
                Directory.CreateDirectory(_basePath + "\\data");

            if (!Directory.Exists(_basePath + "\\data\\minecraft"))
                Directory.CreateDirectory(_basePath + "\\data\\minecraft");

            if (!Directory.Exists(_basePath + "\\data\\minecraft\\tags"))
                Directory.CreateDirectory(_basePath + "\\data\\minecraft\\tags");

            if (!Directory.Exists(_basePath + "\\data\\minecraft\\tags\\blocks"))
                Directory.CreateDirectory(_basePath + "\\data\\minecraft\\tags\\blocks");

            if (!Directory.Exists(_basePath + "\\data\\minecraft\\tags\\items"))
                Directory.CreateDirectory(_basePath + "\\data\\minecraft\\tags\\items");

            if (!Directory.Exists(_basePath + "\\data\\" + modid))
                Directory.CreateDirectory(_basePath + "\\data\\" + modid);

            if (!Directory.Exists(_basePath + "\\data\\" + modid + "\\loot_tables"))
                Directory.CreateDirectory(_basePath + "\\data\\" + modid + "\\loot_tables");

            if (!Directory.Exists(_basePath + "\\data\\" + modid + "\\loot_tables\\blocks"))
                Directory.CreateDirectory(_basePath + "\\data\\" + modid + "\\loot_tables\\blocks");

            if (!Directory.Exists(_basePath + "\\assets\\" + modid))
                Directory.CreateDirectory(_basePath + "\\assets\\" + modid);

            if (!Directory.Exists(_basePath + "\\assets\\" + modid + "\\advancements"))
                Directory.CreateDirectory(_basePath + "\\assets\\" + modid + "\\advancements");

            double version;

            double.TryParse(this.version, out version);

            if (version >= 1.14)
            {
                if (!Directory.Exists(_basePath + "\\data\\" + modid + "\\recipes"))
                    Directory.CreateDirectory(_basePath + "\\data\\" + modid + "\\recipes");
            }
            else
            {
                if (!Directory.Exists(_basePath + "\\assets\\" + modid + "\\recipes"))
                    Directory.CreateDirectory(_basePath + "\\assets\\" + modid + "\\recipes");
            }

            if (!Directory.Exists(_basePath + "\\assets\\" + modid + "\\advancements\\" + modid))
                Directory.CreateDirectory(_basePath + "\\assets\\" + modid + "\\advancements\\" + modid);

            if (!Directory.Exists(_basePath + "\\assets\\" + modid + "\\advancements\\recipes"))
                Directory.CreateDirectory(_basePath + "\\assets\\" + modid + "\\advancements\\recipes");

            if (!Directory.Exists(_basePath + "\\assets\\" + modid + "\\lang"))
                Directory.CreateDirectory(_basePath + "\\assets\\" + modid + "\\lang");

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
                await RenderBlockJSON(false);

                if (smooth)
                    await RenderBlockJSON(true);

                if (brick)
                    await RenderBrickBlockJSON(false);

                if (brick && smooth)
                    await RenderBrickBlockJSON(true);
            }

            if (stairs)
            {
                await RenderStairJSON(false, false);

                if (smooth)
                    await RenderStairJSON(true, false);

                if (brick)
                    await RenderStairJSON(false, true);

                if (brick && smooth)
                    await RenderStairJSON(true, true);
            }

            if (walls)
            {
                await RenderWallJSON(false, false);

                if (smooth)
                    await RenderWallJSON(true, false);

                if (brick)
                    await RenderWallJSON(false, true);

                if (brick && smooth)
                    await RenderWallJSON(true, true);
            }

            if (furnace)
            {
                await RenderFurnaceJSON(false, false);

                if (smooth)
                    await RenderFurnaceJSON(true, false);

                if (brick)
                    await RenderFurnaceJSON(false, true);

                if (brick && smooth)
                    await RenderFurnaceJSON(true, true);
            }

            if (slabs)
            {
                await RenderSlabJSON(false, false, false);

                if (smooth)
                    await RenderSlabJSON(true, false, false);

                if (brick)
                    await RenderSlabJSON(false, true, false);

                if (brick && smooth)
                    await RenderSlabJSON(true, true, false);
            }

            if (releifs) await RenderReleifJSON(langs);

            if (chairs) await RenderChairJSON(langs);

            if (leaves) await RenderBlockJSON("leaves");

            if (log) await RenderLogJSON();

            if (planks) await RenderBlockJSON("planks");

            if (woodStairs) await RenderStairJSON(false, false, true);

            if (renderWoodSlabs) await RenderSlabJSON(false, false, true);

            if (renderGate) await RenderGateJSON();

            if (renderFence) await RenderFenceJSON();

            if (renderDoor) await RenderDoorJSON();

            if (renderDoubleSlab)
            {
                await RenderDoubleSlabJSON(false, false);

                if (smooth)
                    await RenderDoubleSlabJSON(true, false);

                if (brick)
                    await RenderDoubleSlabJSON(false, true);

                if (brick && smooth)
                    await RenderDoubleSlabJSON(true, true);
            }
            //renderGate
            return _filesGenerated;
        }
    }
}
