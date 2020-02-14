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

using Quras;
using Quras.Wallets;
using Quras_gui_wpf.Global;

namespace Quras_gui_wpf.Controls
{
    /// <summary>
    /// Interaction logic for ApprovalItem.xaml
    /// </summary>
    public partial class ApprovalItem : UserControl
    {
        public UInt256 dTXhash = null;

        public event EventHandler<ApprovalItem> ApproveTxEvent;
        public event EventHandler<ApprovalItem> DisputTxEvent;

        public ApprovalItem()
        {
            InitializeComponent();
        }

        

        public void RefreshLanguage()
        {

        }

        public void RefreshInterface()
        {
            
        }

        private void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            ApproveTxEvent?.Invoke(sender, this);
        }

        private void BtnDisput_Click(object sender, RoutedEventArgs e)
        {
            DisputTxEvent?.Invoke(sender, this);
        }
    }
}
