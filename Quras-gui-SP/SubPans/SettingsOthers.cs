using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Quras_gui_SP.Controls;
using Quras_gui_SP.Global;
namespace Quras_gui_SP.SubPans
{
    public partial class SettingsOthers : UserControl
    {
        public List<AssetItem> AssetList;
        public SettingsOthers()
        {
            InitializeComponent();

            InitInstance();
        }

        private void InitInstance()
        {
            AssetList = new List<AssetItem>();
        }

        public void AddAsset(AssetInfoStructure info)
        {
            AssetItem item = new AssetItem(info);

            item.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            item.Location = new System.Drawing.Point(5, 5 + AssetList.Count * 80);
            item.Name = "assetItem" + AssetList.Count() ;
            item.Size = new System.Drawing.Size(476, 70);
            item.TabIndex = 100 + AssetList.Count;

            this.pan_list_assets.Controls.Add(item);

            AssetList.Add(item);
        }
    }
}
