using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quras_gui.Global;

namespace Quras_gui.Dialogs
{
    public enum AlertLevel
    {
        Warning,
        Show
    }
    public partial class AlertDialog : Form
    {
        private int Width_;
        private int Height_;
        private AlertLevel alertLevel;
        private int FromPosH;
        private int ToPosH;
        private string Title;
        private string Body;

        private int current_location_x;
        private int current_location_y;

        private int iLang => Constant.GetLang();

        public AlertDialog()
        {
            InitializeComponent();
        }

        public AlertDialog(AlertLevel level,Point startPostion, int FromPos, int ToPos, string title, string body)
        {
            InitializeComponent();
            Width_ = Width;
            Height_ = Height;

            alertLevel = level;
            FromPosH = FromPos;
            ToPosH = ToPos;
            Title = title;
            Body = body;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = startPostion;
            InitInterface();
        }

        public AlertDialog(int width, int height)
        {
            InitializeComponent();
            Width_ = width;
            Height_ = height;
            InitInterface();
        }

        private void InitInterface()
        {
            this.Width = Width_;
            this.Height = Height_;

            switch (alertLevel)
            {
                case AlertLevel.Warning:
                    break;
                case AlertLevel.Show:
                    break;
            }

            current_location_x = this.Location.X;
            current_location_y = this.Location.Y;

            tm_show_dlg.Enabled = true;

            lbl_title.Text = Title;
            lbl_body.Text = Body;
        }
        private void AlertDialog_Load(object sender, EventArgs e)
        {

        }

        private void AlertDialog_Click(object sender, EventArgs e)
        {
                this.DialogResult = DialogResult.OK;
        }

        private void tm_show_dlg_Tick(object sender, EventArgs e)
        {
            if (current_location_y < ToPosH)
            {
                tm_show_dlg.Enabled = false;
            }

            current_location_y -= 5;
            this.SetDesktopLocation(current_location_x, current_location_y);
        }
    }
}
