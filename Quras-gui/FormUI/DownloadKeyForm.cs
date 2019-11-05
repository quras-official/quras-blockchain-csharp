using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Runtime.InteropServices;

using MaterialSkin;
using MaterialSkin.Controls;

using Quras_gui.Global;

namespace Quras_gui.FormUI
{
    public partial class DownloadKeyForm : MaterialForm
    {
        private readonly WebClient web = new WebClient();
        private readonly string vkKeyUrl = "http://13.230.62.42/quras/Keys/vk.key";
        private readonly string pkKeyUrl = "http://13.230.62.42/quras/Keys/pk.key";

        private static string download_path = System.IO.Directory.GetCurrentDirectory();

        private string vkPath = download_path + "\\crypto\\vk.key";
        private string pkPath = download_path + "\\crypto\\pk.key";
        private string status_text = "Downloading Verify Key. ";

        private int key_status;

        private int iLang => Constant.GetLang();

        private string[] STR_TITLE = { "Download ZK-Snarks keys", "ZK-Snarksキーのダウンロード" };
        private string[] STR_COMMENT = { "You have to download zk-snarks keys. \r\nWithout this, you can't use the anonymous transaction.", "ニックネームトランザクションのためにはZK-Snarks　キーのダウンロードが必要です。" };
        private string[] STR_SKIP = { "SKIP", "スキップ" };
        private string[] STR_CLOSE = { "CLOSE", "閉じる" };

        private string[] STR_DOWNLOADING_VK = { "Downloading Verify key. ", "検証キーのダウンロード。" };
        private string[] STR_DOWNLOADING_PK = { "Downloading Public key. ", "公開キーのダウンロード。" };

        public DownloadKeyForm()
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

            InitInterface();
        }

        public DownloadKeyForm(int status)
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

            web.DownloadProgressChanged += Web_DownloadProgressChanged;
            web.DownloadFileCompleted += Web_DownloadFileCompleted;

            pan_percent.Width = 0;
            key_status = status;

            InitInterface();
            InitInstance();
        }

        private void InitInterface()
        {
            this.Text = STR_TITLE[iLang];
            lbl_cmt_status.Text = STR_COMMENT[iLang];
            btn_skip.Text = STR_SKIP[iLang];
            btn_cancel.Text = STR_CLOSE[iLang];
        }

        private void InitInstance()
        {
            if (key_status == 1)
            {
                status_text = STR_DOWNLOADING_VK[iLang] ;
                web.DownloadFileAsync(new Uri(vkKeyUrl), vkPath);
            }
            if (key_status == 2)
            {
                status_text = STR_DOWNLOADING_PK[iLang];
                web.DownloadFileAsync(new Uri(pkKeyUrl), pkPath);
            }
            if (key_status == 3)
            {
                status_text = STR_DOWNLOADING_VK[iLang];
                web.DownloadFileAsync(new Uri(vkKeyUrl), vkPath);
            }
        }

        private void Web_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
            lbl_status.Text = status_text + e.ProgressPercentage.ToString() + "%";
            pan_percent.Width = pan_percent_full.Width / 100 * e.ProgressPercentage;

            if (e.ProgressPercentage == 100)
            {
                pan_percent.Width = pan_percent_full.Width - 2;
            }
        }

        private void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (key_status == 0)
            {
                this.DialogResult = DialogResult.OK;
            }

            if (key_status == 3)
            {
                status_text = STR_DOWNLOADING_PK[iLang];
                web.DownloadFileAsync(new Uri(pkKeyUrl), pkPath);

                key_status = 0;
            }
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

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_skip_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
