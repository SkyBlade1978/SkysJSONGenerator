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
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "WoodChairs", Side = false, Top = false },
                        new Block { Name = "Lang", Side = true, Top = true }
                    }, "Iron Age Furniture 1.10 ");

                var spookyBiomes112 = new Profile("1.12", "spookybiomes", new List<string>
                    {  "bloodwood"
                     , "ghostly"
                     , "oozing"
                     , "witchwood"
                    },
                    "blocks", "items", 3, new List<Block>
                    {
                        new Block { Name = "Leaves", Side = false, Top = false },
                        new Block { Name = "Log", Side = false, Top = true},
                        new Block { Name = "WoodStairs", Side = false, Top = false },
                        new Block { Name = "Planks", Side = false, Top = false },
                        new Block { Name = "Door", Side = false, Top = false },
                        new Block { Name = "DoubleSlab", Side = false, Top = false }

                    }, "Spooky Biomes 1.12 - Wood");

                _profiles.Add(mineralogyProfile);
                _profiles.Add(mineralogyProfile110);
                _profiles.Add(mineralogyProfileSedimentary);
                _profiles.Add(iafProfile110);
                _profiles.Add(spookyBiomes112);

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
                renderDoor = false;
                renderDoubleSlab = false;
                renderAdvancement = false;

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
            
            generated = generator.RenderJSON(renderBlocks, renderStairs, renderWalls, renderSlabs, renderSmooth, renderBrick, renderFurnace, renderReliefs, renderLangs, renderChairs, renderLeaves, renderLog, renderPlanks, renderWoodStairs, renderDoor, renderDoubleSlab, renderAdvancement).GetAwaiter().GetResult();
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
