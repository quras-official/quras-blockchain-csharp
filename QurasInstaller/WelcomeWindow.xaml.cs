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
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
            InitializeInterface();
        }

        private void InitializeInterface()
        {
            if (Configuration.Default.InstallPath != "")
                txbInstallPath.Text = Configuration.Default.InstallPath;
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
            Configuration.Default.InstallPath = txbInstallPath.Text;
            BlockDirectoryWindow NextWindow = new BlockDirectoryWindow();
            NextWindow.Show();
            this.Close();
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                if (path.Contains("QurasWallet"))
                    txbInstallPath.Text = path;
                else if (path.Last<char>() == '\\')
                    txbInstallPath.Text = path + "QurasDev\\QurasWallet";
                else
                    txbInstallPath.Text = path + "\\QurasDev\\QurasWallet";
            }
        }
    }
}
