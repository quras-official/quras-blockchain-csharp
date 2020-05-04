using QurasInstaller.InstallConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QurasInstaller
{
    /// <summary>
    /// Interaction logic for BlockDirectoryWindow.xaml
    /// </summary>
    public partial class BlockDirectoryWindow : Window
    {
        public BlockDirectoryWindow()
        {
            InitializeComponent();
            InitializeInterface();
        }
        private void InitializeInterface()
        {
            if (Configuration.Default.ChainPath != "")
            {
                txbChainPath.Text = Configuration.Default.ChainPath;
            }
            else
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                path += "\\QurasWallet\\Chain";
                Configuration.Default.CryptoPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\QurasWallet\\Crypto";
                txbChainPath.Text = path;
            }
            
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Default.ChainPath = txbChainPath.Text;
            ProgressWindow NextWindow = new ProgressWindow();
            NextWindow.Show();
            this.Close();
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                if (path.Contains("chain"))
                    txbChainPath.Text = path;
                else if (path.Last<char>() == '\\')
                    txbChainPath.Text = path + "\\QurasWallet\\chain";
                else
                    txbChainPath.Text = path + "\\QurasWallet\\chain";
            }
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            WelcomeWindow PrevWindow = new WelcomeWindow();
            PrevWindow.Show();
            this.Close();
        }
    }
}
