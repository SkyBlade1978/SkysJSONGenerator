using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace SkysJSONGenerator
{
    public partial class MainForm : Form
    {
        private List<Profile> _profiles;
        private List<string> _versions;

        bool renderBlocks;
        bool renderStairs;
        bool renderWalls;
        bool renderSlabs;
        bool renderWoodSlabs;
        bool renderSmooth;
        bool renderBrick;
        bool renderFurnace;
        bool renderReliefs;
        bool renderLangs;
        bool renderChairs;
        bool renderLeaves;
        bool renderLog;
        bool renderPlanks;
        bool renderWoodStairs;
        bool renderDoor;
        bool renderDoubleSlab;
        bool renderAdvancement;
        bool renderGate;
        bool renderFence;
        bool renderRecipe;

        Profile selectedProfile;
        string basePath;
        private int generated;

        public MainForm()
        {
            InitializeComponent();

            LoadProfiles("All");
        }

        private void LoadConfig()
        {
            _profiles = new List<Profile>();
            _versions = new List<string>();

            var serializer = new JsonSerializer();

            if (Directory.Exists(@"profiles"))
            {
                foreach (var enumerateFile in Directory.EnumerateFiles("profiles", "*.cfg"))
                {
                    //var fileNameArray = enumerateFile.Split('\\');
                    //var fileName = fileNameArray[fileNameArray.Length - 1];

                    using (var file = File.OpenText(enumerateFile))
                    {
                        var profile =(Profile)serializer.Deserialize(file, typeof(Profile));

                        if (profile.ProfileVersion < 3)
                        {
                            profile.ProfileVersion = 3;

                            var fileCount = 2;

                            while (File.Exists(profile.Name + fileCount + ".old"))
                            {
                                fileCount++;

                                if (fileCount <= 100)
                                    continue;

                                MessageBox.Show(@"Please delete some old cfg files");
                                throw new Exception("Too many old config files");
                            }

                            File.Copy(enumerateFile, "profiles\\" + profile.Name + fileCount + ".old");
                            File.Delete(enumerateFile);

                            WriteFile($@"{profile.Name}.cfg", JsonConvert.SerializeObject(profile));
                        }

                        _profiles.Add(profile);
                    }

                }
            }
            else
            {
                var mineralogyProfile = new Profile("1.14", "mineralogy", new List<string>
                    {  "andesite"
                     , "basalt"
                     , "diorite"
                     , "granite"
                     , "rhyolite"
                     , "pegmatite"
                     , "shale"
                     , "conglomerate"
                     , "dolomite"
                     , "limestone"
                     , "marble"
                     , "slate"
                     , "schist"
                     , "gneiss"
                     , "phyllite"
                     , "amphibolite"
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "Blocks", Side = false, Top = false },
                        new Block { Name = "Stairs", Side = false, Top = false },
                        new Block { Name = "Walls", Side = true, Top = true },
                        new Block { Name = "Slabs", Side = true, Top = true },
                        new Block { Name = "Smooth", Side = false, Top = false },
                        new Block { Name = "Brick", Side = true, Top = true },
                    }, "Mineralogy 1.14 - Regular Stone");
                
                var mineralogyProfile110 = new Profile("1.10", "mineralogy", new List<string>
                    {  "mineralogy:andesite:{materialname}_smooth:basalt_smooth"
                     , "mineralogy:basalt:{materialname}_smooth:marble_smooth"
                     , "mineralogy:diorite:{materialname}_smooth:basalt_smooth"
                     , "mineralogy:granite:{materialname}_smooth:marble_smooth"
                     , "mineralogy:rhyolite:{materialname}_smooth:marble_smooth"
                     , "mineralogy:pegmatite:{materialname}_smooth:marble_smooth"
                     , "mineralogy:shale:{materialname}_smooth:marble_smooth"
                     , "mineralogy:conglomerate:{materialname}_smooth:marble_smooth"
                     , "mineralogy:dolomite:{materialname}_smooth:basalt_smooth"
                     , "mineralogy:limestone:{materialname}_smooth:marble_smooth"
                     , "mineralogy:marble:{materialname}_smooth:basalt_smooth"
                     , "mineralogy:slate:{materialname}_smooth:marble_smooth"
                     , "mineralogy:schist:{materialname}_smooth:marble_smooth"
                     , "mineralogy:gneiss:{materialname}_smooth:marble_smooth"
                     , "mineralogy:phyllite:{materialname}_smooth:marble_smooth"
                     , "mineralogy:amphibolite:{materialname}_smooth:marble_smooth"
                     , "mineralogy:diabase:{materialname}_smooth:marble_smooth" // new from here
                     , "mineralogy:gabbro:{materialname}_smooth:marble_smooth"
                     , "mineralogy:peridotite:{materialname}_smooth:marble_smooth"
                     , "mineralogy:basaltic_glass:{materialname}_smooth:marble_smooth"
                     , "mineralogy:scoria:{materialname}_smooth:marble_smooth"
                     , "mineralogy:tuff:{materialname}_smooth:marble_smooth"
                     , "mineralogy:siltstone:{materialname}_smooth:marble_smooth"
                     , "mineralogy:rock_salt:{materialname}_smooth:basalt_smooth"
                     , "mineralogy:hornfels:{materialname}_smooth:marble_smooth"
                     , "mineralogy:quartzite:{materialname}_smooth:marble_smooth"
                     , "mineralogy:novaculite:{materialname}_smooth:marble_smooth"
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "Blocks", Side = false, Top = false },
                        new Block { Name = "Stairs", Side = false, Top = false },
                        new Block { Name = "Walls", Side = true, Top = true},
                        new Block { Name = "Slabs", Side = true, Top = true },
                        new Block { Name = "DoubleSlab", Side = true, Top = true },
                        new Block { Name = "Smooth", Side = false, Top = false },
                        new Block { Name = "Brick", Side = true, Top = true },
                        new Block { Name = "Furnace", Side = true, Top = true },
                        new Block { Name = "Relief", Side = true, Top = true },
                        new Block { Name = "Lang", Side = true, Top = true },
                        new Block { Name = "Advancement", Side = false, Top = false }
                    }, "Mineralogy 1.10 - Regular Stone");

                var mineralogyProfileSedimentary = new Profile("1.10", "mineralogy", new List<string>
                    {  "chalk"         
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "Blocks", Side = false, Top = false }
                    }, "Mineralogy 1.10 - Irregular Blocks");

                var iafProfile110 = new Profile("1.10", "ironagefurniture", new List<string>
                    {  "minecraft:oak:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                        , "minecraft:spruce:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                        , "minecraft:birch:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                        , "minecraft:jungle:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                        , "minecraft:acacia:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                        , "minecraft:big_oak:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                     , "biomesoplenty:umbran:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:willow:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:magic:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:mahogany:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:mangrove:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:palm:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:pine:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:redwood:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:sacred_oak:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:ebony:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:ethereal:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:eucalyptus:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:fir:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:hellbark:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:jacaranda:{materialname}_planks:{materialname}_log"
                     , "biomesoplenty:cherry:{materialname}_planks:{materialname}_log"
                     , "natura:amaranth:{materialname}_planks:{materialname}_log"
                    , "natura:eucalyptus:{materialname}_planks:{materialname}_log"
                    , "natura:hopseed:{materialname}_planks:{materialname}_log"
                    , "natura:maple:{materialname}_planks:{materialname}_log"
                    , "natura:redwood:{materialname}_planks:{materialname}_bark"
                    , "natura:sakura:{materialname}_planks:{materialname}_log"
                    , "natura:silverbell:{materialname}_planks:{materialname}_log"
                    , "natura:tiger:{materialname}_planks:{materialname}_log"
                    , "natura:willow:{materialname}_planks:{materialname}_log"
                    , "natura:bloodwood:{materialname}_planks:{materialname}_bark"
                    , "natura:darkwood:{materialname}_planks:{materialname}_log"
                    , "natura:fusewood:{materialname}_planks:{materialname}_log"
                    , "natura:ghostwood:{materialname}_planks:{materialname}_log"
                    , "immersiveengineering:treatedWood:{materialname}_vertical:{materialname}_packaged"
                    , "forestry:acacia:planks.{materialname}:bark.{materialname}"
                    , "forestry:balsa:planks.{materialname}:bark.{materialname}"
                    , "forestry:baobab:planks.{materialname}:bark.{materialname}"
                    , "forestry:cherry:planks.{materialname}:bark.{materialname}"
                    , "forestry:chestnut:planks.{materialname}:bark.{materialname}"
                    , "forestry:citrus:planks.{materialname}:bark.{materialname}"
                    , "forestry:cocobolo:planks.{materialname}:bark.{materialname}"
                    , "forestry:ebony:planks.{materialname}:bark.{materialname}"
                    , "forestry:giganteum:planks.{materialname}:bark.{materialname}"
                    , "forestry:greenheart:planks.{materialname}:bark.{materialname}"
                    , "forestry:ipe:planks.{materialname}:bark.{materialname}"
                    , "forestry:kapok:planks.{materialname}:bark.{materialname}"
                    , "forestry:larch:planks.{materialname}:bark.{materialname}"
                    , "forestry:lime:planks.{materialname}:bark.{materialname}"
                    , "forestry:mahoe:planks.{materialname}:bark.{materialname}"
                    , "forestry:mahogany:planks.{materialname}:bark.{materialname}"
                    , "forestry:maple:planks.{materialname}:bark.{materialname}"
                    , "forestry:padauk:planks.{materialname}:bark.{materialname}"
                    , "forestry:palm:planks.{materialname}:bark.{materialname}"
                    , "forestry:papaya:planks.{materialname}:bark.{materialname}"
                    , "forestry:pine:planks.{materialname}:bark.{materialname}"
                    , "forestry:plum:planks.{materialname}:bark.{materialname}"
                    , "forestry:poplar:planks.{materialname}:bark.{materialname}"
                    , "forestry:sequoia:planks.{materialname}:bark.{materialname}"
                    , "forestry:teak:planks.{materialname}:bark.{materialname}"
                    , "forestry:walnut:planks.{materialname}:bark.{materialname}"
                    , "forestry:wenge:planks.{materialname}:bark.{materialname}"
                    , "forestry:willow:planks.{materialname}:bark.{materialname}"
                    , "forestry:zebrawood:planks.{materialname}:bark.{materialname}"

                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "WoodChairs", Side = false, Top = false },
                        new Block { Name = "Lang", Side = true, Top = true }
                    }, "Iron Age Furniture 1.10 ");


                var iafProfile112 = new Profile("1.12", "ironagefurniture", new List<string>
                    {  "minecraft:oak:planks_{materialname}:log_{materialname}:ingredients:minecraft:planks:0"
                        , "minecraft:spruce:planks_{materialname}:log_{materialname}:ingredients:minecraft:planks:1"
                        , "minecraft:birch:planks_{materialname}:log_{materialname}:ingredients:minecraft:planks:2"
                        , "minecraft:jungle:planks_{materialname}:log_{materialname}:ingredients:minecraft:planks:3"
                        , "minecraft:acacia:planks_{materialname}:log_{materialname}:ingredients:minecraft:planks:4"
                        , "minecraft:big_oak:planks_{materialname}:log_{materialname}:ingredients:minecraft:planks:5"
                        , "biomesoplenty:sacred_oak:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:0"
                        , "biomesoplenty:cherry:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:1"
                        , "biomesoplenty:umbran:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:2"
                        , "biomesoplenty:fir:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:3"
                        , "biomesoplenty:ethereal:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:4"
                        , "biomesoplenty:magic:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:5"
                        , "biomesoplenty:mangrove:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:6"
                        , "biomesoplenty:palm:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:7"
                        , "biomesoplenty:redwood:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:8"
                        , "biomesoplenty:willow:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:9"
                        , "biomesoplenty:pine:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:10"
                        , "biomesoplenty:hellbark:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:11"
                        , "biomesoplenty:jacaranda:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:12"
                        , "biomesoplenty:mahogany:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:13"
                        , "biomesoplenty:ebony:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:14"
                        , "biomesoplenty:eucalyptus:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:planks_0:15"
                        , "natura:maple:{materialname}_planks:{materialname}_log:ingredients:natura:overworld_planks:0"
                        , "natura:silverbell:{materialname}_planks:{materialname}_log:ingredients:natura:overworld_planks:1"
                        , "natura:amaranth:{materialname}_planks:{materialname}_log:ingredients:natura:overworld_planks:2"
                        , "natura:tiger:{materialname}_planks:{materialname}_log:ingredients:natura:overworld_planks:3"
                        , "natura:willow:{materialname}_planks:{materialname}_log:ingredients:natura:overworld_planks:4"
                        , "natura:eucalyptus:{materialname}_planks:{materialname}_log:ingredients:natura:overworld_planks:5"
                        , "natura:hopseed:{materialname}_planks:{materialname}_log:ingredients:natura:overworld_planks:6"
                        , "natura:sakura:{materialname}_planks:{materialname}_log:ingredients:natura:overworld_planks:7"
                        , "natura:redwood:{materialname}_planks:{materialname}_bark:ingredients:natura:overworld_planks:8"
                        , "natura:ghostwood:{materialname}_planks:{materialname}_log:ingredients:natura:nether_planks:0"
                        , "natura:bloodwood:{materialname}_planks:{materialname}_bark:ingredients:natura:nether_planks:1"
                        , "natura:fusewood:{materialname}_planks:{materialname}_log:ingredients:natura:nether_planks:3"
                        , "natura:darkwood:{materialname}_planks:{materialname}_log:ingredients:natura:nether_planks:2"
                        , "immersiveengineering:treatedWood:{materialname}_vertical:{materialname}_packaged:ingredients:immersiveengineering:treatedWood_packaged:0"
                        , "forestry:larch:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:0"
                        , "forestry:teak:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:1"
                        , "forestry:acacia:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:2"
                        , "forestry:lime:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:3"
                        , "forestry:chestnut:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:4"
                        , "forestry:wenge:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:5"
                        , "forestry:baobab:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:6"
                        , "forestry:sequoia:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:7"
                        , "forestry:kapok:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:8"
                        , "forestry:ebony:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:9"
                        , "forestry:mahogany:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:10"
                        , "forestry:balsa:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:11"
                        , "forestry:willow:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:12"
                        , "forestry:walnut:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:13"
                        , "forestry:greenheart:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:14"
                        , "forestry:cherry:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.0:15"
                        , "forestry:mahoe:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:0"
                        , "forestry:poplar:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:1"
                        , "forestry:palm:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:2"
                        , "forestry:papaya:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:3"
                        , "forestry:pine:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:4"
                        , "forestry:plum:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:5"
                        , "forestry:maple:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:6"
                        , "forestry:citrus:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:7"
                        , "forestry:giganteum:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:8"
                        , "forestry:ipe:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:9"
                        , "forestry:padauk:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:10"
                        , "forestry:cocobolo:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:11"
                        , "forestry:zebrawood:planks.{materialname}:bark.{materialname}:ingredients:forestry:planks.1:12"
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "WoodChairs", Side = false, Top = false },
                        new Block { Name = "Lang", Side = true, Top = true },
                        new Block { Name = "Advancement", Side = false, Top = false },
                        new Block { Name = "Recipe", Side = false, Top = false }
                    }, "Iron Age Furniture 1.12 ");

                var iafProfile114 = new Profile("1.14", "ironagefurniture", new List<string>
                    {  "minecraft:oak:{materialname}_planks:{materialname}_log:ingredients:minecraft:{materialname}_planks"
                        , "minecraft:spruce:{materialname}_planks:{materialname}_log:ingredients:minecraft:{materialname}_planks"
                        , "minecraft:birch:{materialname}_planks:{materialname}_log:ingredients:minecraft:{materialname}_planks"
                        , "minecraft:jungle:{materialname}_planks:{materialname}_log:ingredients:minecraft:{materialname}_planks"
                        , "minecraft:acacia:{materialname}_planks:{materialname}_log:ingredients:minecraft:{materialname}_planks"
                        , "minecraft:dark_oak:{materialname}_planks:{materialname}_log:ingredients:minecraft:{materialname}_planks"
                        , "biomesoplenty:cherry:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:ethereal:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:fir:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:hellbark:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:jacaranda:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:magic:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:mahogany:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:palm:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:redwood:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:umbran:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:willow:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "biomesoplenty:dead:{materialname}_planks:{materialname}_log:ingredients:biomesoplenty:{materialname}_planks"
                        , "immersiveengineering:treated_wood:wooden_decoration/{materialname}_vertical:wooden_decoration/{materialname}_packaged:ingredients:immersiveengineering:{materialname}_packaged"

                    },
                   "block", "items", 3, new List<Block>
                   {
                        new Block { Name = "WoodChairs", Side = false, Top = false },
                        new Block { Name = "Lang", Side = true, Top = true },
                        new Block { Name = "Advancement", Side = false, Top = false },
                        new Block { Name = "Recipe", Side = false, Top = false }
                   }, "Iron Age Furniture 1.14 ");

                var spookyBiomes112 = new Profile("1.12", "spookybiomes", new List<string>
                    {  "bloodwood"
                     , "ghostly"
                     , "seeping"
                     , "sorbus"
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "Leaves", Side = false, Top = false },
                        new Block { Name = "Log", Side = false, Top = true},
                        new Block { Name = "WoodStairs", Side = false, Top = false },
                        new Block { Name = "Planks", Side = false, Top = false },
                        new Block { Name = "Door", Side = false, Top = false },
                        new Block { Name = "DoubleSlab", Side = false, Top = false },
                        new Block { Name = "WoodSlab", Side = false, Top = false },
                        new Block { Name = "Gate", Side = false, Top = false },
                        new Block { Name = "Fence", Side = false, Top = false }

                    }, "Spooky Biomes 1.12 - Wood");

                _profiles.Add(mineralogyProfile);
                _profiles.Add(mineralogyProfile110);
                _profiles.Add(mineralogyProfileSedimentary);
                _profiles.Add(iafProfile110);
                _profiles.Add(iafProfile112);
                _profiles.Add(spookyBiomes112);
                _profiles.Add((iafProfile114));

                if (!Directory.Exists("profiles"))
                    Directory.CreateDirectory("profiles");

                foreach (var profile in _profiles)
                    WriteFile($@"profiles\\{profile.Name}.cfg", JsonConvert.SerializeObject(profile));
            }

            foreach (var item in _profiles)
            {
                if (!_versions.Contains(item.Version))
                    _versions.Add(item.Version);
            }

            checkedListBoxOutput.Items.Clear();
        }

        private static void WriteFile(string path, string content)
        {
            if (File.Exists(path))
                File.Delete(path);

            var parsedJson = JToken.Parse(content);

            var beautified = parsedJson.ToString(Formatting.Indented);

            File.WriteAllLines(path, new[] { beautified });
        }

        private void LoadProfiles(string version)
        {
            LoadConfig();

            comboBoxMod.Items.Clear();

            foreach (var item in _profiles)
                if (version == "All" || version == item.Version)
                    comboBoxMod.Items.Add(item);
        }

        private void CounterIncrement(int counter)
        {
            labelCounter.Text = counter.ToString();
            labelCounter.Refresh();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                return;

            if (comboBoxMod.SelectedIndex >= 0)
            {

                renderBlocks = false;
                renderStairs = false;
                renderWalls = false;
                renderSlabs = false;
                renderSmooth = false;
                renderBrick = false;
                renderFurnace = false;
                renderReliefs = false;
                renderLangs = false;
                renderChairs = false;
                renderLeaves = false;
                renderLog = false;
                renderPlanks = false;
                renderWoodStairs = false;
                renderWoodSlabs = false;
                renderDoor = false;
                renderDoubleSlab = false;
                renderAdvancement = false;
                renderGate = false;
                renderFence = false;
                renderRecipe = false;

                selectedProfile = (Profile)comboBoxMod.SelectedItem;
                basePath = "out\\" + selectedProfile.Modid + "\\" + selectedProfile.Version;
                
                if (!Directory.Exists("templates\\" + selectedProfile.Version))
                {
                    MessageBox.Show(@"Whoops, there doesn't appear to be a templates folder for version " + selectedProfile.Version);
                    return;
                }

                buttonGenerate.Enabled = false;

                for (var i = 0; i < checkedListBoxOutput.Items.Count; i++)
                {
                    if (!checkedListBoxOutput.GetItemChecked(i))
                        continue;

                    var blockItem = (Block)checkedListBoxOutput.Items[i];

                    switch (blockItem.Name)
                    {
                        case "Blocks":
                            renderBlocks = true;
                            break;
                        case "Stairs":
                            renderStairs = true;
                            break;
                        case "Walls":
                            renderWalls = true;
                            break;
                        case "Slabs":
                            renderSlabs = true;
                            break;
                        case "Smooth":
                            renderSmooth = true;
                            break;
                        case "Brick":
                            renderBrick = true;
                            break;
                        case "Furnace":
                            renderFurnace = true;
                            break;
                        case "Relief":
                            renderReliefs = true;
                            break;
                        case "Lang":
                            renderLangs = true;
                            break;
                        case "WoodChairs":
                            renderChairs = true;
                            break;
                        case "Leaves":
                            renderLeaves = true;
                            break;
                        case "Log":
                            renderLog = true;
                            break;
                        case "Planks":
                            renderPlanks = true;
                            break;
                        case "WoodStairs":
                            renderWoodStairs = true;
                            break;
                        case "Door":
                            renderDoor = true;
                            break;
                        case "DoubleSlab":
                            renderDoubleSlab = true;
                            break;
                        case "Advancement":
                            renderAdvancement = true;
                            break;
                        case "WoodSlab":
                            renderWoodSlabs = true;
                            break;
                        case "Gate":
                            renderGate = true;
                            break;
                        case "Fence":
                            renderFence = true;
                            break;
                        case "Recipe":
                            renderRecipe = true;
                            break;
                    }
                }

                backgroundWorker1.RunWorkerAsync();
            }
            else
                MessageBox.Show(@"Please select a mod profile above", @"Which mod?", MessageBoxButtons.OK);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new AboutBox();

            about.ShowDialog(this);
        }

        private void comboBoxMod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMod.SelectedIndex < 0)
                return;

            var profile = (Profile)comboBoxMod.SelectedItem;
            listBoxMaterials.DataSource = profile.Materials;

            checkedListBoxOutput.Items.Clear();

            foreach (var item in profile.Blocks)
            {
                checkedListBoxOutput.Items.Add(item);
                checkedListBoxOutput.SetItemChecked(checkedListBoxOutput.Items.Count - 1, true);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var generator = new JSonGenerator(selectedProfile, basePath);

            generator.FileProcessedEvent += counter => worker?.ReportProgress(counter);
            
            generated = generator.RenderJSON(renderBlocks, renderStairs, renderWalls, renderSlabs, renderSmooth, renderBrick, renderFurnace, renderReliefs, renderLangs, renderChairs, renderLeaves, renderLog, renderPlanks, renderWoodStairs, renderDoor, renderDoubleSlab, renderAdvancement, renderWoodSlabs, renderGate, renderFence, renderRecipe).GetAwaiter().GetResult();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (generated == 0)
                MessageBox.Show(@"No files generated", @"Result", MessageBoxButtons.OK);
            else
            {
                MessageBox.Show(generated + @" files generated", @"Result", MessageBoxButtons.OK);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = basePath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }

            buttonGenerate.Enabled = true;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelCounter.Text = e.ProgressPercentage.ToString();
        }
    }
}
