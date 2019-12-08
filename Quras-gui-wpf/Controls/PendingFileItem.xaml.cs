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
using Pure.Wallets;
using Quras_gui_wpf.Global;

namespace Quras_gui_wpf.Controls
{
    /// <summary>
    /// Interaction logic for PendingFileItem.xaml
    /// </summary>
    public partial class PendingFileItem : UserControl
    {
        public DownloadRequestTransaction transInfo;
        public int approvalTotal;
        public int approved;
        public event EventHandler<PendingFileItem> RemovePendingFileItemEvent;
        public PendingFileItem()
        {
            approvalTotal = approved = 0;
            InitializeComponent();
        }

        public PendingFileItem(string address, Fixed8 amount)
        {
            InitializeComponent();

            RefreshInterface();
            RefreshLanguage();
        }

        public void RefreshLanguage()
        {

        }

        public void RefreshInterface()
        {
            
        }


        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            RemovePendingFileItemEvent?.Invoke(sender, this);
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            btnPay.Visibility = Visibility.Hidden;
            btnDownload.Visibility = Visibility.Visible;
            progDownPercent.Visibility = Visibility.Visible;
        }
    }
}
