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
        bool renderSconce;
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
        bool renderCode;

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
                    {  "stone:mineralogy:andesite:{materialname}_smooth:basalt_smooth"
                     , "stone:mineralogy:basalt:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:diorite:{materialname}_smooth:basalt_smooth"
                     , "stone:mineralogy:granite:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:rhyolite:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:pegmatite:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:shale:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:conglomerate:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:dolomite:{materialname}_smooth:basalt_smooth"
                     , "stone:mineralogy:limestone:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:marble:{materialname}_smooth:basalt_smooth"
                     , "stone:mineralogy:slate:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:schist:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:gneiss:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:phyllite:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:amphibolite:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:diabase:{materialname}_smooth:marble_smooth" // new from here
                     , "stone:mineralogy:gabbro:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:peridotite:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:basaltic_glass:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:scoria:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:tuff:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:siltstone:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:rock_salt:{materialname}_smooth:basalt_smooth"
                     , "stone:mineralogy:hornfels:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:quartzite:{materialname}_smooth:marble_smooth"
                     , "stone:mineralogy:novaculite:{materialname}_smooth:marble_smooth"
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
                    {  "wood:minecraft:oak:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                        , "wood:minecraft:spruce:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                        , "wood:minecraft:birch:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                        , "wood:minecraft:jungle:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                        , "wood:minecraft:acacia:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                        , "wood:minecraft:big_oak:planks_{materialname}:log_{materialname}:log_{materialname}_top"
                     , "wood:biomesoplenty:umbran:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:willow:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:magic:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:mahogany:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:mangrove:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:palm:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:pine:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:redwood:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:sacred_oak:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:ebony:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:ethereal:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:eucalyptus:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:fir:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:hellbark:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:jacaranda:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:biomesoplenty:cherry:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                     , "wood:natura:amaranth:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:natura:eucalyptus:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:natura:hopseed:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:natura:maple:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:natura:redwood:{materialname}_planks:{materialname}_bark:{materialname}_log_top"
                    , "wood:natura:sakura:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:natura:silverbell:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:natura:tiger:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:natura:willow:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:natura:bloodwood:{materialname}_planks:{materialname}_bark:{materialname}_heart_small"
                    , "wood:natura:darkwood:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:natura:fusewood:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:natura:ghostwood:{materialname}_planks:{materialname}_log:{materialname}_log_top"
                    , "wood:immersiveengineering:treatedWood:{materialname}_vertical:{materialname}_packaged:{materialname}_log_top"
                    , "wood:forestry:acacia:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:balsa:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:baobab:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:cherry:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:chestnut:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:citrus:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:cocobolo:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:ebony:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:giganteum:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:greenheart:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:ipe:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:kapok:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:larch:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:lime:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:mahoe:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:mahogany:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:maple:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:padauk:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:palm:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:papaya:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:pine:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:plum:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:poplar:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:sequoia:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:teak:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:walnut:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:wenge:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:willow:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "wood:forestry:zebrawood:planks.{materialname}:bark.{materialname}:heart.{materialname}"
                    , "metal:minecraft:iron:{materialname}_block:{materialname}_block:{materialname}_block"
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "WoodChairs", Side = false, Top = false },
                        new Block { Name = "Sconce", Side = false, Top = false },
                        new Block { Name = "Lang", Side = true, Top = true }
                    }, "Iron Age Furniture 1.10 ");


                var iafProfile112 = new Profile("1.12", "ironagefurniture", new List<string>
                    {  "wood:minecraft:oak:planks_{materialname}:log_{materialname}:log_{materialname}_top:ingredients:minecraft:planks:0:minecraft:log:0"
                        , "wood:minecraft:spruce:planks_{materialname}:log_{materialname}:log_{materialname}_top:ingredients:minecraft:planks:1:minecraft:log:1"
                        , "wood:minecraft:birch:planks_{materialname}:log_{materialname}:log_{materialname}_top:ingredients:minecraft:planks:2:minecraft:log:2"
                        , "wood:minecraft:jungle:planks_{materialname}:log_{materialname}:log_{materialname}_top:ingredients:minecraft:planks:3:minecraft:log:3"
                        , "wood:minecraft:acacia:planks_{materialname}:log_{materialname}:log_{materialname}_top:ingredients:minecraft:planks:4:minecraft:log2:0"
                        , "wood:minecraft:big_oak:planks_{materialname}:log_{materialname}:log_{materialname}_top:ingredients:minecraft:planks:5:minecraft:log2:1"
                        , "wood:biomesoplenty:sacred_oak:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:0:biomesoplenty:log_0:4"
                        , "wood:biomesoplenty:cherry:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:1:biomesoplenty:log_0:5"
                        , "wood:biomesoplenty:umbran:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:2:biomesoplenty:log_0:6"
                        , "wood:biomesoplenty:fir:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:3:biomesoplenty:log_0:7"
                        , "wood:biomesoplenty:ethereal:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:4:biomesoplenty:log_1:4"
                        , "wood:biomesoplenty:magic:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:5:biomesoplenty:log_1:5"
                        , "wood:wood:biomesoplenty:mangrove:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:6:biomesoplenty:log_1:6"
                        , "wood:biomesoplenty:palm:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:7:biomesoplenty:log_1:7"
                        , "wood:biomesoplenty:redwood:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:8:biomesoplenty:log_2:4"
                        , "wood:biomesoplenty:willow:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:9:biomesoplenty:log_2:5"
                        , "wood:biomesoplenty:pine:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:10:biomesoplenty:log_2:6"
                        , "wood:biomesoplenty:hellbark:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:11:biomesoplenty:log_2:7"
                        , "wood:biomesoplenty:jacaranda:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:12:biomesoplenty:log_3:4"
                        , "wood:biomesoplenty:mahogany:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:13:biomesoplenty:log_3:5"
                        , "wood:biomesoplenty:ebony:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:14:biomesoplenty:log_3:6"
                        , "wood:biomesoplenty:eucalyptus:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:planks_0:15:biomesoplenty:log_3:7"
                        , "wood:natura:maple:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:overworld_planks:0:natura:overworld_logs:0"
                        , "wood:natura:silverbell:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:overworld_planks:1:natura:overworld_logs:1"
                        , "wood:natura:amaranth:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:overworld_planks:2:natura:overworld_logs:2"
                        , "wood:natura:tiger:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:overworld_planks:3:natura:overworld_logs:3"
                        , "wood:natura:willow:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:overworld_planks:4:natura:overworld_logs2:0"
                        , "wood:natura:eucalyptus:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:overworld_planks:5:natura:overworld_logs2:1"
                        , "wood:natura:hopseed:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:overworld_planks:6:natura:overworld_logs2:2"
                        , "wood:natura:sakura:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:overworld_planks:7:natura:overworld_logs2:3"
                        , "wood:natura:ghostwood:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:nether_planks:0:natura:nether_logs:0"
                        , "wood:natura:bloodwood:{materialname}_planks:{materialname}_bark:{materialname}_heart_small:ingredients:natura:nether_planks:1:natura:nether_logs2:0"
                        , "wood:natura:fusewood:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:nether_planks:3:natura:nether_logs:2"
                        , "wood:natura:darkwood:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:natura:nether_planks:2:natura:nether_logs:1"
                        , "wood:immersiveengineering:treated_wood:{materialname}_vertical:{materialname}_packaged:{materialname}_log_top:ingredients:immersiveengineering:treated_wood:0:minecraft:log:9"
                        , "wood:forestry:larch:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:0:forestry:logs.0:0"
                        , "wood:forestry:teak:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:1:forestry:logs.0:1"
                        , "wood:forestry:acacia:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:2:forestry:logs.0:2"
                        , "wood:forestry:lime:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:3:forestry:logs.0:3"
                        , "wood:forestry:chestnut:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:4:forestry:logs.1:0"
                        , "wood:forestry:wenge:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:5:forestry:logs.1:1"
                        , "wood:forestry:baobab:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:6:forestry:logs.1:2"
                        , "wood:forestry:sequoia:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:7:forestry:logs.1:3"
                        , "wood:forestry:kapok:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:8:forestry:logs.2:0"
                        , "wood:forestry:ebony:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:9:forestry:logs.2:1"
                        , "wood:forestry:mahogany:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:10:forestry:logs.2:2"
                        , "wood:forestry:balsa:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:11:forestry:logs.2:3"
                        , "wood:forestry:willow:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:12:forestry:logs.3:0"
                        , "wood:forestry:walnut:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:13:forestry:logs.3:1"
                        , "wood:forestry:greenheart:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:14:forestry:logs.3:2"
                        , "wood:forestry:cherry:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.0:15:forestry:logs.3:3"
                        , "wood:forestry:mahoe:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:0:forestry:logs.4:0"
                        , "wood:forestry:poplar:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:1:forestry:logs.4:1"
                        , "wood:forestry:palm:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:2:forestry:logs.4:2"
                        , "wood:forestry:papaya:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:3:forestry:logs.4:3"
                        , "wood:forestry:pine:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:4:forestry:logs.5:0"
                        , "wood:forestry:plum:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:5:forestry:logs.5:1"
                        , "wood:forestry:maple:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:6:forestry:logs.5:2"
                        , "wood:forestry:citrus:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:7:forestry:logs.5:3"
                        , "wood:forestry:giganteum:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:8:forestry:logs.6:0"
                        , "wood:forestry:ipe:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:9:forestry:logs.6:1"
                        , "wood:forestry:padauk:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:10:forestry:logs.6:2"
                        , "wood:forestry:cocobolo:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:11:forestry:logs.6:3"
                        , "wood:forestry:zebrawood:planks.{materialname}:bark.{materialname}:heart.{materialname}:ingredients:forestry:planks.1:12:forestry:logs.7:0"
                    },
              "blocks", "items", 3, new List<Block>
              {
                        new Block { Name = "WoodChairs", Side = false, Top = false },
                        new Block { Name = "Lang", Side = true, Top = true },
                        new Block { Name = "Advancement", Side = false, Top = false },
                        new Block { Name = "Recipe", Side = false, Top = false }
              }, "Iron Age Furniture 1.12 ");
                
                var iafProfile114 = new Profile("1.14", "ironagefurniture", new List<string>
                    {     "wood:minecraft:oak:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:spruce:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:birch:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:jungle:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:acacia:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:dark_oak:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:biomesoplenty:cherry:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:ethereal:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:fir:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:hellbark:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:jacaranda:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:magic:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:mahogany:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:palm:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:redwood:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:umbran:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:willow:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:dead:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:immersiveengineering:treated_wood:wooden_decoration/{materialname}_vertical:wooden_decoration/{materialname}_packaged:log_top:ingredients:immersiveengineering:{materialname}_packaged:immersiveengineering:logs"
                    },
                   "block", "items", 3, new List<Block>
                   {
                        new Block { Name = "WoodChairs", Side = false, Top = false },
                        new Block { Name = "Lang", Side = true, Top = true },
                        new Block { Name = "Advancement", Side = false, Top = false },
                        new Block { Name = "Recipe", Side = false, Top = false }
                   }, "Iron Age Furniture 1.14 ");

                var iafProfile115 = new Profile("1.15", "ironagefurniture", new List<string>
                    {  "wood:minecraft:oak:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:spruce:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:birch:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:jungle:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:acacia:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:dark_oak:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:biomesoplenty:cherry:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:fir:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:hellbark:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:jacaranda:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:magic:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:mahogany:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:palm:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:redwood:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:umbran:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:willow:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:dead:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:immersiveengineering:treated_wood:wooden_decoration/{materialname}_vertical:wooden_decoration/{materialname}_packaged:log_top:ingredients:immersiveengineering:{materialname}_packaged:immersiveengineering:logs"
                         , "wood:byg:aspen:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:baobab:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:blue_enchanted:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:bulbis:{materialname}_planks:{materialname}_planks:{materialname}_stem_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:cherry:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:cika:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:cypress:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:ebony:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:embur:{materialname}_planks:{materialname}_planks:{materialname}_pedu_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:fir:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:glacial_oak:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:green_enchanted:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:holly:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:jacaranda:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:mahogany:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:mangrove:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:maple:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:pine:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:rainbow_eucalyptus:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:redwood:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:skyris:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:willow:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:witch_hazel:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:zelkova:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                    },
                   "block", "items", 3, new List<Block>
                   {
                        new Block { Name = "WoodChairs", Side = false, Top = false },
                        new Block { Name = "Lang", Side = true, Top = true },
                        new Block { Name = "Advancement", Side = false, Top = false },
                        new Block { Name = "Recipe", Side = false, Top = false },
                        new Block { Name = "Code", Side = false, Top = false }
                   }, "Iron Age Furniture 1.15 ");

                var iafProfile116 = new Profile("1.16", "ironagefurniture", new List<string>
                    {  "wood:minecraft:oak:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:spruce:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:birch:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:jungle:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:acacia:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:minecraft:dark_oak:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:minecraft:{materialname}_planks:minecraft:{materialname}_log"
                        , "wood:biomesoplenty:cherry:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:fir:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:hellbark:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:jacaranda:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:magic:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:mahogany:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:palm:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:redwood:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:umbran:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:willow:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:biomesoplenty:dead:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:biomesoplenty:{materialname}_planks:biomesoplenty:{materialname}_log"
                        , "wood:immersiveengineering:treated_wood:wooden_decoration/{materialname}_vertical:wooden_decoration/{materialname}_packaged:log_top:ingredients:immersiveengineering:{materialname}_packaged:immersiveengineering:logs"
                         , "wood:byg:aspen:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:baobab:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:blue_enchanted:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:bulbis:{materialname}_planks:{materialname}_planks:{materialname}_stem_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:cherry:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:cika:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:cypress:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:ebony:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:embur:{materialname}_planks:{materialname}_planks:{materialname}_pedu_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:fir:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:glacial_oak:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:green_enchanted:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:holly:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:jacaranda:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:mahogany:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:mangrove:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:maple:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:pine:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:rainbow_eucalyptus:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:redwood:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:skyris:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:willow:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:witch_hazel:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                         , "wood:byg:zelkova:{materialname}_planks:{materialname}_log:{materialname}_log_top:ingredients:byg:{materialname}_planks:byg:{materialname}_log"
                    },
   "block", "items", 3, new List<Block>
   {
                        new Block { Name = "WoodChairs", Side = false, Top = false },
                        new Block { Name = "Lang", Side = true, Top = true },
                        new Block { Name = "Advancement", Side = false, Top = false },
                        new Block { Name = "Recipe", Side = false, Top = false },
                        new Block { Name = "Code", Side = false, Top = false }
   }, "Iron Age Furniture 1.16 ");


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
                _profiles.Add((iafProfile115));
                _profiles.Add((iafProfile116));

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
                renderSconce = false;
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
                renderCode = false;

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
                        case "Sconce":
                            renderSconce = true;
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
                        case "Code":
                            renderCode = true;
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
            
            generated = generator.RenderJSON(renderBlocks, renderStairs, renderWalls, renderSlabs, renderSmooth, 
                renderBrick, renderFurnace, renderReliefs, renderLangs, renderChairs, renderLeaves, renderLog, 
                renderPlanks, renderWoodStairs, renderDoor, renderDoubleSlab, renderAdvancement, 
                renderWoodSlabs, renderGate, renderFence, renderRecipe, renderCode, renderSconce).GetAwaiter().GetResult();
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
