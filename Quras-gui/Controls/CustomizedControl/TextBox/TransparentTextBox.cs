using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quras_gui.Controls.CustomizeControl.TextBox
{
    public partial class TransparentTextBox : System.Windows.Forms.TextBox
    {
        public TransparentTextBox()
        {
            InitializeComponent();

            SetStyle(ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //BackColor = Color.Transparent;

            TextChanged += UserControl2_OnTextChanged;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            
            var backgroundBrush = new SolidBrush(Color.Transparent);
            Graphics g = pe.Graphics;
            g.FillRectangle(backgroundBrush, 0, 0, this.Width, this.Height);

            if (PasswordChar != '\0')
            {
                string password = "";
                for (int i = 0; i < Text.Length; i++)
                    password = password + "*";
                g.DrawString(password, Font, new SolidBrush(ForeColor), new PointF(0, 0), StringFormat.GenericDefault);
            }
            else
            {
                g.DrawString(Text, Font, new SolidBrush(ForeColor), new PointF(0, 0));
            }
            
        }

        public void UserControl2_OnTextChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
