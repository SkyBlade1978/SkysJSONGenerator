using Newtonsoft.Json;
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

            comboBoxVersion.SelectedIndex = 0;
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
                        if(item.ProfileVersion < 2)
                        {
                            dirty = true;
                            item.ProfileVersion = 2;
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
                    "blocks", "items", 2, new List<Block>
                    {
                        new Block { Name = "Blocks", Side = false, Top = false },
                        new Block { Name = "Stairs", Side = false, Top = false },
                        new Block { Name = "Walls", Side = false, Top = false },
                        new Block { Name = "Slabs", Side = false, Top = false },
                        new Block { Name = "Smooth", Side = false, Top = false },
                        new Block { Name = "Brick", Side = true, Top = true },
                    });

                _profiles.Add(mineralogyProfile);

                WriteFile(@"profiles.cfg", JsonConvert.SerializeObject(_profiles));
            }

            foreach (var item in _profiles)
            {
                if (!_versions.Contains(item.Version))
                    _versions.Add(item.Version);
            }

            comboBoxVersion.DataSource = _versions;
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

        private void comboBoxVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
                LoadProfiles(comboBoxVersion.SelectedItem.ToString());
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

                            default:
                                break;
                        }
                    }
                }

                var generated = generator.RenderJSON(renderBlocks, renderStairs, renderWalls, renderSlabs, renderSmooth, renderBrick);

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
