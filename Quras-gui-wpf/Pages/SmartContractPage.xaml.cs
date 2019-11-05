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

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;
using Pure.Wallets;
using Pure;

namespace Quras_gui_wpf.Pages
{
    public enum SmartContractPageStatus
    {
        Invoke,
        Deploy,
        AddAsset,
        Other
    }

    /// <summary>
    /// Interaction logic for SmartContractPage.xaml
    /// </summary>
    public partial class SmartContractPage : UserControl
    {
        private SmartContractPageStatus pageStatus;

        private InvokeSmartContractPage invokeSCPage;
        private DeploySmartContractPage deploySCPage;
        private AddAssetsPage addAssetsPage;

        private LANG iLang => Constant.GetLang();

        public SmartContractPage()
        {
            InitializeComponent();

            InitInstance();
            RefreshLanguage();
        }

        private void InitInstance()
        {
            invokeSCPage = new InvokeSmartContractPage();
            deploySCPage = new DeploySmartContractPage();
            addAssetsPage = new AddAssetsPage();

            pageStatus = SmartContractPageStatus.Invoke;
            ShowPages(pageStatus);
        }

        public void RefreshLanguage()
        {
            btnDeploySmartContract.Content = StringTable.GetInstance().GetString("STR_SMARTCONTRACT_DEPLOY_BUTTON", iLang);
            btnInvokeSmartContract.Content = StringTable.GetInstance().GetString("STR_SMARTCONTRACT_INVOKE_BUTTON", iLang);
            btnAddAssets.Content = StringTable.GetInstance().GetString("STR_SMARTCONTRACT_ADD_ASSET_BUTTON", iLang);

            invokeSCPage.RefreshLanguage();
            deploySCPage.RefreshLanguage();
            addAssetsPage.RefreshLanguage();

            ShowPages(pageStatus);
        }

        private void ShowPages(SmartContractPageStatus status)
        {
            switch (status)
            {
                case SmartContractPageStatus.Invoke:
                    btnInvokeSmartContract.IsChecked = true;
                    btnDeploySmartContract.IsChecked = false;
                    btnAddAssets.IsChecked = false;
                    
                    if (iLang == LANG.EN)
                    {
                        btnInvokeSmartContract.FontWeight = FontWeights.Bold;
                        btnDeploySmartContract.FontWeight = FontWeights.Normal;
                        btnAddAssets.FontWeight = FontWeights.Normal;
                    }
                    else
                    {
                        btnInvokeSmartContract.FontWeight = FontWeights.Heavy;
                        btnDeploySmartContract.FontWeight = FontWeights.Normal;
                        btnAddAssets.FontWeight = FontWeights.Normal;
                    }
                    
                    panISC.Visibility = Visibility.Hidden;
                    panDSC.Visibility = Visibility.Hidden;
                    panAS.Visibility = Visibility.Hidden;
                    panOthers.Visibility = Visibility.Hidden;
                    panPrev.Visibility = Visibility.Hidden;

                    pageSmartContract.ShowPage(invokeSCPage);
                    break;
                case SmartContractPageStatus.Deploy:
                    btnInvokeSmartContract.IsChecked = false;
                    btnDeploySmartContract.IsChecked = true;
                    btnAddAssets.IsChecked = false;

                    if (iLang == LANG.EN)
                    {
                        btnInvokeSmartContract.FontWeight = FontWeights.Normal;
                        btnDeploySmartContract.FontWeight = FontWeights.Bold;
                        btnAddAssets.FontWeight = FontWeights.Normal;
                    }
                    else
                    {
                        btnInvokeSmartContract.FontWeight = FontWeights.Normal;
                        btnDeploySmartContract.FontWeight = FontWeights.Heavy;
                        btnAddAssets.FontWeight = FontWeights.Normal;
                    }
                    
                    panISC.Visibility = Visibility.Hidden;
                    panDSC.Visibility = Visibility.Hidden;
                    panAS.Visibility = Visibility.Hidden;
                    panOthers.Visibility = Visibility.Hidden;
                    panPrev.Visibility = Visibility.Hidden;

                    pageSmartContract.ShowPage(deploySCPage);
                    break;
                case SmartContractPageStatus.AddAsset:
                    btnInvokeSmartContract.IsChecked = false;
                    btnDeploySmartContract.IsChecked = false;
                    btnAddAssets.IsChecked = true;

                    if (iLang == LANG.EN)
                    {
                        btnInvokeSmartContract.FontWeight = FontWeights.Normal;
                        btnDeploySmartContract.FontWeight = FontWeights.Normal;
                        btnAddAssets.FontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        btnInvokeSmartContract.FontWeight = FontWeights.Normal;
                        btnDeploySmartContract.FontWeight = FontWeights.Normal;
                        btnAddAssets.FontWeight = FontWeights.Heavy;
                    }

                    panISC.Visibility = Visibility.Hidden;
                    panDSC.Visibility = Visibility.Hidden;
                    panAS.Visibility = Visibility.Hidden;
                    panOthers.Visibility = Visibility.Hidden;
                    panPrev.Visibility = Visibility.Hidden;

                    pageSmartContract.ShowPage(addAssetsPage);
                    break;
            }

        }

        private void btnInvokeSmartContract_Click(object sender, RoutedEventArgs e)
        {
            btnInvokeSmartContract.IsChecked = true;
            if (pageStatus == SmartContractPageStatus.Invoke) return;

            pageStatus = SmartContractPageStatus.Invoke;
            ShowPages(pageStatus);
        }

        private void btnDeploySmartContract_Click(object sender, RoutedEventArgs e)
        {
            btnDeploySmartContract.IsChecked = true;
            if (pageStatus == SmartContractPageStatus.Deploy) return;

            pageStatus = SmartContractPageStatus.Deploy;
            ShowPages(pageStatus);
        }

        private void btnAddAssets_Click(object sender, RoutedEventArgs e)
        {
            btnAddAssets.IsChecked = true;
            if (pageStatus == SmartContractPageStatus.AddAsset) return;

            pageStatus = SmartContractPageStatus.AddAsset;
            ShowPages(pageStatus);
        }

        public void hideAddAssetBtn()
        {
            btnAddAssets.Visibility = Visibility.Hidden;
        }
    }
}
