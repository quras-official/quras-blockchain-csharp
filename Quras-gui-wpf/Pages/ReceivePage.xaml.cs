using System;
using System.IO;
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
using System.Drawing;

using QRCoder;

using Pure;
using Pure.Wallets;

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for ReceivePage.xaml
    /// </summary>
    public partial class ReceivePage : UserControl
    {
        private string data_;
        private LANG iLang => Constant.GetLang();

        public ReceivePage()
        {
            InitializeComponent();
            RefreshLanguage();
        }

        public void RefreshLanguage()
        {
            txbAmount.Tag = StringTable.GetInstance().GetString("STR_RP_AMOUNT", iLang);
            btnGenerate.Content = StringTable.GetInstance().GetString("STR_RP_GENERATE", iLang);
        }

        private string CheckParameter()
        {
            double amount = 0;
            if (txbAmount.Text.Length == 0)
            {
                return "STR_RP_ERR_INPUT_AMOUNT";
            }
            try
            {
                if (cmbAssets.Text == "XQG")
                {
                    amount = double.Parse(txbAmount.Text);
                }
                else if (cmbAssets.Text == "XQC")
                {
                    amount = double.Parse(txbAmount.Text);
                }
                else
                {
                    amount = double.Parse(txbAmount.Text);
                }
            }
            catch
            {
                return "STR_RP_ERR_AMOUNT_FORMAT";
            }

            if (amount <= 0)
            {
                return "STR_RP_ERR_INPUT_AMOUNT";
            }

            return "STR_RP_SUCCESS";
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string strCheck = CheckParameter();

            if (strCheck != "STR_RP_SUCCESS")
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString(strCheck, iLang));
                return;
            }

            // Get Address
            string receiveAddress = "";
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);
                receiveAddress = contract.Address;
            }

            data_ = $"addr: {receiveAddress}, asset_type: {cmbAssets.Text}, balance: {txbAmount.Text}";

            DrawQRCode();
        }

        private void DrawQRCode()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data_, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            BitmapImage qrBtmImg = new BitmapImage();

            qrBtmImg.BeginInit();

            MemoryStream ms = new MemoryStream();

            // Save to a memory stream...
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            // Rewind the stream...
            ms.Seek(0, SeekOrigin.Begin);

            // Tell the WPF image to use this stream...
            qrBtmImg.StreamSource = ms;

            qrBtmImg.EndInit();

            ImgQRCode.Source = qrBtmImg;

            /*
            Bitmap result = new Bitmap(ImgQRCode.Width, pic_qr_code.Height);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(qrCodeImage, 0, 0, pic_qr_code.Width, pic_qr_code.Height);

            pic_qr_code.Image = result;
            */
        }

        public void AddAsset(UInt256 assetId, string assetName, Fixed8 value)
        {
            ComboBoxItem item = new ComboBoxItem();

            AssetTypeItem tagItem = new AssetTypeItem(assetId, assetName, value);

            item.Tag = tagItem;
            item.Content = assetName;

            if (!cmbAssets.Items.Contains(item))
            {
                cmbAssets.Items.Add(item);
            }

            if (cmbAssets.SelectedItem == null)
            {
                cmbAssets.SelectedIndex = 0;
            }
        }
    }
}
