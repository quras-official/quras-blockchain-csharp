using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quras_gui.Controls.CustomizeControl.Panels
{
    public partial class TransparentPanel : System.Windows.Forms.Panel
    {
        private const int WS_EX_TRANSPARENT = 0x20;

        private int opacity = 50;

        [DefaultValue(50)]
        public int Opacity
        {
            get
            {
                return this.opacity;
            }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("value must be between 0 and 100");

                this.opacity = value;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cpar = base.CreateParams;
                cpar.ExStyle = cpar.ExStyle | WS_EX_TRANSPARENT;
                return cpar;
            }
        }

        public TransparentPanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Opaque, true);
        }

        public TransparentPanel(IContainer con)
        {

            con.Add(this);

            InitializeComponent();

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            using (var brush = new SolidBrush(Color.FromArgb
            (this.opacity * 255 / 100, this.BackColor)))
            {
                pe.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            base.OnPaint(pe);
        }
    }
}
