using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Runtime.InteropServices;

using MaterialSkin;
using MaterialSkin.Controls;

using Quras_gui.Properties;
using Quras_gui.Global;

namespace Quras_gui.FormUI
{
    public partial class UpdateWindow : MaterialForm
    {
        private readonly WebClient web = new WebClient();
        private readonly string download_url;
        private string download_path;
        private int iLang => Constant.GetLang();

        public UpdateWindow()
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

        public UpdateWindow(XDocument xdoc)
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

            Version latest = Version.Parse(xdoc.Element("update").Attribute("latest").Value);
            txb_update_version.Text = latest.ToString();
            XElement release = xdoc.Element("update").Elements("release").First(p => p.Attribute("version").Value == latest.ToString());
            txb_update_logs.Text = release.Element("changes").Value.Replace("\n", Environment.NewLine);
            download_url = release.Attribute("file").Value;
            web.DownloadProgressChanged += Web_DownloadProgressChanged;
            web.DownloadFileCompleted += Web_DownloadFileCompleted;

            pan_percent.Width = 0;
            InitInterface();
        }

        public void InitInterface()
        {
            lbl_update_version.Text = StringTable.DATA[iLang, 24];
            this.Text = StringTable.DATA[iLang, 25];
            lbl_change_log.Text = StringTable.DATA[iLang, 26];
            btn_update.Text = StringTable.DATA[iLang, 27];
            btn_cancel.Text = StringTable.DATA[iLang, 28];
        }

        private void Web_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
            lbl_percent.Text = e.ProgressPercentage.ToString() + "%";
            pan_percent.Width = pan_percent_full.Width / 100 * e.ProgressPercentage;

            if (e.ProgressPercentage == 100)
            {
                pan_percent.Width = pan_percent_full.Width - 2;
            }
        }

        private void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

            if (e.Cancelled || e.Error != null) return;
            DirectoryInfo di = new DirectoryInfo("update");
            if (di.Exists) di.Delete(true);
            di.Create();
            ZipFile.ExtractToDirectory(download_path, di.Name);
            FileSystemInfo[] fs = di.GetFileSystemInfos();
            if (fs.Length == 1 && fs[0] is DirectoryInfo)
            {
                ((DirectoryInfo)fs[0]).MoveTo("update2");
                di.Delete();
                Directory.Move("update2", di.Name);
            }
            File.WriteAllBytes("update.bat", Resources.UpdateBat);
            Close();
            Process.Start("update.bat");

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

        private void btn_update_Click(object sender, EventArgs e)
        {
            btn_cancel.Enabled = false;
            download_path = "update.zip";
            web.DownloadFileAsync(new Uri(download_url), download_path);
        }
    }
}
