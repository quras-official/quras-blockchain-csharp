using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Quras_gui_SP.Global
{
    public static class Utils
    {
        public enum Effect { Roll, Slide, Center, Blend }

        public static void Animate(Control ctl, Effect effect, int msec, int angle)
        {
            int flags = effmap[(int)effect];
            if (ctl.Visible) { flags |= 0x10000; angle += 180; }
            else
            {
                if (ctl.TopLevelControl == ctl) flags |= 0x20000;
                else if (effect == Effect.Blend) throw new ArgumentException();
            }
            flags |= dirmap[(angle % 360) / 45];
            bool ok = AnimateWindow(ctl.Handle, msec, flags);
            if (!ok) throw new Exception("Animation failed");
            ctl.Visible = !ctl.Visible;
        }

        private static int[] dirmap = { 1, 5, 4, 6, 2, 10, 8, 9 };
        private static int[] effmap = { 0, 0x40000, 0x10, 0x80000 };

        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr handle, int msec, int flags);
    }

    public class WinAPI
    {
        /// <summary>
        /// Animates the window from left to right. This flag can be used with roll or slide animation.
        /// </summary>
        public const int AW_HOR_POSITIVE = 0X1;
        /// <summary>
        /// Animates the window from right to left. This flag can be used with roll or slide animation.
        /// </summary>
        public const int AW_HOR_NEGATIVE = 0X2;
        /// <summary>
        /// Animates the window from top to bottom. This flag can be used with roll or slide animation.
        /// </summary>
        public const int AW_VER_POSITIVE = 0X4;
        /// <summary>
        /// Animates the window from bottom to top. This flag can be used with roll or slide animation.
        /// </summary>
        public const int AW_VER_NEGATIVE = 0X8;
        /// <summary>
        /// Makes the window appear to collapse inward if AW_HIDE is used or expand outward if the AW_HIDE is not used.
        /// </summary>
        public const int AW_CENTER = 0X10;
        /// <summary>
        /// Hides the window. By default, the window is shown.
        /// </summary>
        public const int AW_HIDE = 0X10000;
        /// <summary>
        /// Activates the window.
        /// </summary>
        public const int AW_ACTIVATE = 0X20000;
        /// <summary>
        /// Uses slide animation. By default, roll animation is used.
        /// </summary>
        public const int AW_SLIDE = 0X40000;
        /// <summary>
        /// Uses a fade effect. This flag can be used only if hwnd is a top-level window.
        /// </summary>
        public const int AW_BLEND = 0X80000;

        /// <summary>
        /// Animates a window.
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int AnimateWindow(IntPtr hwand, int dwTime, int dwFlags);
    }
}
