using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
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

namespace QurasUninstaller
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        private int nProgress = 0;
        private string strStateString = "Initializing files...";
        public ProgressWindow()
        {
            InitializeComponent();
            InitializeInterface();
        }

        private void InitializeInterface()
        {

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
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();

            Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
            thread.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            SetProgressStatus(nProgress);
            if (nProgress == 100)
            {
                lbBigTitle.Content = "Uninstall finished successfully";
                btnFinish.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
            }
        }

        private void WorkThreadFunction()
        {

            nProgress = 0;
            try
            {
                // Step1. Extract all the files to program files
                StopProcess();

                // Step2. Set Configure to the config.json
                nProgress = 20;
                strStateString = "Deleting Files...";

                RemoveFiles();

                // Step3. Create shortcut to the start menu
                nProgress = 80;
                strStateString = "Deleting configurations...";
                RemoveShortcut();
                RegeditRemove();

                nProgress = 100;
                strStateString = "";
            }
            catch(Exception ex)
            {

            }
        }
        
        public void StopProcess()
        {
            try
            {
                foreach (var process in Process.GetProcessesByName("QurasWallet"))
                {
                    process.Kill();
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void RemoveFiles()
        {
            try
            {
                string path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                Directory.Delete(path, true);
            }
            catch(Exception ex)
            {

            }
        }

        public void SetProgressStatus(int nProgressValue)
        {
            if (nProgressValue >= pgInstall.Minimum && nProgressValue <= pgInstall.Maximum)
            {
                pgInstall.Value = nProgressValue;
                lbProgress.Content = nProgressValue + "%";
            }

            lbStatus.Content = strStateString;
        }

        private void RemoveShortcut()
        {
            try
            {
                string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
                string appStartMenuPath = System.IO.Path.Combine(commonStartMenuPath, "Programs", "QurasDev");
                Directory.Delete(appStartMenuPath, true);
            }
            catch(Exception ex)
            {

            }
        }

        private void RegeditRemove()
        {
            Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Quras_dev");
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
