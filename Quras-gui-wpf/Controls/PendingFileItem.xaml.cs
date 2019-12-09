using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
    public partial class PendingFileItem : System.Windows.Controls.UserControl
    {
        public DownloadRequestTransaction transInfo;
        public int approvalTotal;
        public int approved;

        public event EventHandler<PendingFileItem> payTxEvent;
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

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            string targetFilePath = "";

            /**/
            SaveFileDialog saveDlg = new SaveFileDialog();

            saveDlg.InitialDirectory = @"C:\";
            saveDlg.Title = "Save text Files";
            saveDlg.CheckPathExists = true;
            saveDlg.FileName = transInfo.FileName;
            saveDlg.Filter = "All files (*.*)|*.*";
            saveDlg.FilterIndex = 2;
            saveDlg.RestoreDirectory = true;

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                targetFilePath = saveDlg.FileName;
            }
            /**/
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new System.Uri(transInfo.FileURL),
                    // Param2 = Path to save
                    targetFilePath
                );
            }

        }
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progDownPercent.Value = e.ProgressPercentage;
        }
        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            payTxEvent?.Invoke(sender, this);
        }

    }
}
