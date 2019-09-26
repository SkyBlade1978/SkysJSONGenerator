﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SkysJSONGenerator
{
    public partial class MainForm : Form
    {
        private List<Profile> _profiles;
        private List<string> _versions;

        public MainForm()
        {
            InitializeComponent();

            LoadProfiles("All");
        }

        private void LoadConfig()
        {
            _profiles = new List<Profile>();
            _versions = new List<string>();

            JsonSerializer serializer = new JsonSerializer();

            if (File.Exists(@"profiles.cfg"))
            {
                var dirty = false;

                using (StreamReader file = File.OpenText(@"profiles.cfg"))
                {
                    _profiles = (List<Profile>)serializer.Deserialize(file, typeof(List<Profile>));

                    foreach (var item in _profiles)
                        if(item.ProfileVersion < 3)
                        {
                            dirty = true;
                            item.ProfileVersion = 3;
                        }       
                }

                if (dirty)
                {
                    int fileCount = 2;

                    while (File.Exists(@"profiles" + fileCount + ".old"))
                    {
                        fileCount++;

                        if (fileCount > 100)
                        {
                            MessageBox.Show("Please delete some old cfg files");
                            throw new Exception("Too many old config files");
                        }
                    }
                    
                    File.Copy(@"profiles.cfg", @"profiles" + fileCount + ".old");
                    File.Delete(@"profiles.cfg");

                    WriteFile(@"profiles.cfg", JsonConvert.SerializeObject(_profiles));
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
                     , "diabase" // new from here
                     , "gabbro"
                     , "peridotite"
                     , "basaltic_glass"
                     , "scoria"
                     , "tuff"
                     , "siltstone"
                     , "rock_salt"
                     , "hornfels"
                     , "quartzite"
                     , "novaculite"
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "Blocks", Side = false, Top = false },
                        new Block { Name = "Stairs", Side = false, Top = false },
                        new Block { Name = "Walls", Side = true, Top = true},
                        new Block { Name = "Slabs", Side = true, Top = true },
                        new Block { Name = "Smooth", Side = false, Top = false },
                        new Block { Name = "Brick", Side = true, Top = true },
                        new Block { Name = "Furnace", Side = true, Top = true },
                        new Block { Name = "Relief", Side = true, Top = true },
                        new Block { Name = "Lang", Side = true, Top = true }
                    }, "Mineralogy 1.10 - Regular Stone");

                var mineralogyProfileSedimentary = new Profile("1.10", "mineralogy", new List<string>
                    {  "chalk"         
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "Blocks", Side = false, Top = false }
                    }, "Mineralogy 1.10 - Irregular Blocks");

                var iafProfile110 = new Profile("1.10", "ironagefurniture", new List<string>
                    {  "minecraft:oak:planks_{materialname}:log_{materialname}"
                     , "minecraft:acacia:planks_{materialname}:log_{materialname}"
                     , "minecraft:big_oak:planks_{materialname}:log_{materialname}"
                     , "minecraft:birch:planks_{materialname}:log_{materialname}"
                     , "minecraft:jungle:planks_{materialname}:log_{materialname}"
                     , "minecraft:spruce:planks_{materialname}:log_{materialname}"
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
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "WoodChairs", Side = false, Top = false },
                        new Block { Name = "Lang", Side = true, Top = true }
                    }, "Iron Age Furniture 1.10 ");

                var spookyBiomes112 = new Profile("1.12", "spookybiomes", new List<string>
                    {  "blood"
                     , "example1"
                     , "example2"
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "Leaves", Side = false, Top = false },
                        new Block { Name = "Log", Side = false, Top = true}
                    }, "Spooky Biomes 1.12 - Wood");

                _profiles.Add(mineralogyProfile);
                _profiles.Add(mineralogyProfile110);
                _profiles.Add(mineralogyProfileSedimentary);
                _profiles.Add(iafProfile110);
                _profiles.Add(spookyBiomes112);

                WriteFile(@"profiles.cfg", JsonConvert.SerializeObject(_profiles));
            }

            foreach (var item in _profiles)
            {
                if (!_versions.Contains(item.Version))
                    _versions.Add(item.Version);
            }

            checkedListBoxOutput.Items.Clear();
        }

        private void WriteFile(string path, string content)
        {
            if (File.Exists(path))
                File.Delete(path);

            JToken parsedJson = JToken.Parse(content);

            var beautified = parsedJson.ToString(Formatting.Indented);

            File.WriteAllLines(path, new string[] { beautified });
        }

        private void LoadProfiles(string version)
        {
            LoadConfig();

            comboBoxMod.Items.Clear();

            foreach (var item in _profiles)
            {
                if (version == "All" || version == item.Version)
                    comboBoxMod.Items.Add(item);
            }          
        }
   
        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (comboBoxMod.SelectedIndex >= 0)
            {
                var renderBlocks = false;
                var renderStairs = false;
                var renderWalls = false;
                var renderSlabs = false;
                var renderSmooth = false;
                var renderBrick = false;
                var renderFurnace = false;
                var renderReliefs = false;
                var renderLangs = false;
                var renderChairs = false;
                var renderLeaves = false;
                var renderLog = false;

                var selectedProfile = (Profile)comboBoxMod.SelectedItem;
                var basePath = "out\\" + selectedProfile.Modid + "\\" + selectedProfile.Version;
                var generator = new JSonGenerator(selectedProfile, basePath);
                
                if (!Directory.Exists("templates\\" + selectedProfile.Version))
                {
                    MessageBox.Show("Whoops, there doesn't appear to be a templates folder for version " + selectedProfile.Version);
                    return;
                }

                buttonGenerate.Enabled = false;

                for (int i = 0; i < checkedListBoxOutput.Items.Count; i++)
                {
                    if (checkedListBoxOutput.GetItemChecked(i))
                    {
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

                            default:
                                break;
                        }
                    }
                }

                var generated = generator.RenderJSON(renderBlocks, renderStairs, renderWalls, renderSlabs, renderSmooth, renderBrick, renderFurnace, renderReliefs, renderLangs, renderChairs, renderLeaves, renderLog);

                if (generated == 0)
                    MessageBox.Show("No files generated", "Result", MessageBoxButtons.OK);
                else
                {
                    MessageBox.Show(generated + " files generated", "Result", MessageBoxButtons.OK);

                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = basePath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
            }
            else
                MessageBox.Show("Please select a mod profile above", "Which mod?", MessageBoxButtons.OK);

            buttonGenerate.Enabled = true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new AboutBox();

            about.ShowDialog(this);
        }

        private void comboBoxMod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMod.SelectedIndex >= 0)
            {
                var profile = (Profile)comboBoxMod.SelectedItem;
                listBoxMaterials.DataSource = profile.Materials;

                checkedListBoxOutput.Items.Clear();

                foreach (var item in profile.Blocks)
                {
                    checkedListBoxOutput.Items.Add(item);
                    checkedListBoxOutput.SetItemChecked(checkedListBoxOutput.Items.Count - 1, true);
                }
            }
        }
    }
}
