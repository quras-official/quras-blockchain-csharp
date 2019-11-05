using System;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

using Pure;
using Pure.Wallets;
using Pure.Implementations.Wallets.EntityFramework;
using PureCore.Wallets.AnonymousKey.Key;
using PureCore.Wallets.AnonymousKey.Note;

using Quras_gui.Properties;
using Quras_gui.Global;

namespace Quras_gui.FormUI
{
    public partial class NewWalletForm : Form
    {
        private int iLang => Constant.GetLang();
        public NewWalletForm()
        {
            InitializeComponent();
            InitInterface();
        }

        public void InitInterface()
        {
            lbl_warning.Hide();

            lbl_comment.Text = StringTable.DATA[iLang, 4];
            lbl_reciepent_address.Text = StringTable.DATA[iLang, 5];
            txb_wallet_path.WaterMark = StringTable.DATA[iLang, 6];
            lbl_password_cmt.Text = StringTable.DATA[iLang, 7];
            txb_password.WaterMark = StringTable.DATA[iLang, 8];
            txb_confirm_password.WaterMark = StringTable.DATA[iLang, 9];
            chk_anonymous.Text = StringTable.DATA[iLang, 10];
            btn_new_wallet_yes.Text = StringTable.DATA[iLang, 11];
            this.Text = StringTable.DATA[iLang, 17];
        }

        private void btn_new_wallet_yes_Click(object sender, EventArgs e)
        {
            if (!CheckParameter())
                return;

            UserWallet wallet;
            if (chk_anonymous.Checked == true)
            {
                wallet = UserWallet.Create(txb_wallet_path.Text, txb_password.Text, KeyType.Anonymous);
            }
            else
            {
                wallet = UserWallet.Create(txb_wallet_path.Text, txb_password.Text, KeyType.Transparent);
            }
            

            Settings.Default.LastWalletPath = txb_wallet_path.Text;
            Settings.Default.Save();

            using (MainWalletForm dialog = new MainWalletForm(wallet))
            {
                this.Hide();
                FormManager.GetInstance().Push(dialog);
                if (dialog.ShowDialog() != DialogResult.OK) return;
            }
            //Get hSig
            byte[] random_byte256 = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }
            UInt256 hSig = new UInt256(random_byte256);
            NoteEncryption noteEnc = new NoteEncryption(hSig);

            SpendingKey sk = SpendingKey.random();
            Console.Write("SpendingKey is " + sk.ToArray().ToHexString());
            Console.WriteLine();

            PaymentAddress addr = sk.address();
            PaymentAddress addr1 = sk.address();


            byte[] plain_text = { 0x74, 0x64, 0x64, 0x66, 0x23, 0x46, 0x54, 0x23, 0x32, 0x33, 0x33, 0x33, 0x33 };
            byte[] cipher_text = noteEnc.Encrypt(addr.pk_enc, plain_text);

            NoteDecryption noteDec = new NoteDecryption(sk.receiving_key());
            byte[] plain_text_m = noteDec.Decrypt(cipher_text, noteEnc.get_epk(), hSig, (char)0);
        }

        private bool CheckParameter()
        {
            if (txb_wallet_path.Text.Length == 0)
            {
                lbl_warning.Text = StringTable.DATA[iLang, 12];
                //lbl_warning.Top = txb_wallet_path.Top + 5;
                //lbl_warning.Left = btn_browser.Left + btn_browser.Width + 10;
                lbl_warning.Show();
                return false;
            }

            if (!Directory.Exists(Path.GetDirectoryName(txb_wallet_path.Text)))
            {
                lbl_warning.Text = StringTable.DATA[iLang, 13];
                //lbl_warning.Top = txb_wallet_path.Top + 5;
                //lbl_warning.Left = btn_browser.Left + btn_browser.Width + 10;
                lbl_warning.Show();
                return false;
            }

            if (txb_password.Text.Length == 0)
            {
                lbl_warning.Text = StringTable.DATA[iLang, 14];
                //lbl_warning.Top = txb_password.Top + 5;
                //lbl_warning.Left = txb_password.Left + txb_password.Width + 10;
                lbl_warning.Show();
                return false;
            }

            if (txb_password.Text != txb_confirm_password.Text)
            {
                lbl_warning.Text = StringTable.DATA[iLang, 15];
                //lbl_warning.Show();
                //lbl_warning.Top = txb_confirm_password.Top + 5;
                lbl_warning.Left = txb_confirm_password.Left + txb_confirm_password.Width + 10;
                return false;
            }

            lbl_warning.Hide();

            return true;
        }

        private void pan_background_Paint(object sender, PaintEventArgs e)
        {

        }

        private void NewWalletForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btn_browser_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.DefaultExt = "db3";
            dlg.Filter = "Data Files (*.db3)|*.db3";
            dlg.AddExtension = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txb_wallet_path.Text = dlg.FileName;
            }
        }
    }
}
