using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SkysJSONGenerator
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }

    public class JSonGenerator
    {
        private readonly Profile _profile;

        private readonly string _baseTemplateFolder;
        private readonly string _blockModelTemplateFolder;
        private readonly string _itemModelTemplateFolder;
        private readonly string _blockstateTemplateFolder;
        private readonly string _lootTableTemplateFolder;
        private readonly string _langTemplateFolder;
        private readonly string _tagTemplateFolder;
        private readonly string _wallItemTagPath;
        private readonly string _wallTagPath;
        private readonly string _basePath;
        private readonly string _blockstatesPath;
        private readonly string _modelsPath;
        private readonly string _modelsBlockPath;
        private readonly string _modelsItemPath;
        private readonly string _lootTablePath;
        private readonly string _langPath;
        private int _filesGenerated;

        // interpolated fields
        private readonly string modid;
        private readonly string blocktexturefolder;
        private readonly string version;

        string _wallTags = "";

        public JSonGenerator(Profile profile, string basePath)
        {
            modid = profile.Modid;
            blocktexturefolder = profile.TextureBlocksFolder;
            version = profile.Version;

            _profile = profile;

            _basePath = basePath;
            _lootTablePath = $"{_basePath}\\data\\{modid}\\loot_tables\\blocks";
            _blockstatesPath = $"{_basePath}\\assets\\{modid}\\blockstates";
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
        }

        private string LoadTemplate(string path, string name, string blockname, string materialname, string topsuffix, string sidesuffix, string walllist, string langname, string[] textures)
        {
            var overridePath = path + "\\" + modid + "_" + name + ".template";
            var fullPath = path + "\\" + name + ".template";

            var texture1 = string.Empty;
            var texture2 = string.Empty;
            var texture3 = string.Empty;

            if (File.Exists(overridePath))
                fullPath = overridePath;

            if (File.Exists(fullPath))
            {
                var template = File.ReadAllText(fullPath).Replace("{modid}", "{0}").Replace("{blockname}", "{1}").Replace("{materialname}", "{2}").Replace("{topsuffix}", "{3}").Replace("{sidesuffix}", "{4}").Replace("{blocktexturefolder}", "{5}").Replace("{walllist}", "{6}").Replace("{langname}", "{7}").Replace("{texture1}", "{8}").Replace("{texture2}", "{9}").Replace("{texture3}", "{10}");

                if (textures.Length > 0)
                    texture1 = textures[0];

                if (textures.Length > 1)
                    texture2 = textures[1];

                if (textures.Length > 2)
                    texture3 = textures[2];

                try
                {
                    return string.Format(@template, modid, blockname, materialname, topsuffix, sidesuffix, blocktexturefolder, walllist, langname, texture1, texture2, texture3);
                }
                catch (Exception)
                {
                    MessageBox.Show("Whoops, there appears to be an error in template file:" + fullPath);
                    throw;
                }
            }
            else
            {
                Debug.Print("Template not found: " + fullPath);
                return string.Empty;
            }
        }

        private string LoadTemplate(string path, string name, string blockname, string materialname, string topsuffix, string sidesuffix, string walllist, string langname)
        {
            return LoadTemplate(path, name, blockname, materialname, topsuffix, sidesuffix, walllist, langname, new string[] { });
        }

        private string LoadTemplate(string path, string name, string blockname, string materialname, string topsuffix, string sidesuffix, string walllist)
        {
            return LoadTemplate(path, name, blockname, materialname, topsuffix, sidesuffix, walllist, "");
        }

        private void RenderStairJSON(bool smooth, bool brick)
        {
            RenderStairJSON(smooth, brick, false);
        }

        private void RenderStairJSON(bool smooth, bool brick, bool wood)
        {
            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;

                var textures = getTextureOverrides(item, out materialname, out domain);
                
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

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

                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path:_blockstateTemplateFolder, name: "stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));         
                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile($"{_modelsBlockPath}\\{blockname}_inner.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "inner_stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile($"{_modelsBlockPath}\\{blockname}_outer.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "outer_stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
            }
        }

        private void RenderDoorJSON()
        {
            foreach (var item in _profile.Materials)
            {

                string materialname;
                string domain;

                var textures = getTextureOverrides(item, out materialname, out domain);
                
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                var blockname = materialname + "_door";

                var fileName = $"\\{blockname}.json";

                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "door", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "door", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));

                WriteFile($"{_modelsBlockPath}\\{blockname}_bottom.json",
                    @LoadTemplate(path: _blockModelTemplateFolder, name: "door_bottom", blockname: blockname,
                        materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));

                WriteFile($"{_modelsBlockPath}\\{blockname}_bottom_rh.json",
                    @LoadTemplate(path: _blockModelTemplateFolder, name: "door_bottom_rh", blockname: blockname,
                        materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));

                WriteFile($"{_modelsBlockPath}\\{blockname}_top.json",
                    @LoadTemplate(path: _blockModelTemplateFolder, name: "door_top", blockname: blockname,
                        materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));

                WriteFile($"{_modelsBlockPath}\\{blockname}_top_rh.json",
                    @LoadTemplate(path: _blockModelTemplateFolder, name: "door_top_rh", blockname: blockname,
                        materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "door", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "door", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
            }
        }

        private void RenderSlabJSON(bool smooth, bool brick)
        {
            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;

                var textures = getTextureOverrides(item, out materialname, out domain);
                
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

                var blockname = materialname + "_slab";

                var fileName = $"\\{blockname}.json";

                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile($"{_modelsBlockPath}\\upper_{blockname}.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "upper_slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile($"{_modelsBlockPath}\\half_{blockname}.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "half_slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
            }
        }

        private void RenderReleifJSON(bool langs)
        {
            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;

                var textures = getTextureOverrides(item, out materialname, out domain);
                
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

                    WriteFile(_modelsBlockPath + blockModelFilename, @LoadTemplate(path: _blockModelTemplateFolder, name: templateName, blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));

                    if (filenameArray.Length >= 3)
                        continue;

                    WriteFile(_blockstatesPath + blockModelFilename, @LoadTemplate(path: _blockstateTemplateFolder, name: "relief", blockname: blockname + $"_{ reliefName[0]}", materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                    WriteFile(_modelsItemPath + blockModelFilename, @LoadTemplate(path: _itemModelTemplateFolder, name: "relief", blockname: blockname + $"_{ reliefName[0]}", materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));

                    if (langs)
                        AppendFile(_langPath + "\\en_EN.lang", @LoadTemplate(path: _langTemplateFolder, name: "lang", blockname: blockname + $"_{ reliefName[0]}", materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "",langname: materialname.FirstCharToUpper() + " " + reliefName[0].FirstCharToUpper() + " Relief", textures: textures));
                }
                
                //WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "relief", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
            }
        }

        private void RenderFurnaceJSON(bool smooth, bool brick)
        {
            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;

                var textures = getTextureOverrides(item, out materialname, out domain);

                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

                var block = _profile.Blocks.Find(b => b.Name == "Furnace");
                
                if (block.Side)
                    sideSuffix = "_side";

                if (block.Side)
                    topSuffix = "_top";
                
                var blockname = materialname + "_furnace";

                var fileName = $"\\{blockname}.json";
                var fileNameLit = $"\\lit_{blockname}.json";

                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_blockstatesPath + fileNameLit, @LoadTemplate(path: _blockstateTemplateFolder, name: "lit_furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsBlockPath + fileNameLit, @LoadTemplate(path: _blockModelTemplateFolder, name: "lit_furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsItemPath + fileNameLit, @LoadTemplate(path: _itemModelTemplateFolder, name: "furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
            }
        }

        private void RenderWallJSON(bool smooth, bool brick)
        {
            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;

                var textures = getTextureOverrides(item, out materialname, out domain);

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

                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "wall", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));

                WriteFile($"{_modelsBlockPath}\\{blockname}_inventory.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "wall_inventory", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile($"{_modelsBlockPath}\\{blockname}_post.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "wall_post", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile($"{_modelsBlockPath}\\{blockname}_side.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "wall_side", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));

                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "wall", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "wall", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
            }

            WriteFile(_wallTagPath + "\\walls.json", @LoadTemplate(path: _tagTemplateFolder, name: "wall", blockname: "", materialname: "", topsuffix: "", sidesuffix: "", walllist: _wallTags));
            WriteFile(_wallItemTagPath + "\\walls.json", @LoadTemplate(path: _tagTemplateFolder, name: "wall", blockname: "", materialname: "", topsuffix: "", sidesuffix: "", walllist: _wallTags));

        }

        private string getNaturaBlockFolder(string input, string materialname)
        { 
            var overUnder = "overworld";

            if (materialname.Contains("bloodwood") || materialname.Contains("darkwood") ||
                materialname.Contains("fusewood") || materialname.Contains("ghostwood"))
                overUnder = "nether";

            if (input.Contains("bark"))
                return $"blocks/logs/{overUnder}";

            if (input.Contains("log"))
               return $"blocks/logs/{overUnder}";

            if (input.Contains("plank"))
                return $"blocks/planks/{overUnder}";

            return  "blocks";
        }

        private string[] getTextureOverrides(string materialname, out string materialNameOut, out string domainOut)
        {
            var domain = modid;

            var texture1 = domain + ":" + blocktexturefolder + "\\" + materialname;
            var texture2 = domain + ":" + blocktexturefolder + "\\" + materialname;
            var texture3 = domain + ":" + blocktexturefolder + "\\" + materialname;

            if (materialname.Contains(":"))
            {
                var materialNameArray = materialname.Split(':');

                domain = materialNameArray[0];
                materialname = materialNameArray[1];

                // natura has it's own "special" way of structuring it's assets :(
                if (domain == "natura")
                {
                    Debug.Print("natura");

                    if (materialNameArray.Length > 2)
                        texture1 = domain + ":" + getNaturaBlockFolder(materialNameArray[2], materialname) + "/" +
                                   materialNameArray[2].Replace("{materialname}", materialname);
                    else
                        texture1 = string.Empty;

                    if (materialNameArray.Length > 3)
                        texture2 = domain + ":" + getNaturaBlockFolder(materialNameArray[3], materialname) + "/" +
                                   materialNameArray[3].Replace("{materialname}", materialname);
                    else
                        texture2 = string.Empty;

                    if (materialNameArray.Length > 4)
                        texture3 = domain + ":" + getNaturaBlockFolder(materialNameArray[4], materialname) + "/" +
                                   materialNameArray[4].Replace("{materialname}", materialname);
                    else
                        texture3 = string.Empty;
                }
                else
                {
                    if (materialNameArray.Length > 2)
                        texture1 = domain + ":" + blocktexturefolder + "/" + materialNameArray[2].Replace("{materialname}", materialname);
                    else
                        texture1 = string.Empty;

                    if (materialNameArray.Length > 3)
                        texture2 = domain + ":" + blocktexturefolder + "/" + materialNameArray[3].Replace("{materialname}", materialname);
                    else
                        texture2 = string.Empty;

                    if (materialNameArray.Length > 4)
                        texture3 = domain + ":" + blocktexturefolder + "/" + materialNameArray[4].Replace("{materialname}", materialname);
                    else
                        texture3 = string.Empty;

                }
            }

            domainOut = domain;
            materialNameOut = materialname;

            return new[] {texture1, texture2, texture3};
        }

        private void RenderChairJSON(bool langs)
        {
            foreach (var item in _profile.Materials)
            {
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                foreach (var file in Directory.EnumerateFiles(_blockModelTemplateFolder))
                {
                    var materialname = item;

                    if (!file.Contains("\\chair_wood_"))
                        continue;

                    var filePathArray = file.Split('\\');
                    var fileNameArray = filePathArray[filePathArray.Length - 1].Split('.');

                    var chairName = fileNameArray[0];

                    string domain;

                    var textures = getTextureOverrides(materialname, out materialname, out domain);

                    if (domain == "minecraft")
                        chairName += "_"  + materialname;
                    else
                        chairName += "_" + domain + "_" + materialname;

                    //blockModelFilename = $"\\{chairName}_inventory.json";
                    //var inventoryTemplateName = $"{fileNameArray[0]}_inventory";

                    var blockModelFilename = $"\\{chairName}.json";

                    WriteFile(_modelsBlockPath + blockModelFilename, @LoadTemplate(path: _blockModelTemplateFolder, name: fileNameArray[0], blockname: chairName, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "",  textures: textures));
                        
                    WriteFile(_blockstatesPath + blockModelFilename, @LoadTemplate(path: _blockstateTemplateFolder, name: fileNameArray[0], blockname: chairName, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                    WriteFile(_modelsItemPath + blockModelFilename, @LoadTemplate(path: _itemModelTemplateFolder, name: fileNameArray[0], blockname: chairName, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));

                    if (!langs)
                        continue;

                    var templateNameArray = fileNameArray[0].Split('_');
                    var langName = materialname.FirstCharToUpper();

                    for (var i = 0; i < templateNameArray.Length; i++)
                        if (i > 1)
                            langName +=  " " + templateNameArray[i].FirstCharToUpper();

                    langName += " " + "Chair";

                    AppendFile(_langPath + "\\en_EN.lang", @LoadTemplate(path: _langTemplateFolder, name: "lang", blockname: chairName, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: langName));
                }
            }
        }

        private void RenderLogJSON()
        {
            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;

                var textures = getTextureOverrides(item, out materialname, out domain);

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

                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "log", blockname: blockname, materialname: item, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsBlockPath + fileNameBark, @LoadTemplate(path: _blockModelTemplateFolder, name: "bark", blockname: barkname, materialname: item, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "log", blockname: blockname, materialname: item, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "log", blockname: blockname, materialname: item, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "log", blockname: blockname, materialname: item, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
            }
        }

        
        private void RenderBlockJSON(string template)
        {
            RenderBlockJSON(false, template);
        }

        private void RenderBlockJSON(bool smooth)
        {
            RenderBlockJSON(smooth, string.Empty);
        }

        private void RenderBlockJSON(bool smooth, string template)
        {
            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;

                var textures = getTextureOverrides(item, out materialname, out domain);

                var blockname = materialname;

                if (smooth)
                    blockname += "_smooth";

                if (template != string.Empty && template != "block")
                    blockname += "_" + template;
                else
                    template = "block";

                var fileName = "\\" + blockname + ".json";

                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: template, blockname: blockname, materialname: materialname, topsuffix: "", sidesuffix: "", walllist: "", langname:"", textures: textures));
                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: template, blockname: blockname, materialname: materialname, topsuffix: "", sidesuffix: "", walllist: "", langname: "", textures: textures));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: template, blockname: blockname, materialname: materialname, topsuffix: "", sidesuffix: "", walllist: "", langname: "", textures: textures));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: template, blockname: blockname, materialname: materialname, topsuffix: "", sidesuffix: "", walllist: "", langname: "", textures: textures));
            }
        }

        private void RenderBrickBlockJSON(bool smooth)
        {
            foreach (var item in _profile.Materials)
            {
                string materialname;
                string domain;

                var textures = getTextureOverrides(item, out materialname, out domain);

                var blockname = materialname;

                if (smooth)
                    blockname += "_smooth";

                blockname += "_brick";

                var fileName = "\\" + blockname + ".json";

                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                var block = _profile.Blocks.Find(b => b.Name == "Brick");

                if (block.Side)
                    sideSuffix = "_side";

                if (block.Top)
                    topSuffix = "_top";

                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "brick", blockname: blockname, materialname: "", topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "brick", blockname: blockname, materialname: "", topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "brick", blockname: blockname, materialname: "", topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname: "", textures: textures));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "brick", blockname: blockname, materialname: "", topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: "", langname:"", textures: textures));
            }
        }

        private void WriteFile(string path, string content)
        {
            if (content == string.Empty) return;

            if (File.Exists(path))
                File.Delete(path);

            var parsedJson = JToken.Parse(content);

            var beautified = parsedJson.ToString(Formatting.Indented);

            File.WriteAllLines(path, new[] { beautified });

            _filesGenerated++;
        }

        private static void AppendFile(string path, string content)
        {
            using (var w = File.AppendText(path))
            {
                w.WriteLine(content);
            }
        }

        public int RenderJSON(bool blocks, bool stairs, bool walls, bool slabs, bool smooth, bool brick, bool furnace, bool releifs, bool langs, bool chairs, bool leaves, bool log, bool planks, bool woodStairs, bool renderDoor, bool renderDoubleSlab)
        {
            _filesGenerated = 0;

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

            if (walls)
            {
                RenderWallJSON(false, false);

                if (smooth)
                    RenderWallJSON(true, false);

                if (brick)
                    RenderWallJSON(false, true);

                if (brick && smooth)
                    RenderWallJSON(true, true);
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

            if (furnace)
            {
                RenderFurnaceJSON(false, false);

                if (smooth)
                    RenderFurnaceJSON(true, false);

                if (brick)
                    RenderFurnaceJSON(false, true);

                if (brick && smooth)
                    RenderFurnaceJSON(true, true);
            }

            if (slabs)
            {
                RenderSlabJSON(false, false);

                if (smooth)
                    RenderSlabJSON(true, false);

                if (brick)
                    RenderSlabJSON(false, true);

                if (brick && smooth)
                    RenderSlabJSON(true, true);
            }

            if (releifs) RenderReleifJSON(langs);

            if (chairs) RenderChairJSON(langs);

            if (leaves) RenderBlockJSON("leaves");

            if (log) RenderLogJSON();

            if (planks) RenderBlockJSON("planks");

            if (woodStairs) RenderStairJSON(false, false, true);

            if (renderDoor) RenderDoorJSON();

            if (renderDoubleSlab)
                RenderBlockJSON("double_slab");

            return _filesGenerated;
        }
    }
}
