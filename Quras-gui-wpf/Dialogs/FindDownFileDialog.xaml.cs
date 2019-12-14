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
using System.Windows.Shapes;
using System.Windows.Threading;

using Quras;
using Quras.Core;

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Controls;
using Quras.Wallets;
using Quras_gui_wpf.Properties;
using System.Net;
using System.Web.Script.Serialization;

namespace Quras_gui_wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for FindDownFileDialog.xaml
    /// </summary>
    public partial class FindDownFileDialog : Window, System.IDisposable
    {
        private LANG iLang => Constant.GetLang();
        private List<TxOutPutItem> outPutItems;
        private DispatcherTimer errorMsgTimer = new DispatcherTimer();
        private Fixed8 totalAmount = Fixed8.Zero;

        public FindDownFileDialog()
        {
            InitializeComponent();
            InitInstance();
        }

        public event EventHandler<HttpDownFileInformation> FileSelectedEvent;

        public FindDownFileDialog(Window parent)
        {
            Owner = parent;
            InitializeComponent();

            InitInstance();
            RefreshLanguage();
        }

        public void InitInstance()
        {
            outPutItems = new List<TxOutPutItem>();
            
        }

        public void RefreshLanguage()
        {
            TxbFindFileTitle.Text = StringTable.GetInstance().GetString("STR_FDFD_TITLE", iLang);
            TxbFileInfoToSearch.Tag = StringTable.GetInstance().GetString("STR_FDFD_TAG_SEARCH_INFO", iLang);
            btnSearch.Content = StringTable.GetInstance().GetString("STR_FDFD_BUTTON_SEARCH", iLang);
            btnOK.Content = StringTable.GetInstance().GetString("STR_FDFD_BUTTON_CANCEL", iLang);
            

            foreach(TxOutPutItem item in outPutItems)
            {
                item.RefreshLanguage();
            }
        }
        private void GridMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {

            }
        }

        public List<TxOutPutItem> GetOutputList()
        {
            return outPutItems;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }


        public void Dispose()
        {
            
        }


        private void RemoveTxOutputItemEvent(object sender, TxOutPutItem item)
        {
            item.RemoveTxOutPutItemEvent -= RemoveTxOutputItemEvent;
            outPutItems.Remove(item);
            this.stackOutPuts.Children.Remove(item);

        }


        private void UpdateTimer(object sender, EventArgs args)
        {
            errorMsgTimer.Stop();
        }


        private void TxbFileInfoToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void DownloadItemClickedEvent(object sender, DownloadFileItem item)
        {
            FileSelectedEvent?.Invoke(sender, item.information);

            this.DialogResult = true;
            this.Close();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            stackOutPuts.Children.Clear();
            var wb = new WebClient();
            var response = "";
            try
            {
                response = wb.DownloadString(SettingsConfig.instance.ApiPrefix + "/v1/file/all");
                string searchContent = TxbFileInfoToSearch.Text;
                List<HttpDownFileInformation> httpData = new JavaScriptSerializer().Deserialize<List<HttpDownFileInformation>>(response);

                foreach (HttpDownFileInformation fileInfo in httpData)
                {
                    if (!(fileInfo.file_name.Contains(searchContent) || fileInfo.file_description.Contains(searchContent) || Wallet.ToAddress(UInt160.Parse(fileInfo.upload_address)).Contains(searchContent)))
                        continue;

                    DownloadFileItem item = new DownloadFileItem();

                    item.TxbFileTitle.Text = fileInfo.file_name;
                    item.TxbUploadAddress.Text = "Uploader : " + Wallet.ToAddress(UInt160.Parse(fileInfo.upload_address));
                    item.TxbPayAmount.Text = fileInfo.pay_amount.ToString() + " XQG";
                    item.ItemClickedEvent += DownloadItemClickedEvent;
                    item.information = fileInfo;

                    stackOutPuts.Children.Add(item);
                }
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}
