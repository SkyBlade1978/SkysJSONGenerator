using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkysJSONGenerator
{
    public partial class MainForm : Form
    {
        private List<Profile> _profiles;

        public MainForm()
        {
            InitializeComponent();

            LoadProfiles("All");

            comboBoxVersion.SelectedIndex = 0;
        }

        private void LoadConfig()
        {
            _profiles = new List<Profile>();

            // TODO: move this to config file
            // mineralogy config
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
            //
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
                var generator = new JSonGenerator((Profile)comboBoxMod.SelectedItem);
                generator.RenderJSON(true, true, true, true, true);
            }

            buttonGenerate.Enabled = true;
        }
    }
}
