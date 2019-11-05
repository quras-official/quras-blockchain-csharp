using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Pure;
using Pure.Core;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;

namespace Quras_gui_wpf.Controls
{
    /// <summary>
    /// Interaction logic for AssetItem.xaml
    /// </summary>
    public partial class AssetItem : UserControl
    {
        public AssetState Asset;
        public Fixed8 Value;
        public Fixed8 Claim;
        public double Rate;

        private LANG iLang => Constant.GetLang();
        public AssetItem()
        {
            InitializeComponent();
        }

        public AssetItem(AssetState asset, Fixed8 value, Fixed8 claim, double rate= 1.5)
        {
            InitializeComponent();

            Asset = asset;
            Value = value;
            Claim = claim;
            Rate = rate;

            RefreshInterface();
        }

        public void RefreshInterface()
        {
            tbAssetName.Text = Asset.GetName();
            tbBalance.Text = Windows.Helper.FormatNumber(Value.ToString());

            Fixed8 usdBalance;
            if (Asset.AssetId == Blockchain.GoverningToken.Hash)
            {
                tbBalanceUSD.Text = $"${Windows.Helper.FormatNumber((Value * Fixed8.Satoshi * 8000000).ToString())}";
                
            }
            else if (Asset.AssetId == Blockchain.UtilityToken.Hash)
            {
                tbBalanceUSD.Text = $"${Windows.Helper.FormatNumber((Value * Fixed8.Satoshi * 2000000).ToString())}";
            }
            else
            {
                tbBalanceUSD.Text = $"${Windows.Helper.FormatNumber(((double)((decimal)Value) * Rate).ToString())}";
            }
            
            
            tbPrecision.Text = String.Format(StringTable.GetInstance().GetString("STR_MENU_ASSET_PRECISION", iLang), Asset.Precision.ToString());
            tbTotalSupply.Text = String.Format(StringTable.GetInstance().GetString("STR_MENU_ASSET_TOTAL_SUPPLY", iLang), Windows.Helper.FormatNumber(Asset.Amount.ToString()));
        }
    }
}
