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
using Pure.Wallets;
using Quras_gui_wpf.Global;

namespace Quras_gui_wpf.Controls
{
    /// <summary>
    /// Interaction logic for ApprovalItem.xaml
    /// </summary>
    public partial class ApprovalItem : UserControl
    {
        private string address;
        private Fixed8 amount;

        private UInt160 _script_hash = null;
        public UInt160 ScriptHash
        {
            get
            {
                return _script_hash;
            }
            set
            {
                _script_hash = value;
            }
        }

        public event EventHandler<ApprovalItem> RemoveApprovalItemEvent;
        public ApprovalItem()
        {
            InitializeComponent();
        }

        public ApprovalItem(string address, Fixed8 amount)
        {
            InitializeComponent();

            this.address = address;
            this.amount = amount;
            this._script_hash = Wallet.ToScriptHash(address);

            RefreshInterface();
            RefreshLanguage();
        }

        public void RefreshLanguage()
        {

        }

        public void RefreshInterface()
        {
            
        }

        public string GetAddress()
        {
            return address;
        }

        public Fixed8 GetAmount()
        {
            return amount;
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            RemoveApprovalItemEvent?.Invoke(sender, this);
        }

        private void BtnApprove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDisput_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
