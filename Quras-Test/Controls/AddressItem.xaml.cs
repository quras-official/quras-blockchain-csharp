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

using Pure;
namespace Quras_Test.Controls
{
    /// <summary>
    /// Interaction logic for AddressItem.xaml
    /// </summary>
    public partial class AddressItem : UserControl
    {
        private string address;
        private string privKey;
        private string qrgBalance;
        private string qrsBalance;

        public event EventHandler<AddressItem> DeleteAddressEvent;

        public AddressItem()
        {
            InitializeComponent();
            InitInterface();
        }

        public AddressItem(string address, string privKey, string qrsBalance = "0", string qrgBalance = "0")
        {
            InitializeComponent();

            this.address = address;
            this.privKey = privKey;
            this.qrsBalance = qrsBalance;
            this.qrgBalance = qrgBalance;

            InitInterface();
        }

        private void InitInterface()
        {
            TxbAddress.Text = address;
            TxbBalance.Text = $"XQC : {qrsBalance}   XQG : {qrgBalance}";
        }

        public string GetAddress()
        {
            return address;
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(privKey);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Cannot copy private key.");
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot copy private key.");
                return;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteAddressEvent?.Invoke(sender, this);
        }
    }
}
