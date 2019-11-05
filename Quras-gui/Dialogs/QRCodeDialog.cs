using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using QRCoder;
using Quras_gui.Global;
namespace Quras_gui.Dialogs
{
    public partial class QRCodeDialog : Form
    {
        private string data_;
        private int iLang => Constant.GetLang();

        public QRCodeDialog(string data)
        {
            InitializeComponent();

            data_ = data;
            DrawQRCode();
            InitInterface();
        }
        public QRCodeDialog()
        {
            InitializeComponent();
            InitInterface();
        }

        private void InitInterface()
        {
            lbl_comment.Text = StringTable.DATA[iLang, 48];
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

        private void pic_qr_code_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        // Form Move
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
