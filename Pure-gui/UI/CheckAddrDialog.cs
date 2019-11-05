using Pure.Wallets;
using System;
using System.Windows.Forms;
using System.Security.Cryptography;

using Pure.Cryptography;

namespace Pure.UI
{
    public partial class CheckAddrDialog : Form
    {
        public CheckAddrDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string privKey = textBox1.Text;
            byte[] byPrivKey = privKey.HexToBytes();

            KeyPair key = new KeyPair(byPrivKey);

            VerificationContract cc = new VerificationContract();
            cc.PublicKeyHash = key.PublicKeyHash;
            cc.Script = VerificationContract.CreateSignatureRedeemScript(key.PublicKey);
            textBox2.Text = Wallet.ToAddress(cc.ScriptHash);
            textBox3.Text = key.PublicKey.ToString();

            string sss = "aaaaaa";
            //byte[] aaa = key.PublicKey.EncodePoint(true);
            byte[] aaa = { 0x03, 0x68, 0x0f, 0x94, 0x1f, 0xc5, 0x6d, 0x46, 0x26, 0xb4, 0xc3, 0x1f, 0x10, 0xae, 0x7c, 0xcf, 0xed, 0x10, 0x58, 0x08, 0x08, 0x6a, 0x97, 0x16, 0x8e, 0x85, 0xc0, 0xbb, 0x47, 0x63, 0xa7, 0x4d, 0x91 };
            byte[] bb = aaa.Sha256();
        }
    }
}
