using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Pure;
using Pure.Core;
using Pure.Core.Anonoymous;
using Pure.Cryptography;
using Pure.Wallets;
using Quras_gui.Global;
using Pure.IO;

using MaterialSkin;
using MaterialSkin.Controls;

using QRCoder;

namespace Quras_gui.Dialogs
{
    public partial class ReceiveDialog : MaterialForm
    {
        private string data_;
        private int iLang => Constant.GetLang();

        public ReceiveDialog()
        {
            InitializeComponent();

            // Create a material theme manager and add the form to manage (this)
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Configure color schema
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue700, Primary.Blue700,
                Primary.Blue700, Accent.LightBlue200,
                TextShade.WHITE
            );

            InitInstance();
            InitInterface();
        }

        private void InitInstance()
        {
            // From Combobox initialize
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);
                cmb_from.Items.Add(contract.Address);
            }

            if (cmb_from.Items.Count > 0)
            {
                cmb_from.SelectedIndex = 0;
            }

            // Load Assets
            LoadAssets();

            data_ = $"addr: {cmb_from.Text}, asset_type: {cmb_assets.Text}, balance: {txb_balance}";
            DrawQRCode();
        }

        private void InitInterface()
        {
            this.Text = StringTable.DATA[iLang, 70];
            lbl_address.Text = StringTable.DATA[iLang, 71];
            lbl_addr_comment.Text = StringTable.DATA[iLang, 72];
            lbl_asset_type.Text = StringTable.DATA[iLang, 73];
            lbl_asset_comment.Text = StringTable.DATA[iLang, 74];
            lbl_balance.Text = StringTable.DATA[iLang, 75];
            btn_make_qr_code.Text = StringTable.DATA[iLang, 76];
        }

        private void LoadAssets()
        {
            for (int i = 0; i < AssetsImp.getInstance().GetList().Count; i++)
            {
                AssetState state = Blockchain.Default.GetAssetState(AssetsImp.getInstance().GetList()[i].Asset_ID);
                Global.AssetDescriptor item = new Global.AssetDescriptor(state);

                cmb_assets.Items.Add(item);
            }

            if (cmb_assets.Items.Count > 0)
            {
                cmb_assets.SelectedIndex = 0;
            }
        }

        private void btn_make_qr_code_Click(object sender, EventArgs e)
        {
            data_ = $"addr: {cmb_from.Text}, asset_type: {cmb_assets.Text}, balance: {txb_balance.Text}";
            DrawQRCode();
        }

        private void DrawQRCode()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data_, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            Bitmap result = new Bitmap(pic_qr_code.Width, pic_qr_code.Height);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(qrCodeImage, 0, 0, pic_qr_code.Width, pic_qr_code.Height);

            pic_qr_code.Image = result;
        }
    }
}
