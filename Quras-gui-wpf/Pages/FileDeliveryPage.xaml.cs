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

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;
using Pure.Wallets;
using Pure;
using Pure.Implementations.Wallets.EntityFramework;

namespace Quras_gui_wpf.Pages
{
    public enum FileDeliveryPageStatus
    {
        Upload,
        Download,
        Approve,
        Other
    }

    /// <summary>
    /// Interaction logic for FileDeliveryPage.xaml
    /// </summary>
    public partial class FileDeliveryPage : UserControl
    {
        private FileDeliveryPageStatus pageStatus;

        private UploadPage uploadPage;
        private DownloadPage downloadPage;
        private ApprovePage approvePage;

        private LANG iLang => Constant.GetLang();

        public FileDeliveryPage()
        {
            InitializeComponent();

            InitInstance();
            RefreshLanguage();
        }

        private void InitInstance()
        {
            uploadPage = new UploadPage();
            downloadPage = new DownloadPage();
            approvePage = new ApprovePage();

            pageStatus = FileDeliveryPageStatus.Upload;
            ShowPages(pageStatus);
        }

        public void RefreshLanguage()
        {
            btnUploadFile.Content = StringTable.GetInstance().GetString("STR_SMARTCONTRACT_UPLOAD_BUTTON", iLang);
            btnDownloadFile.Content = StringTable.GetInstance().GetString("STR_SMARTCONTRACT_DOWNLOAD_BUTTON", iLang);
 

            uploadPage.RefreshLanguage();
            downloadPage.RefreshLanguage();

            ShowPages(pageStatus);
        }

        private void ShowPages(FileDeliveryPageStatus status)
        {
            switch (status)
            {
                case FileDeliveryPageStatus.Upload:
                    btnUploadFile.IsChecked = true;
                    btnDownloadFile.IsChecked = false;
                    btnApproveFile.IsChecked = false;

                    if (iLang == LANG.EN)
                    {
                        btnUploadFile.FontWeight = FontWeights.Bold;
                        btnDownloadFile.FontWeight = FontWeights.Normal;
                        btnApproveFile.FontWeight = FontWeights.Normal;
                    }
                    else
                    {
                        btnUploadFile.FontWeight = FontWeights.Heavy;
                        btnDownloadFile.FontWeight = FontWeights.Normal;
                        btnApproveFile.FontWeight = FontWeights.Normal;
                    }
                    
                    panISC.Visibility = Visibility.Hidden;
                    panDSC.Visibility = Visibility.Hidden;
                    panAS.Visibility = Visibility.Hidden;
                    panOthers.Visibility = Visibility.Hidden;
                    panPrev.Visibility = Visibility.Hidden;

                    pageFileDelivery.ShowPage(uploadPage);
                    break;
                case FileDeliveryPageStatus.Download:
                    btnUploadFile.IsChecked = false;
                    btnDownloadFile.IsChecked = true;
                    btnApproveFile.IsChecked = false;

                    if (iLang == LANG.EN)
                    {
                        btnUploadFile.FontWeight = FontWeights.Normal;
                        btnDownloadFile.FontWeight = FontWeights.Bold;
                        btnApproveFile.FontWeight = FontWeights.Normal;

                    }
                    else
                    {
                        btnUploadFile.FontWeight = FontWeights.Normal;
                        btnDownloadFile.FontWeight = FontWeights.Heavy;
                        btnApproveFile.FontWeight = FontWeights.Normal;
                    }
                    
                    panISC.Visibility = Visibility.Hidden;
                    panDSC.Visibility = Visibility.Hidden;
                    panAS.Visibility = Visibility.Hidden;
                    panOthers.Visibility = Visibility.Hidden;
                    panPrev.Visibility = Visibility.Hidden;

                    pageFileDelivery.ShowPage(downloadPage);
                    break;

                case FileDeliveryPageStatus.Approve:
                    btnUploadFile.IsChecked = false;
                    btnDownloadFile.IsChecked = false;
                    btnApproveFile.IsChecked = true;

                    if (iLang == LANG.EN)
                    {
                        btnUploadFile.FontWeight = FontWeights.Normal;
                        btnDownloadFile.FontWeight = FontWeights.Normal;
                        btnApproveFile.FontWeight = FontWeights.Bold;

                    }
                    else
                    {
                        btnUploadFile.FontWeight = FontWeights.Normal;
                        btnDownloadFile.FontWeight = FontWeights.Normal;
                        btnApproveFile.FontWeight = FontWeights.Heavy;
                    }

                    panISC.Visibility = Visibility.Hidden;
                    panDSC.Visibility = Visibility.Hidden;
                    panAS.Visibility = Visibility.Hidden;
                    panOthers.Visibility = Visibility.Hidden;
                    panPrev.Visibility = Visibility.Hidden;

                    pageFileDelivery.ShowPage(approvePage);
                    break;
            }

        }

        private void btnUploadFile_Click(object sender, RoutedEventArgs e)
        {
            btnUploadFile.IsChecked = true;
            if (pageStatus == FileDeliveryPageStatus.Upload) return;

            pageStatus = FileDeliveryPageStatus.Upload;
            ShowPages(pageStatus);
        }

        private void btnDownloadFile_Click(object sender, RoutedEventArgs e)
        {
            btnDownloadFile.IsChecked = true;
            if (pageStatus == FileDeliveryPageStatus.Download) return;

            pageStatus = FileDeliveryPageStatus.Download;
            ShowPages(pageStatus);
        }

        private void btnApproveFile_Click(object sender, RoutedEventArgs e)
        {
            btnApproveFile.IsChecked = true;
            if (pageStatus == FileDeliveryPageStatus.Approve) return;

            pageStatus = FileDeliveryPageStatus.Approve;
            ShowPages(pageStatus);
        }

        public void AddApprovalItem(TransactionInfo info)
        {
            approvePage.AddApprovalItem(info);
        }

        public void RemoveApprovalItem(UInt256 dTXhash)
        {
            approvePage.RemoveApprovalItem(dTXhash);
        }
        public void AddApprovalToPending(UInt256 dTXhash)
        {
            downloadPage.AddApprovalToPending(dTXhash);
        }

        public void AddPendingFileItem(TransactionInfo info)
        {
            downloadPage.AddPendingFileItem(info);
        }

        public void SetPayFlag(UInt256 dTXhash)
        {
            downloadPage.SetPayFlag(dTXhash);
        }
    }
}
