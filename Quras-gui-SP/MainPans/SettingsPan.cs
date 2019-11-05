using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Quras_gui_SP.Global;

namespace Quras_gui_SP.MainPans
{
    public partial class SettingsPan : UserControl
    {
        public SettingsPan()
        {
            InitializeComponent();

            InitInterface();
        }

        private void InitInterface()
        {
            //settingsEngine1.Top = 0;
            //settingsEngine1.Left = 0;
            //settingsEngine1.BringToFront();
        }

        public void AddAsset(AssetInfoStructure info)
        {
            //settingsOthers1.AddAsset(info);
        }

        private void btn_engine_Click(object sender, EventArgs e)
        {
            pan_select.Left = btn_engine.Left;

            //settingsEngine1.Top = 0;
            //settingsEngine1.Left = 0;
            //settingsEngine1.BringToFront();
        }

        private void btn_others_Click(object sender, EventArgs e)
        {
            pan_select.Left = btn_others.Left;

            //settingsOthers1.Top = 0;
            //settingsOthers1.Left = 0;
            //settingsOthers1.BringToFront();
        }
    }
}
