using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Pure.Core;
using Quras_gui_SP.Global;

namespace Quras_gui_SP.Controls
{
    public partial class AssetItem : UserControl
    {
        public AssetInfoStructure Info;
        public AssetItem()
        {
            InitializeComponent();
        }

        public AssetItem(AssetInfoStructure info)
        {
            InitializeComponent();

            Info = info;
            InitInterface();
        }

        private void InitInterface()
        {
            if (Info.Asset_ID == Blockchain.GoverningToken.Hash || 
                Info.Asset_ID == Blockchain.UtilityToken.Hash)
            {
                btn_delete.Enabled = false;
                btn_edit.Enabled = false;
            }

            if (Info.Asset_ID == Blockchain.GoverningToken.Hash)
            {
                lbl_asset_type.Text = "Governing Token";
            }
            else if (Info.Asset_ID == Blockchain.UtilityToken.Hash)
            {
                lbl_asset_type.Text = "Utility Token";
            }
            else
            {
                lbl_asset_type.Text = "Unknown Token";
            }

            txb_assets_name.Text = Info.Asset_Unit;
            txb_assets_address.Text = Info.Asset_ID.ToString();
        }
    }
}
