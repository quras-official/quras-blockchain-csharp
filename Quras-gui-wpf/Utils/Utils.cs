using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Media;
using System.Security.Cryptography;

using Quras_gui_wpf.Global;
using Quras_gui_wpf.Dialogs.NotifyMessage;

namespace Quras_gui_wpf.Utils
{
    public static class StaticUtils
    {
        public static Brush ErrorBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFD, 0x4C, 59));
        public static Brush GreenBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x4f, 0xBF, 0x81));
        public static Brush BlueBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x43, 0x91, 0xDB));

        public static void ShowMessageBox(System.Windows.Media.Brush skin, string body)
        {
            NotifyMessage msg = null;

            msg = new NotifyMessage(skin, body,
                                    () => /*MessageBox.Show("Green Skin has been chosen.", "Green Skin", MessageBoxButton.OK)*/ skin = skin );

            Constant.NotifyMessageMgr.EnqueueMessage(msg);
        }

        /// <summary>
        /// Check the zk-snarks params
        /// </summary>
        /// <returns>
        /// 0 : success
        /// 1 : vkKey does not exist
        /// 2 : pkKey does not exist
        /// 3 : vkKey and pkKey both does not exist
        /// </returns>
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
                            string hexVkMd5 = ByteArrayToString(vkMd5);

#if DEBUG
                            
#else
                            if (serverVkMd5 != hexVkMd5)
                            {
                                ret += 1;
                            }
#endif

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
                            string hexPkMd5 = ByteArrayToString(pkMd5);
#if DEBUG
#else
                            if (serverPkMd5 != hexPkMd5)
                            {
                                ret += 2;
                            }
#endif
                        }
                    }
                    catch (Exception ex)
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
    }
}
