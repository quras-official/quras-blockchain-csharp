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

namespace Quras_gui_SP.Dialogs
{
    public partial class WarningDlg : Form
    {
        public WarningDlg(string title = "", string content = "")
        {
            InitializeComponent();
            InitInterface(title, content);
        }

        void InitInterface(string title, string content)
        {
            lbl_title.Text = title;
            lbl_warning.Text = content;
        }

        private void btn_confirm_Click(object sender, EventArgs e)
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
