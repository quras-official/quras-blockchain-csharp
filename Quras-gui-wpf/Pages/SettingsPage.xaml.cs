using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;
using System.Xml.Linq;

using Pure;
using Pure.Core;
using Pure.Core.Anonoymous;

using Quras_gui_wpf.Windows;
using Quras_gui_wpf.Dialogs;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Properties;

namespace Quras_gui_wpf.Pages
{
    public enum KeyState
    {
        NotDownloaded,
        Downloading,
        Downloaded,
        Loading,
        Loaded
    }

    public enum UpdateState
    {
        OldVersion,
        Downloading,
        Downloaded,
        Updated,
        Failed,
    }

    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        private DispatcherTimer updateTimer = new DispatcherTimer();

        public event EventHandler ChangeLanguageEvent;
        public event EventHandler ResetHistoryEvent;

        private LANG iLang => Constant.GetLang();
        private int zkSnarksParamStatus;

        private readonly WebClient vkWeb = new WebClient();
        private readonly WebClient pkWeb = new WebClient();

        private readonly string vkKeyUrl = "http://13.230.62.42/quras/Keys/vk.key";
        private readonly string pkKeyUrl = "http://13.230.62.42/quras/Keys/pk.key";

        private static string download_path = System.IO.Directory.GetCurrentDirectory();

        private string vkPath = download_path + "\\crypto\\vk.key";
        private string pkPath = download_path + "\\crypto\\pk.key";

        private readonly WebClient updateWeb = new WebClient();
        private string updateDownloadUrl;
        private string updateDownloadPath;

        private KeyState vkKeyState;
        private KeyState pkKeyState;
        private KeyState zkModuleState;

        private UpdateState updateState;

        public event EventHandler<string> UpdateDownloadedFinished;
        public event EventHandler<TaskMessage> TaskChangedEvent;

        public SettingsPage()
        {
            InitializeComponent();
            InitInstance();
            RefreshLanguage();
        }

        private void InitInstance()
        {
            if (iLang == LANG.JP)
            {
                cmbLanguage.SelectedIndex = 1;
            }
            else if (iLang == LANG.EN)
            {
                cmbLanguage.SelectedIndex = 0;
            }

            InitializeZKSnarksParams();

            vkWeb.DownloadProgressChanged += vkDownloadProgressChanged;
            vkWeb.DownloadFileCompleted += vkDownloadFileCompleted;

            pkWeb.DownloadProgressChanged += pkDownloadProgressChanged;
            pkWeb.DownloadFileCompleted += pkDownloadFileCompleted;

            updateTimer.Tick += new EventHandler(UpdateTimer);
            updateTimer.Interval = new TimeSpan(1, 0, 0); // 1 hour
            updateTimer.Start();

            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load(Constant.UpdateUrl);
            }
            catch {

            }

            if (xdoc != null)
            {
                try
                {
                    Version latest = Version.Parse(xdoc.Element("update").Attribute("latest").Value);
                    XElement release = xdoc.Element("update").Elements("release").First(p => p.Attribute("version").Value == latest.ToString());
                    updateDownloadUrl = release.Attribute("file").Value;
                }
                catch (Exception ex)
                {
                    updateDownloadUrl = "";
                }
            }
            else
            {
                updateDownloadUrl = "";
            }

            updateWeb.DownloadProgressChanged += updateDownloadProgressChanged;
            updateWeb.DownloadFileCompleted += updateDownloadFileCompleted;

            UpdateUpdateButtonStatus();
        }

        private void ShowVkKeyState(KeyState state)
        {
            switch(state)
            {
                case KeyState.Downloaded:
                    TxbVerifyKeyStatusDownloaded.Visibility = Visibility.Visible;
                    TxbVerifyKeyStatusLoaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusNotDownloaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusDownloading.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusLoading.Visibility = Visibility.Hidden;
                    BdVerifyKeyStatus.Background = StaticUtils.BlueBrush;
                    btnVKDownload.IsEnabled = false;
                    break;
                case KeyState.Downloading:
                    TxbVerifyKeyStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusLoaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusNotDownloaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusLoading.Visibility = Visibility.Hidden;

                    TxbVerifyKeyStatusDownloading.Visibility = Visibility.Visible;
                    BdVerifyKeyStatus.Background = StaticUtils.BlueBrush;
                    btnVKDownload.IsEnabled = false;
                    break;
                case KeyState.NotDownloaded:
                    TxbVerifyKeyStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusLoaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusNotDownloaded.Visibility = Visibility.Visible;
                    TxbVerifyKeyStatusLoading.Visibility = Visibility.Hidden;

                    TxbVerifyKeyStatusDownloading.Visibility = Visibility.Hidden;
                    BdVerifyKeyStatus.Background = StaticUtils.ErrorBrush;

                    btnVKDownload.IsEnabled = true;
                    break;
                case KeyState.Loading:
                    TxbVerifyKeyStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusDownloading.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusNotDownloaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusLoaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusLoading.Visibility = Visibility.Visible;

                    BdVerifyKeyStatus.Background = StaticUtils.BlueBrush;

                    btnVKDownload.IsEnabled = false;
                    break;
                case KeyState.Loaded:
                    TxbVerifyKeyStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusDownloading.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusNotDownloaded.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusLoading.Visibility = Visibility.Hidden;
                    TxbVerifyKeyStatusLoaded.Visibility = Visibility.Visible;

                    BdVerifyKeyStatus.Background = StaticUtils.GreenBrush;

                    btnVKDownload.IsEnabled = false;
                    break;
            }
        }

        private void ShowPkKeyState(KeyState state)
        {
            switch (state)
            {
                case KeyState.Downloaded:
                    TxbPublicKeyStatusDownloading.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusDownloaded.Visibility = Visibility.Visible;
                    TxbPublicKeyStatusLoaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusNotDownloaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusLoading.Visibility = Visibility.Hidden;
                    BdPublicKeyStatus.Background = StaticUtils.BlueBrush;
                    btnPKDownload.IsEnabled = false;

                    break;
                case KeyState.Downloading:
                    TxbPublicKeyStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusLoaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusNotDownloaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusDownloading.Visibility = Visibility.Visible;
                    TxbPublicKeyStatusLoading.Visibility = Visibility.Hidden;
                    BdPublicKeyStatus.Background = StaticUtils.BlueBrush;

                    btnPKDownload.IsEnabled = false;
                    break;
                case KeyState.NotDownloaded:
                    TxbPublicKeyStatusDownloading.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusLoaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusNotDownloaded.Visibility = Visibility.Visible;
                    TxbPublicKeyStatusLoading.Visibility = Visibility.Hidden;

                    BdPublicKeyStatus.Background = StaticUtils.ErrorBrush;
                    btnPKDownload.IsEnabled = true;
                    break;
                case KeyState.Loading:
                    TxbPublicKeyStatusDownloading.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusLoaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusNotDownloaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusLoading.Visibility = Visibility.Visible;

                    BdPublicKeyStatus.Background = StaticUtils.BlueBrush;
                    btnPKDownload.IsEnabled = false;
                    break;
                case KeyState.Loaded:
                    TxbPublicKeyStatusDownloading.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusLoaded.Visibility = Visibility.Visible;
                    TxbPublicKeyStatusNotDownloaded.Visibility = Visibility.Hidden;
                    TxbPublicKeyStatusLoading.Visibility = Visibility.Hidden;

                    BdPublicKeyStatus.Background = StaticUtils.GreenBrush;
                    btnPKDownload.IsEnabled = false;
                    break;
            }
        }

        private void ShowZkModuleState(KeyState state)
        {
            switch (state)
            {
                case KeyState.Downloaded:
                    TxbZKModuleStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbZKModuleStatusLoaded.Visibility = Visibility.Hidden;
                    TxbZKModuleStatusNotLoaded.Visibility = Visibility.Visible;
                    TxbZKModuleStatusLoading.Visibility = Visibility.Hidden;

                    BdZKModuleStatus.Background = StaticUtils.BlueBrush;
                    btnZKModuleLoad.IsEnabled = true;
                    
                    break;
                case KeyState.Downloading:
                    break;
                case KeyState.NotDownloaded:
                    TxbZKModuleStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbZKModuleStatusLoaded.Visibility = Visibility.Hidden;
                    TxbZKModuleStatusNotLoaded.Visibility = Visibility.Visible;
                    TxbZKModuleStatusLoading.Visibility = Visibility.Hidden;

                    BdZKModuleStatus.Background = StaticUtils.BlueBrush;
                    btnZKModuleLoad.IsEnabled = false;
                    break;
                case KeyState.Loading:
                    TxbZKModuleStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbZKModuleStatusLoaded.Visibility = Visibility.Hidden;
                    TxbZKModuleStatusNotLoaded.Visibility = Visibility.Hidden;
                    TxbZKModuleStatusLoading.Visibility = Visibility.Visible;

                    BdZKModuleStatus.Background = StaticUtils.BlueBrush;
                    btnZKModuleLoad.IsEnabled = false;
                    break;
                case KeyState.Loaded:
                    TxbZKModuleStatusDownloaded.Visibility = Visibility.Hidden;
                    TxbZKModuleStatusLoaded.Visibility = Visibility.Visible;
                    TxbZKModuleStatusNotLoaded.Visibility = Visibility.Hidden;
                    TxbZKModuleStatusLoading.Visibility = Visibility.Hidden;

                    BdZKModuleStatus.Background = StaticUtils.GreenBrush;
                    btnZKModuleLoad.IsEnabled = false;
                    break;
            }
        }

        private void ShowUpdateStatus(UpdateState state)
        {
            switch (state)
            {
                case UpdateState.OldVersion:
                    TxbUpdateDownloaded.Visibility = Visibility.Hidden;
                    TxbUpdateDownloading.Visibility = Visibility.Hidden;
                    TxbUpdateFailed.Visibility = Visibility.Hidden;
                    TxbUpdateOld.Visibility = Visibility.Visible;
                    TxbUpdateUpdated.Visibility = Visibility.Hidden;

                    BdUpdateStatus.Background = StaticUtils.ErrorBrush;

                    btnUpdate.IsEnabled = true;
                    break;
                case UpdateState.Downloading:
                    TxbUpdateDownloaded.Visibility = Visibility.Hidden;
                    TxbUpdateDownloading.Visibility = Visibility.Visible;
                    TxbUpdateFailed.Visibility = Visibility.Hidden;
                    TxbUpdateOld.Visibility = Visibility.Hidden;
                    TxbUpdateUpdated.Visibility = Visibility.Hidden;

                    BdUpdateStatus.Background = StaticUtils.BlueBrush;

                    btnUpdate.IsEnabled = false;
                    break;
                case UpdateState.Downloaded:
                    TxbUpdateDownloaded.Visibility = Visibility.Visible;
                    TxbUpdateDownloading.Visibility = Visibility.Hidden;
                    TxbUpdateFailed.Visibility = Visibility.Hidden;
                    TxbUpdateOld.Visibility = Visibility.Hidden;
                    TxbUpdateUpdated.Visibility = Visibility.Hidden;

                    BdUpdateStatus.Background = StaticUtils.BlueBrush;

                    btnUpdate.IsEnabled = false;
                    break;
                case UpdateState.Updated:
                    TxbUpdateDownloaded.Visibility = Visibility.Hidden;
                    TxbUpdateDownloading.Visibility = Visibility.Hidden;
                    TxbUpdateFailed.Visibility = Visibility.Hidden;
                    TxbUpdateOld.Visibility = Visibility.Hidden;
                    TxbUpdateUpdated.Visibility = Visibility.Visible;

                    BdUpdateStatus.Background = StaticUtils.GreenBrush;

                    btnUpdate.IsEnabled = false;
                    break;
                case UpdateState.Failed:
                    TxbUpdateDownloaded.Visibility = Visibility.Hidden;
                    TxbUpdateDownloading.Visibility = Visibility.Hidden;
                    TxbUpdateFailed.Visibility = Visibility.Visible;
                    TxbUpdateOld.Visibility = Visibility.Hidden;
                    TxbUpdateUpdated.Visibility = Visibility.Hidden;

                    BdUpdateStatus.Background = StaticUtils.ErrorBrush;

                    btnUpdate.IsEnabled = true;
                    break;
            }
        }

        private void InitializeZKSnarksParams()
        {
            zkSnarksParamStatus = StaticUtils.CheckZKSnarksKeyStatus();

            switch (zkSnarksParamStatus)
            {
                case 0:
                    vkKeyState = KeyState.Downloaded;
                    pkKeyState = KeyState.Downloaded;
                    zkModuleState = KeyState.Downloaded;
                    break;
                case 1:
                    vkKeyState = KeyState.NotDownloaded;
                    pkKeyState = KeyState.Downloaded;
                    zkModuleState = KeyState.NotDownloaded;
                    break;
                case 2:
                    vkKeyState = KeyState.Downloaded;
                    pkKeyState = KeyState.NotDownloaded;
                    zkModuleState = KeyState.NotDownloaded;
                    break;
                case 3:
                    vkKeyState = KeyState.NotDownloaded;
                    pkKeyState = KeyState.NotDownloaded;
                    zkModuleState = KeyState.NotDownloaded;
                    break;
            }

            if (Constant.isLoadedVK)
            {
                vkKeyState = KeyState.Loaded;
            }
            ShowVkKeyState(vkKeyState);
            if (Constant.isLoadedPK)
            {
                pkKeyState = KeyState.Loaded;
            }
            ShowPkKeyState(pkKeyState);
            if (Constant.isLoadedPK && Constant.isLoadedVK)
            {
                zkModuleState = KeyState.Loaded;
            }
            ShowZkModuleState(zkModuleState);
        }

        public void RefreshLanguage()
        {
            TxbSettingsTitle.Text = StringTable.GetInstance().GetString("STR_SETTINGS_TITLE", iLang);
            TxbComment.Text = StringTable.GetInstance().GetString("STR_SETTINGS_TITLE_COMMENT", iLang);
            TxbLanguageTitle.Text = StringTable.GetInstance().GetString("STR_SETTINGS_LANGUAGE_TITLE", iLang);
            TxbLanguageComment.Text = StringTable.GetInstance().GetString("STR_SETTINGS_LANGUAGE_COMMENT", iLang);

            TxbZkSnarksTitle.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_TITLE", iLang);
            TxbZkSnarksComment.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_COMMENT", iLang);
            TxbVerifyKeyTitle.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_VK_TITLE", iLang);
            TxbVerifyKeyComment.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_VK_COMMENT", iLang);
            TxbPublicKeyTitle.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_PK_TITLE", iLang);
            TxbPublicKeyComment.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_PK_COMMENT", iLang);
            TxbZKModuleTitle.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_ZKMODULE_TITLE", iLang);
            TxbZKModuleComment.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_ZKMODULE_COMMENT", iLang);

            TxbVerifyKeyStatusDownloaded.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_VK_DOWNLOADED", iLang);
            TxbVerifyKeyStatusLoaded.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_VK_LOADED", iLang);
            TxbVerifyKeyStatusNotDownloaded.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_VK_NOT_DOWNLOADED", iLang);
            TxbVerifyKeyStatusDownloading.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_VK_DOWNLOADING", iLang);

            TxbPublicKeyStatusDownloaded.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_PK_DOWNLOADED", iLang);
            TxbPublicKeyStatusLoaded.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_PK_LOADED", iLang);
            TxbPublicKeyStatusNotDownloaded.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_PK_NOT_DOWNLOADED", iLang);
            TxbPublicKeyStatusDownloading.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_PK_DOWNLOADING", iLang);

            TxbZKModuleStatusDownloaded.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_ZKM_DOWNLOADED", iLang); 
            TxbZKModuleStatusLoaded.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_ZKM_LOADED", iLang);
            TxbZKModuleStatusNotLoaded.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_ZKM_NOT_LOADED", iLang);

            TxbVerifyKeyStatusLoading.Text = StringTable.GetInstance().GetString("STR_SETTINGS_KEYS_LOADING", iLang);
            TxbPublicKeyStatusLoading.Text = StringTable.GetInstance().GetString("STR_SETTINGS_KEYS_LOADING", iLang);
            TxbZKModuleStatusLoading.Text = StringTable.GetInstance().GetString("STR_SETTINGS_KEYS_LOADING", iLang);

            TxbUpdateComment.Text = String.Format(StringTable.GetInstance().GetString("STR_UPDATE_COMMENT", iLang), Constant.GetLocalWalletVersion().ToString(), Constant.GetNewestWalletVersionFromServer().ToString());

            TxbRepairTittle.Text = StringTable.GetInstance().GetString("STR_SETTINGS_REPAIR_TITLE", iLang);
            TxbRepairComment.Text = StringTable.GetInstance().GetString("STR_SETTINGS_REPAIR_COMMENT", iLang);

            btnVKDownload.Content = StringTable.GetInstance().GetString("STR_SETTINGS_BUTTON_DOWNLOAD", iLang);
            btnPKDownload.Content = StringTable.GetInstance().GetString("STR_SETTINGS_BUTTON_DOWNLOAD", iLang);
            btnZKModuleLoad.Content = StringTable.GetInstance().GetString("STR_SETTINGS_BUTTON_LOAD", iLang);
            btnUpdate.Content = StringTable.GetInstance().GetString("STR_SETTINGS_BUTTON_UPDATE", iLang);
            btnRepair.Content = StringTable.GetInstance().GetString("STR_SETTINGS_BUTTON_REPAIR", iLang);

            TxbUpdateTitle.Text = StringTable.GetInstance().GetString("STR_SETTINGS_UPDATE_TITLE", iLang);
            TxbUpdateDownloading.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_VK_DOWNLOADING", iLang);
            TxbUpdateDownloaded.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_PK_DOWNLOADED", iLang);
            TxbUpdateOld.Text = StringTable.GetInstance().GetString("STR_SETTINGS_STATUS_OLDVERSION", iLang);
            TxbUpdateUpdated.Text = StringTable.GetInstance().GetString("STR_SETTINGS_STATUS_UPDATED", iLang);
            TxbUpdateFailed.Text = StringTable.GetInstance().GetString("STR_SETTINGS_STATUS_FAILED", iLang);

            TxbClaimTitle.Text = StringTable.GetInstance().GetString("STR_SETTINGS_CLAIM_TITLE", iLang);
            btnClaim.Content = StringTable.GetInstance().GetString("STR_SETTINGS_BUTTON_CLAIM", iLang);
        }

        private void vkDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            TxbVerifyKeyStatusDownloading.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_VK_DOWNLOADING", iLang) + "(" + e.ProgressPercentage.ToString() + "%)";
        }

        private void vkDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                vkKeyState = KeyState.NotDownloaded;
                ShowVkKeyState(vkKeyState);

                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_VK_DOWNLOAD", iLang));
            }
            else
            {
                vkKeyState = KeyState.Downloaded;
                ShowVkKeyState(vkKeyState);

                if (pkKeyState == KeyState.Downloaded)
                {
                    zkModuleState = KeyState.Downloaded;
                    ShowZkModuleState(KeyState.Downloaded);
                }
                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUCCESS_VK_DOWNLOAD", iLang));
            }
        }

        private void pkDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            TxbPublicKeyStatusDownloading.Text = StringTable.GetInstance().GetString("STR_SETTINGS_ZKS_PK_DOWNLOADING", iLang) + "(" + e.ProgressPercentage.ToString() + "%)";
        }

        private void pkDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                pkKeyState = KeyState.NotDownloaded;
                ShowPkKeyState(pkKeyState);

                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_PK_DOWNLOAD", iLang));
            }
            else
            {
                pkKeyState = KeyState.Downloaded;
                ShowPkKeyState(pkKeyState);

                btnZKModuleLoad.IsEnabled = true;

                if (vkKeyState == KeyState.Downloaded)
                {
                    zkModuleState = KeyState.Downloaded;
                    ShowZkModuleState(KeyState.Downloaded);
                }
                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUCCESS_PK_DOWNLOAD", iLang));
            }
        }

        private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)((ComboBoxItem)cmbLanguage.SelectedItem).Content == "English")
            {
                Settings.Default.Language = "EN";
                Settings.Default.Save();
                RefreshLanguage();
                ChangeLanguageEvent?.Invoke(sender, e);
            }
            else if ((string)((ComboBoxItem)cmbLanguage.SelectedItem).Content == "日本語")
            {
                Settings.Default.Language = "JP";
                Settings.Default.Save();
                RefreshLanguage();
                ChangeLanguageEvent?.Invoke(sender, e);
            }
        }

        private void btnVKDownload_Click(object sender, RoutedEventArgs e)
        {
            vkKeyState = KeyState.Downloading;
            ShowVkKeyState(vkKeyState);

            
            vkWeb.DownloadFileAsync(new Uri(vkKeyUrl), vkPath);
        }

        private void btnZKModuleLoad_Click(object sender, RoutedEventArgs e)
        {
            btnZKModuleLoad.IsEnabled = false;

            string vkKeyPath = SettingsConfig.Default.VkKeyPath;
            string pkKeyPath = SettingsConfig.Default.PkKeyPath;

            vkKeyPath = System.IO.Path.GetFullPath(vkKeyPath);
            pkKeyPath = System.IO.Path.GetFullPath(pkKeyPath);

            if (StaticUtils.CheckZKSnarksKeyStatus() == 0)
            {
                Task.Run(() => {
                    int ret;
                    if (vkKeyState == KeyState.Loaded)
                    {
                        ret = 1;

                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            zkModuleState = KeyState.Loading;
                            ShowZkModuleState(zkModuleState);
                        }));
                    }
                    else
                    {
                        try
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                StaticUtils.ShowMessageBox(StaticUtils.BlueBrush, StringTable.GetInstance().GetString("STR_SUCCESS_LOADING_VERIFYKEY", iLang));

                                vkKeyState = KeyState.Loading;
                                ShowVkKeyState(vkKeyState);

                                zkModuleState = KeyState.Loading;
                                ShowZkModuleState(zkModuleState);
                            }));
                            ret = SnarkDllApi.Snark_DllInit(1, vkKeyPath.ToArray(), pkKeyPath.ToArray());
                        }
                        catch (Exception ex)
                        {
                            ret = -1;
                        }
                    }
                    
                    if (ret > 0)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUCCESS_LOADED_VERIFYKEY", iLang));

                            vkKeyState = KeyState.Loaded;
                            Constant.isLoadedVK = true;
                            ShowVkKeyState(vkKeyState);
                        }));

                        vkKeyPath = SettingsConfig.Default.VkKeyPath;
                        pkKeyPath = SettingsConfig.Default.PkKeyPath;

                        vkKeyPath = System.IO.Path.GetFullPath(vkKeyPath);
                        pkKeyPath = System.IO.Path.GetFullPath(pkKeyPath);

                        try
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                StaticUtils.ShowMessageBox(StaticUtils.BlueBrush, StringTable.GetInstance().GetString("STR_BEGIN_LOADING_PUBLICKEY", iLang));

                                pkKeyState = KeyState.Loading;
                                ShowPkKeyState(pkKeyState);

                                LoadKeyTaskMessage taskMessage = new LoadKeyTaskMessage(StringTable.GetInstance().GetString("STR_TASK_MESSAGE_PK_KEY_LOAD_START", iLang), DateTime.Now);
                                Constant.TaskMessages.Add(taskMessage);
                                TaskChangedEvent?.Invoke(this, taskMessage);
                            }));

                            ret = SnarkDllApi.Snark_DllInit(2, vkKeyPath.ToArray(), pkKeyPath.ToArray());
                        }
                        catch (Exception)
                        {
                            ret = -1;
                        }
                        if (ret > 0)
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUCCESS_LOADED_PUBLICKEY", iLang));

                                pkKeyState = KeyState.Loaded;
                                ShowPkKeyState(pkKeyState);
                                Constant.isLoadedPK = true;

                                zkModuleState = KeyState.Loaded;
                                ShowZkModuleState(zkModuleState);
                                Constant.bSnarksParamLoaded = true;

                                LoadKeyTaskMessage taskMessage = new LoadKeyTaskMessage(StringTable.GetInstance().GetString("STR_TASK_MESSAGE_PK_KEY_LOAD_FINISHED", iLang), DateTime.Now);
                                Constant.TaskMessages.Add(taskMessage);
                                TaskChangedEvent?.Invoke(this, taskMessage);
                                btnZKModuleLoad.IsEnabled = false;
                            }));
                        }
                        else
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_LOADING_PUBLICKEY", iLang));

                                pkKeyState = KeyState.Downloaded;
                                ShowPkKeyState(pkKeyState);

                                zkModuleState = KeyState.Downloaded;
                                ShowZkModuleState(zkModuleState);

                                LoadKeyTaskMessage taskMessage = new LoadKeyTaskMessage(StringTable.GetInstance().GetString("STR_TASK_MESSAGE_PK_KEY_LOAD_FAILED", iLang), DateTime.Now, TaskColor.Red);
                                Constant.TaskMessages.Add(taskMessage);
                                TaskChangedEvent?.Invoke(this, taskMessage);
                                btnZKModuleLoad.IsEnabled = true;
                            }));
                        }
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_ERR_LOADING_VERIFYKEY", iLang));

                            vkKeyState = KeyState.Downloaded;
                            ShowVkKeyState(vkKeyState);

                            zkModuleState = KeyState.Downloaded;
                            ShowZkModuleState(zkModuleState);

                            btnZKModuleLoad.IsEnabled = true;
                        }));
                    }
                });
            }
        }

        private void btnPKDownload_Click(object sender, RoutedEventArgs e)
        {
            pkKeyState = KeyState.Downloading;
            ShowPkKeyState(pkKeyState);

            pkWeb.DownloadFileAsync(new Uri(pkKeyUrl), pkPath);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool ret = false;
            using (AlertDialog dlg = new AlertDialog(Application.Current.MainWindow, StringTable.GetInstance().GetString("STR_ALERT_DLG_TITLE", iLang), StringTable.GetInstance().GetString("STR_ALERT_UPDATE_BODY", iLang)))
            {
                ret = (bool)dlg.ShowDialog();
            }

            if (ret)
            {
                XDocument xdoc = null;
                try
                {
                    xdoc = XDocument.Load(Constant.UpdateUrl);
                }
                catch
                {

                }

                if (xdoc != null)
                {
                    Version latest = Version.Parse(xdoc.Element("update").Attribute("latest").Value);
                    XElement release = xdoc.Element("update").Elements("release").First(p => p.Attribute("version").Value == latest.ToString());
                    updateDownloadUrl = release.Attribute("file").Value;

                    updateDownloadPath = "update.zip";
                    updateWeb.DownloadFileAsync(new Uri(updateDownloadUrl), updateDownloadPath);
                    updateState = UpdateState.Downloading;
                    ShowUpdateStatus(updateState);
                }
                else
                {
                    updateDownloadUrl = "";
                    updateState = UpdateState.Failed;
                    ShowUpdateStatus(updateState);
                }
            }
        }

        private void UpdateUpdateButtonStatus()
        {
            if (Constant.GetLocalWalletVersion() < Constant.GetNewestWalletVersionFromServer())
            {
                if (updateState != UpdateState.Downloading)
                {
                    updateState = UpdateState.OldVersion;
                }
            }
            else
            {
                updateState = UpdateState.Updated;
            }

            ShowUpdateStatus(updateState);
        }

        private void updateDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            TxbUpdateDownloading.Text = StringTable.GetInstance().GetString("STR_SETTING_UPDATE_DOWNLOADING", iLang) + "(" + e.ProgressPercentage.ToString() + "%)";
        }

        private void updateDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                updateState = UpdateState.Failed;
                ShowUpdateStatus(updateState);

                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_UPDATE_DOWNLOADING", iLang));
            }
            else
            {
                updateState = UpdateState.Downloaded;
                ShowUpdateStatus(updateState);

                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_UPDATE_DOWNLOADING", iLang));
                
                UpdateDownloadedFinished?.Invoke(sender, updateDownloadPath);
            }
        }

        private void UpdateTimer(object sender, EventArgs args)
        {
            TxbUpdateComment.Text = String.Format(StringTable.GetInstance().GetString("STR_UPDATE_COMMENT", iLang), Constant.GetLocalWalletVersion().ToString(), Constant.GetNewestWalletVersionFromServer().ToString());
            UpdateUpdateButtonStatus();
        }

        private void btnRepair_Click(object sender, RoutedEventArgs e)
        {
            bool ret = false;
            using (AlertDialog dlg = new AlertDialog(Application.Current.MainWindow, StringTable.GetInstance().GetString("STR_ALERT_DLG_TITLE", iLang), StringTable.GetInstance().GetString("STR_ALERT_REPAIR_WALLER", iLang)))
            {
                ret = (bool)dlg.ShowDialog();
            }

            if (ret)
            {
                AssetsManager.GetInstance().Reset();
                ResetHistoryEvent?.Invoke(sender, e);
                Constant.CurrentWallet.Rebuild();
                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_REPAIR_WALLET", iLang));
            }
        }

        private void CalculateBonusUnavailable(uint height)
        {
            var unspent = Constant.CurrentWallet.FindUnspentCoins()
                .Where(p => p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash))
                .Select(p => p.Reference)
                ;

            ICollection<CoinReference> references = new HashSet<CoinReference>();

            foreach (var group in unspent.GroupBy(p => p.PrevHash))
            {
                int height_start;
                Transaction tx = Blockchain.Default.GetTransaction(group.Key, out height_start);
                if (tx == null)
                    continue; // not enough of the chain available
                foreach (var reference in group)
                    references.Add(reference);
            }

            TxbClaimTotal.Text = String.Format(StringTable.GetInstance().GetString("STR_CLAIM_UNAVAILABLE", iLang), Blockchain.CalculateBonus(references, height).ToString());
        }

        private void Blockchain_PersistCompleted(object sender, Pure.Core.Block block)
        {
            if (Thread.CurrentThread != this.Dispatcher.Thread)
            {
                this.Dispatcher.BeginInvoke(new Action<object, Pure.Core.Block>(Blockchain_PersistCompleted), sender, block);
            }
            else
            {
                Fixed8 bonus_available = Blockchain.CalculateBonus(Constant.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference));
                CalculateBonusUnavailable(block.Index + 1);
                TxbClaimAvailable.Text = String.Format(StringTable.GetInstance().GetString("STR_CLAIM_AVAILABLE", iLang), bonus_available.ToString());
                
                if (bonus_available == Fixed8.Zero) btnClaim.IsEnabled = false;
                else btnClaim.IsEnabled = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Fixed8 bonus_available = Blockchain.CalculateBonus(Constant.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference));
            TxbClaimAvailable.Text = String.Format(StringTable.GetInstance().GetString("STR_CLAIM_AVAILABLE", iLang), bonus_available.ToString());
            if (bonus_available == Fixed8.Zero) btnClaim.IsEnabled = false;
            CalculateBonusUnavailable(Blockchain.Default.Height + 1);
            Blockchain.PersistCompleted += Blockchain_PersistCompleted;
        }

        private void SettingPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Blockchain.PersistCompleted -= Blockchain_PersistCompleted;
        }

        private void btnClaim_Click(object sender, RoutedEventArgs e)
        {
            CoinReference[] claims = Constant.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference).ToArray();
            if (claims.Length == 0) return;
            Quras_gui_wpf.Windows.Helper.SignAndShowInformation(new ClaimTransaction
            {
                Claims = claims,
                Attributes = new TransactionAttribute[0],
                Inputs = new CoinReference[0],
                Outputs = new[]
                {
                    new TransactionOutput
                    {
                        AssetId = Blockchain.UtilityToken.Hash,
                        Value = Blockchain.CalculateBonus(claims),
                        ScriptHash = Constant.CurrentWallet.GetChangeAddress()
                    }
                }
            });
        }
    }
}
