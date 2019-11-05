using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

using Quras_gui.Global;

namespace Quras_gui.FormUI
{
    public partial class SplashForm : Form
    {
        private int iLang => Constant.GetLang();

        private string[] STR_WALLET_VERSION = { "Wallet Version : ", "ウォレットバージョン : " };
        private string[] STR_QURAS_WALLET = { "Quras Wallet", "Qurasウォレット" };

        public SplashForm()
        {
            InitializeComponent();

            lbl_title.Text = STR_QURAS_WALLET[iLang];
            lbl_status.Text = STR_WALLET_VERSION[iLang] + Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
