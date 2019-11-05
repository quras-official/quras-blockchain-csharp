using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pure.UI
{
    public partial class GetAssetInfo : Form
    {
        public GetAssetInfo()
        {
            InitializeComponent();

            InitInterface();
        }

        private void InitInterface()
        {
            txb_quras_asset.Text = Pure.Core.Blockchain.GoverningToken.Hash.ToString();
            txb_quras_gas_asset.Text = Core.Blockchain.UtilityToken.Hash.ToString();
        }
    }
}
