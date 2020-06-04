using IWshRuntimeLibrary;
using Microsoft.Win32;
using QurasInstaller.InstallConfig;
using System;
using System.Collections.Generic;
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

namespace QurasInstaller
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        private int nProgress = 0;
        private string strStateString = "Initializing files...";
        private System.Windows.Threading.DispatcherTimer dispatcherTimer;

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
            FinishWindow NextWindow = new FinishWindow();
            NextWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
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
                btnNext.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                lbBigTitle.Content = "Installation completed";
                dispatcherTimer.Stop();
            }
        }

        private void WorkThreadFunction()
        {
            nProgress = 0;
            try
            {
                // Step1. Extract all the files to program files
                ExtractFiles();

                // Step2. Set Configure to the config.json
                nProgress = 70;
                strStateString = "Configuring settings ...";

                SetConfigure();

                // Step3. Create shortcut to the start menu
                nProgress = 80;
                strStateString = "Registering application...";
                AddShortcut();

                nProgress = 90;
                CreateUninstaller();

                nProgress = 100;
                strStateString = "";
                
            }
            catch(Exception ex)
            {

            }
        }

        private void ExtractFiles() 
        {
            ResourceManager rm = new ResourceManager("QurasInstaller.Properties.Resources", Assembly.GetExecutingAssembly());
            try
            {
                Directory.Delete(Configuration.Default.InstallPath, true);
            }
            catch(Exception ex)
            {

            }

            Directory.CreateDirectory(Configuration.Default.InstallPath);

            string zipFilePath = System.IO.Path.GetTempPath() + "Quras_wallet.zip";
            System.IO.File.WriteAllBytes(zipFilePath, (byte[])rm.GetObject("QurasWallet"));

            nProgress = 20;
            strStateString = "Extracting Files ...";

            ZipFile.ExtractToDirectory(zipFilePath, Configuration.Default.InstallPath);

            nProgress = 40;
            System.IO.File.Delete(zipFilePath);

            Directory.CreateDirectory(Configuration.Default.CryptoPath);
            try
            {
                System.IO.File.Move(Configuration.Default.InstallPath + "\\crypto\\vk.key", Configuration.Default.CryptoPath + "\\vk.key");
            }
            catch(Exception ex)
            {
            }
        }

        private void SetConfigure()
        {
            string[] arrLine = System.IO.File.ReadAllLines(Configuration.Default.InstallPath + "\\config.json");
            for(int i = 0; i < arrLine.Length; i ++)
            {
                if (arrLine[i].Contains("\"DataDirectoryPath\":"))
                {
                    string chainPath = "    \"DataDirectoryPath\": \"" + Configuration.Default.ChainPath + "\",";
                    chainPath = chainPath.Replace("\\", "/");
                    arrLine[i] = chainPath; 
                }

                if (arrLine[i].Contains("\"PkPath\":"))
                {
                    string chainPath = "    \"PkPath\": \"" + Configuration.Default.CryptoPath + "\\pk.key\",";
                    chainPath = chainPath.Replace("\\", "/");
                    arrLine[i] = chainPath;
                }

                if (arrLine[i].Contains("\"VkPath\":"))
                {
                    string chainPath = "    \"VkPath\": \"" + Configuration.Default.CryptoPath + "\\vk.key\"";
                    chainPath = chainPath.Replace("\\", "/");
                    arrLine[i] = chainPath;
                }
            }
            try
            {
                Directory.Delete(Configuration.Default.ChainPath, true);
            }
            catch(Exception ex)
            {

            }

            Directory.CreateDirectory(Configuration.Default.ChainPath);
            System.IO.File.WriteAllLines(Configuration.Default.InstallPath + "\\config.json", arrLine);
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

        private void AddShortcut()
        {
            string pathToExe = Configuration.Default.InstallPath + "\\QurasWallet.exe";
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            string appStartMenuPath = System.IO.Path.Combine(commonStartMenuPath, "Programs", "QurasDev");

            if (!Directory.Exists(appStartMenuPath))
                Directory.CreateDirectory(appStartMenuPath);

            string shortcutLocation = System.IO.Path.Combine(appStartMenuPath, "QurasWallet" + ".lnk");

            System.IO.File.Delete(shortcutLocation);

            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "Quras Wallet Application";
            shortcut.TargetPath = pathToExe;
            shortcut.Save();

            using (var fs = new FileStream(shortcutLocation, FileMode.Open, FileAccess.ReadWrite))
            {
                fs.Seek(21, SeekOrigin.Begin);
                fs.WriteByte(0x22);
            }
        }

        private void CreateUninstaller()
        {
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(
                         @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }
                try
                {
                    RegistryKey key = null;

                    try
                    {
                        string guidText = "Quras_dev";
                        key = parent.OpenSubKey(guidText, true) ??
                              parent.CreateSubKey(guidText);

                        if (key == null)
                        {
                            throw new Exception(String.Format("Unable to create uninstaller "));
                        }

                        Assembly asm = GetType().Assembly;
                        Version v = asm.GetName().Version;
                        string exe = Configuration.Default.InstallPath + "\\QurasUninstaller.exe";

                        key.SetValue("DisplayName", "QurasWallet");
                        key.SetValue("ApplicationVersion", v.ToString());
                        key.SetValue("Publisher", "QurasDev");
                        key.SetValue("DisplayIcon", exe);
                        key.SetValue("DisplayVersion", v.ToString(2));
                        key.SetValue("URLInfoAbout", "https://quras.io/");
                        key.SetValue("Contact", "tech@quras.io");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", exe);
                    }
                    finally
                    {
                        if (key != null)
                        {
                            key.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "An error occurred writing uninstall information to the registry.  The service is fully installed but can only be uninstalled manually through the command line.",
                        ex);
                }
            }
        }
    }
}
