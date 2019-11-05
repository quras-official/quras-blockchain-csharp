using System;
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

using Quras_Test.Pages;

namespace Quras_Test
{
    public enum MainPageStauts
    {
        Dashboard,
        Accounts,
        History,
        Settings,
        Terminal,
        Stealth
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Main page instances
        private MainPageStauts CurrentPageStatus = MainPageStauts.Dashboard;

        private DashboardPage dashboardPage = new DashboardPage();
        private AccountsPage accountPage = new AccountsPage();
        private HistoryPage historyPage = new HistoryPage();
        private SettingsPage settingPage = new SettingsPage();
        private TerminalPage terminalPage = new TerminalPage();
        private StealthPage stealthPage = new StealthPage();
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            InitInstance();
        }

        private void InitInstance()
        {
            pageMains.ShowPage(dashboardPage);
        }

        private void GridMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void showPages()
        {
            switch(CurrentPageStatus)
            {
                case MainPageStauts.Dashboard:
                    {
                        pageMains.ShowPage(dashboardPage);
                        break;
                    }
                case MainPageStauts.Accounts:
                    {
                        pageMains.ShowPage(accountPage);
                        break;
                    }
                case MainPageStauts.History:
                    {
                        pageMains.ShowPage(historyPage);
                        break;
                    }
                case MainPageStauts.Settings:
                    {
                        pageMains.ShowPage(settingPage);
                        break;
                    }
                case MainPageStauts.Terminal:
                    {
                        pageMains.ShowPage(terminalPage);
                        break;
                    }
                case MainPageStauts.Stealth:
                    {
                        pageMains.ShowPage(stealthPage);
                        break;
                    }
                default:
                    break;
            }
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageStatus == MainPageStauts.Dashboard) return;

            CurrentPageStatus = MainPageStauts.Dashboard;
            grdMenuSelectedPan.Margin = new Thickness(0, 0, 0, 0);
            showPages();
        }

        private void btnAccounts_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageStatus == MainPageStauts.Accounts) return;

            CurrentPageStatus = MainPageStauts.Accounts;
            grdMenuSelectedPan.Margin = new Thickness(0, 40, 0, 0);
            showPages();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageStatus == MainPageStauts.Settings) return;

            CurrentPageStatus = MainPageStauts.Settings;
            grdMenuSelectedPan.Margin = new Thickness(0, 80, 0, 0);
            showPages();
        }

        private void btnTerminal_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageStatus == MainPageStauts.Terminal) return;

            CurrentPageStatus = MainPageStauts.Terminal;
            grdMenuSelectedPan.Margin = new Thickness(0, 160, 0, 0);
            showPages();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageStatus == MainPageStauts.History) return;

            CurrentPageStatus = MainPageStauts.History;
            grdMenuSelectedPan.Margin = new Thickness(0, 120, 0, 0);
            showPages();
        }

        private void btnStealth_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageStatus == MainPageStauts.Stealth) return;

            CurrentPageStatus = MainPageStauts.Stealth;
            grdMenuSelectedPan.Margin = new Thickness(0, 200, 0, 0);
            showPages();
        }
    }
}
