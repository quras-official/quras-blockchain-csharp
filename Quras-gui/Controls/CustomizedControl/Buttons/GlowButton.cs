using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Quras_gui.Controls.CustomizeControl.Buttons
{
    public class GlowButton : Button
    {
        Timer timer;
        int alpha = 0;
        public Color GlowColor { get; set; }

        public GlowButton()
        {
            this.DoubleBuffered = true;
            timer = new Timer() { Interval = 20 };
            timer.Tick += timer_Tick;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GlowColor = Color.Silver;
            this.FlatAppearance.MouseDownBackColor = Color.DarkGray;
        }

        public GlowButton(Size size, int radius)
        {
            this.DoubleBuffered = true;
            timer = new Timer() { Interval = 20 };
            timer.Tick += timer_Tick;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GlowColor = Color.Silver;
            this.FlatAppearance.MouseDownBackColor = Color.DarkGray;
            //this.Size = ControlSize;

            GraphicsPath p = RoundedRect(new Rectangle(this.Location, size), radius);
            this.Region = new Region(p);
        }

        public GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.FlatAppearance.MouseOverBackColor = CalculateColor();
            timer.Start();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            timer.Stop();
            alpha = 0;
            this.FlatAppearance.MouseOverBackColor = CalculateColor();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            int increament = 25;
            if (alpha + increament < 255) { alpha += increament; }
            else { timer.Stop(); alpha = 255; }
            this.FlatAppearance.MouseOverBackColor = CalculateColor();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) timer.Dispose();
            base.Dispose(disposing);
        }
        private Color CalculateColor()
        {
            return AlphaBlend(Color.FromArgb(alpha, GlowColor), this.BackColor);
        }
        public Color AlphaBlend(Color A, Color B)
        {
            var r = (A.R * A.A / 255) + (B.R * B.A * (255 - A.A) / (255 * 255));
            var g = (A.G * A.A / 255) + (B.G * B.A * (255 - A.A) / (255 * 255));
            var b = (A.B * A.A / 255) + (B.B * B.A * (255 - A.A) / (255 * 255));
            var a = A.A + (B.A * (255 - A.A) / 255);
            return Color.FromArgb(a, r, g, b);
        }
    }
}
