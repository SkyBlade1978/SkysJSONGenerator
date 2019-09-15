using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SkysJSONGenerator
{
    public class JSonGenerator
    {
        private Profile _profile;

        private string _baseTemplateFolder;
        private string _blockModelTemplateFolder;
        private string _itemModelTemplateFolder;
        private string _blockstateTemplateFolder;
        private string _lootTableTemplateFolder;
        private string _tagTemplateFolder;
        private string _wallItemTagPath;
        private string _wallTagPath;
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
            _tagTemplateFolder = _baseTemplateFolder + "\\tags";
        }

        private string LoadTemplate(string path, string name, string blockname, string materialname, string topsuffix, string sidesuffix, string walllist)
        {
            var overridePath = path + "\\" + modid + "_"+ name + ".template";
            var fullPath = path + "\\" + name + ".template";

            if (File.Exists(overridePath))
                fullPath = overridePath;

            if (File.Exists(fullPath))
            {
                var template = File.ReadAllText(fullPath).Replace("{modid}", "{0}").Replace("{blockname}", "{1}").Replace("{materialname}", "{2}").Replace("{topsuffix}", "{3}").Replace("{sidesuffix}", "{4}").Replace("{blocktexturefolder}", "{5}").Replace("{walllist}", "{6}");

                return string.Format(@template, modid, blockname, materialname, topsuffix, sidesuffix, blocktexturefolder, walllist);
            }
            else
            {
                Debug.Print("Template not found: " + fullPath);
                return string.Empty;
            }
        }

        private void RenderStairJSON(bool smooth, bool brick)
        {
            foreach (var item in _profile.Materials)
            {
                var materialname = item;
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

                Block block = _profile.Blocks.Find(b => b.Name == "Brick");

                if (brick)
                {
                    if (block.Side)
                        sideSuffix = "_side";

                    if (block.Side)
                        topSuffix = "_top";
                }

                var blockname = materialname + "_stairs";

                var fileName = $"\\{blockname}.json";

                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path:_blockstateTemplateFolder, name: "stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));         
                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile($"{_modelsBlockPath}\\{blockname}_inner.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "inner_stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile($"{_modelsBlockPath}\\{blockname}_outer.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "outer_stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "stairs", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
            }
        }

        private void RenderSlabJSON(bool smooth, bool brick)
        {
            foreach (var item in _profile.Materials)
            {
                var materialname = item;
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

                Block block = _profile.Blocks.Find(b => b.Name == "Slabs");

                if (brick)
                {
                    if (block.Side)
                        sideSuffix = "_side";

                    if (block.Side)
                        topSuffix = "_top";
                }

                var blockname = materialname + "_slab";

                var fileName = $"\\{blockname}.json";

                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile($"{_modelsBlockPath}\\upper_{blockname}.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "upper_slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile($"{_modelsBlockPath}\\half_{blockname}.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "half_slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "slab", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
            }
        }

        private void RenderReleifJSON()
        {
            foreach (var item in _profile.Materials)
            {
                var materialname = item;
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                Block block = _profile.Blocks.Find(b => b.Name == "Relief");

                var blockname = materialname + "_relief";

                var fileName = $"\\{blockname}.json";
                string blockModelFilename = string.Empty;
                
                foreach (var file in Directory.EnumerateFiles(_blockModelTemplateFolder))
                {
                    if (file.Contains("relief"))
                    {
                        var filenameArray = file.Split('_');
                        var reliefName = filenameArray[1].Split('.');
                        blockModelFilename = $"\\{blockname}_{reliefName[0]}.json";
                        WriteFile(_modelsBlockPath + blockModelFilename, @LoadTemplate(path: _blockModelTemplateFolder, name: $"relief_{reliefName[0]}", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));

                        WriteFile(_blockstatesPath + blockModelFilename, @LoadTemplate(path: _blockstateTemplateFolder, name: "relief", blockname: blockname + $"_{ reliefName[0]}", materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                        WriteFile(_modelsItemPath + blockModelFilename, @LoadTemplate(path: _itemModelTemplateFolder, name: "relief", blockname: blockname + $"_{ reliefName[0]}", materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                    }
                }

                
                //WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "relief", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
            }
        }

        private void RenderFurnaceJSON(bool smooth, bool brick)
        {
            foreach (var item in _profile.Materials)
            {
                var materialname = item;
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

                Block block = _profile.Blocks.Find(b => b.Name == "Furnace");
                
                if (block.Side)
                    sideSuffix = "_side";

                if (block.Side)
                    topSuffix = "_top";
                
                var blockname = materialname + "_furnace";

                var fileName = $"\\{blockname}.json";
                var fileNameLit = $"\\lit_{blockname}.json";

                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_blockstatesPath + fileNameLit, @LoadTemplate(path: _blockstateTemplateFolder, name: "lit_furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_modelsBlockPath + fileNameLit, @LoadTemplate(path: _blockModelTemplateFolder, name: "lit_furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "furnace", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
            }
        }

        private void RenderWallJSON(bool smooth, bool brick)
        {
            foreach (var item in _profile.Materials)
            {
                var materialname = item;
                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;
                
                if (smooth)
                    materialname += "_smooth";

                if (brick)
                    materialname += "_brick";

                Block block = _profile.Blocks.Find(b => b.Name == "Walls");

                if (block.Side && brick)
                    sideSuffix = "_side";

                if (block.Side && brick)
                    topSuffix = "_top";

                var blockname = materialname + "_wall";
                var fileName = $"\\{blockname}.json";

                if (_wallTags != "")
                    _wallTags += ", ";

                _wallTags += "\"" + modid + ":" + blockname + "\"";

                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "wall", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));

                WriteFile($"{_modelsBlockPath}\\{blockname}_inventory.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "wall_inventory", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile($"{_modelsBlockPath}\\{blockname}_post.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "wall_post", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile($"{_modelsBlockPath}\\{blockname}_side.json", @LoadTemplate(path: _blockModelTemplateFolder, name: "wall_side", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));

                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "wall", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "wall", blockname: blockname, materialname: materialname, topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
            }

            WriteFile(_wallTagPath + "\\walls.json", @LoadTemplate(path: _tagTemplateFolder, name: "wall", blockname: "", materialname: "", topsuffix: "", sidesuffix: "", walllist: _wallTags));
            WriteFile(_wallItemTagPath + "\\walls.json", @LoadTemplate(path: _tagTemplateFolder, name: "wall", blockname: "", materialname: "", topsuffix: "", sidesuffix: "", walllist: _wallTags));

        }

        private void RenderBlockJSON(bool smooth)
        {
            foreach (var item in _profile.Materials)
            {
                var blockname = item;

                if (smooth)
                    blockname += "_smooth";
                
                var fileName = "\\" + blockname + ".json";

                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "block", blockname: blockname, materialname: blockname, topsuffix: "", sidesuffix: "", walllist: ""));
                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "block", blockname: blockname, materialname: blockname, topsuffix: "", sidesuffix: "", walllist: ""));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "block", blockname: blockname, materialname: blockname, topsuffix: "", sidesuffix: "", walllist: ""));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "block", blockname: blockname, materialname: blockname, topsuffix: "", sidesuffix: "", walllist: ""));
            }
        }

        private void RenderBrickBlockJSON(bool smooth)
        {
            foreach (var item in _profile.Materials)
            {
                var blockname = item;

                if (smooth)
                    blockname += "_smooth";

                blockname += "_brick";

                var fileName = "\\" + blockname + ".json";

                var topSuffix = string.Empty;
                var sideSuffix = string.Empty;

                Block block = _profile.Blocks.Find(b => b.Name == "Brick");

                if (block.Side)
                    sideSuffix = "_side";

                if (block.Top)
                    topSuffix = "_top";

                WriteFile(_modelsBlockPath + fileName, @LoadTemplate(path: _blockModelTemplateFolder, name: "brick", blockname: blockname, materialname: "", topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_blockstatesPath + fileName, @LoadTemplate(path: _blockstateTemplateFolder, name: "brick", blockname: blockname, materialname: "", topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_modelsItemPath + fileName, @LoadTemplate(path: _itemModelTemplateFolder, name: "brick", blockname: blockname, materialname: "", topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
                WriteFile(_lootTablePath + fileName, @LoadTemplate(path: _lootTableTemplateFolder, name: "brick", blockname: blockname, materialname: "", topsuffix: topSuffix, sidesuffix: sideSuffix, walllist: ""));
            }
        }

        private void WriteFile(string path, string content)
        {
            if (content != string.Empty)
            { 
                if (File.Exists(path))
                    File.Delete(path);

                JToken parsedJson = JToken.Parse(content);

                var beautified = parsedJson.ToString(Formatting.Indented);

                File.WriteAllLines(path, new string[] { beautified });

                _filesGenerated++;
            }
        }

        public int RenderJSON(bool blocks, bool stairs, bool walls, bool slabs, bool smooth, bool brick, bool furnace, bool releifs)
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

            if (releifs)
            {
                RenderReleifJSON();
            }

            return _filesGenerated;
        }

    }
}
