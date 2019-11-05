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
using System.Security.Cryptography;

using Pure;
using Pure.Core;
using Pure.Wallets;

using Quras_Test.Structure;
using Quras_Test.Global;
using Quras_Test.Controls;

namespace Quras_Test.Pages
{
    /// <summary>
    /// Interaction logic for AccountsPage.xaml
    /// </summary>
    public partial class AccountsPage : UserControl
    {
        List<AddressItem> lstAddrs;
        public AccountsPage()
        {
            InitializeComponent();
            InitInstance();
        }

        public void InitInstance()
        {
            txbCount.Text = "1";
            lstAddrs = new List<AddressItem>();
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            if (chkAnonymous.IsChecked == false)
            {
                byte[] privateKey = new byte[32];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(privateKey);
                }
                txbPrivKey.Text = privateKey.ToHexString();
            }
            else
            {
                //SpendingKey spendingKey = SpendingKey.random();
                //KeyPair key = CreateKey(spendingKey.ToArray(), nVersion);
                //return key;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txbPrivKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbPrivKey.Text.Length == 64)
            {
                byte[] privKey = txbPrivKey.Text.HexToBytes();
                KeyPair key = new KeyPair(privKey, KeyType.Transparent);

                txbAddress.Text = Wallet.ToAddress(key.PublicKeyHash);
            }
        }

        private void btnGenMulti_Click(object sender, RoutedEventArgs e)
        {
            int count = 1;
            try
            {
                count = int.Parse(txbCount.Text);
            } catch (Exception)
            {
                count = 1;
            }

            for (int i = 0; i < count; i++)
            {
                byte[] privateKey = new byte[32];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(privateKey);
                }
                KeyPair key = new KeyPair(privateKey, KeyType.Transparent);
                string address = Wallet.ToAddress(key.PublicKeyHash);

                TAddrs.GetInstance().SaveAddress(privateKey, address);

                AddressItem item = new AddressItem(address, privateKey.ToHexString());
                lstAddrs.Add(item);

                item.DeleteAddressEvent += DeleteAddrEvent;
                spAddrsPan.Children.Add(item);
            }
        }

        public void ReloadAddrs()
        {
            spAddrsPan.Children.Clear();

            foreach(var item in lstAddrs)
            {
                spAddrsPan.Children.Add(item);
            }
        }

        private void DeleteAddrEvent(object sender, AddressItem item)
        {
            item.DeleteAddressEvent -= DeleteAddrEvent;

            lstAddrs.Remove(item);
            TAddrs.GetInstance().RemoveAddress(item.GetAddress());

            ReloadAddrs();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            spAddrsPan.Children.Clear();

            foreach(TAddressType addrInfo in TAddrs.GetInstance().GetAddress())
            {
                AddressItem item = new AddressItem(addrInfo.address, addrInfo.privKey);
                lstAddrs.Add(item);
                item.DeleteAddressEvent += DeleteAddrEvent;
                spAddrsPan.Children.Add(item);
            }
        }
    }
}
