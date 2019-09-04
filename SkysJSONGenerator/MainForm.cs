using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            checkedListBoxOutput.SetItemChecked(0, true);
            checkedListBoxOutput.SetItemChecked(1, true);
            checkedListBoxOutput.SetItemChecked(4, true);
        }

        private void LoadConfig()
        {
            _profiles = new List<Profile>();
            _versions = new List<string>();

            if (File.Exists(@"profiles.cfg"))
            {
                using (StreamReader file = File.OpenText(@"profiles.cfg"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    _profiles = (List<Profile>)serializer.Deserialize(file, typeof(List<Profile>));
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
                    "blocks", "items");

                _profiles.Add(mineralogyProfile);
                
                using (StreamWriter file = File.CreateText(@"profiles.cfg"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, _profiles);
                }
            }

            foreach (var item in _profiles)
            {
                if (!_versions.Contains(item.Version))
                    _versions.Add(item.Version);
            }

            comboBoxVersion.DataSource = _versions;
            
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
            buttonGenerate.Enabled = false;

            if (comboBoxMod.SelectedIndex >= 0)
            {
                var selectedProfile = (Profile)comboBoxMod.SelectedItem;
                var basePath = "out\\" + selectedProfile.Modid + "\\" + selectedProfile.Version;
                var generator = new JSonGenerator(selectedProfile, basePath);
                var generated = generator.RenderJSON(checkedListBoxOutput.GetItemChecked(0), checkedListBoxOutput.GetItemChecked(1), checkedListBoxOutput.GetItemChecked(2), checkedListBoxOutput.GetItemChecked(3), checkedListBoxOutput.GetItemChecked(4), checkedListBoxOutput.GetItemChecked(5));

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
    }
}
