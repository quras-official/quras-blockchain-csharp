using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace Quras_gui.Global
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

        public static int CheckZKSnarksKeyStatus()
        {
            string strAppPath = System.IO.Directory.GetCurrentDirectory();

            string pkPath = strAppPath + "\\crypto\\pk.key";
            string vkPath = strAppPath + "\\crypto\\vk.key";

            string serverPkMd5 = "";
            string serverVkMd5 = "";

            int ret = 0;

            // Get the Key md5
            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load("http://13.230.62.42/quras/Keys/KeyMd5.xml");
            }
            catch { }

            if (xdoc != null)
            {
                serverVkMd5 = xdoc.Element("Md5").Attribute("vkMd5").Value;
                serverPkMd5 = xdoc.Element("Md5").Attribute("pkMd5").Value;
            }
            // end

            if (!File.Exists(vkPath))
            {
                ret += 1;
            }
            else
            {
                using (var md5 = MD5.Create())
                {
                    try
                    {
                        using (var stream = File.OpenRead(vkPath))
                        {
                            byte[] vkMd5 = md5.ComputeHash(stream);
                            string hexVkMd5 = Global.Utils.ByteArrayToString(vkMd5);

                            if (serverVkMd5 != hexVkMd5)
                            {
                                ret += 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ret += 1;
                    }
                }
            }

            if (!File.Exists(pkPath))
            {
                ret += 2;
            }
            else
            {
                using (var md5 = MD5.Create())
                {
                    try
                    {
                        using (var stream = File.OpenRead(pkPath))
                        {
                            byte[] pkMd5 = md5.ComputeHash(stream);
                            string hexPkMd5 = Global.Utils.ByteArrayToString(pkMd5);

                            if (serverPkMd5 != hexPkMd5)
                            {
                                ret += 2;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        ret += 2;
                    }
                }
            }

            return ret;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
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
