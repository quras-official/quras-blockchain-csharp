using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace KeyManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnPkPath_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "wallet"; // Default file name
            dlg.DefaultExt = "key";
            dlg.Filter = "Key Files (*.key)|*.key|All Files (*.*)|*.*";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                txbPkPath.Text = dlg.FileName;
            }
        }

        private void btnVkPath_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "wallet"; // Default file name
            dlg.DefaultExt = "key";
            dlg.Filter = "Key Files (*.key)|*.key|All Files (*.*)|*.*";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                txbVkPath.Text = dlg.FileName;
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private void btnMake_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(txbPkPath.Text))
            {
                using (var md5 = MD5.Create())
                {
                    try
                    {
                        using (var stream = File.OpenRead(txbPkPath.Text))
                        {
                            byte[] pkMd5 = md5.ComputeHash(stream);
                            txbPKMd5.Text = ByteArrayToString(pkMd5);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }

            if (File.Exists(txbVkPath.Text))
            {
                using (var md5 = MD5.Create())
                {
                    try
                    {
                        using (var stream = File.OpenRead(txbVkPath.Text))
                        {
                            byte[] vkMd5 = md5.ComputeHash(stream);
                            txbVKMd5.Text = ByteArrayToString(vkMd5);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
    }
}
