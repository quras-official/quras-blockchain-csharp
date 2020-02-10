using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System;
using System.IO;
using System.Web;

using Quras;
using Quras.VM;
using Quras.Core;
using Quras.IO.Json;
using Quras.SmartContract;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using Quras_gui_wpf.Controls;
using System.Collections.Generic;
using Quras.Wallets;
using System.Security.Cryptography;
using Quras.Implementations.Wallets.EntityFramework;

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for UploadPage.xaml
    /// </summary>
    public partial class UploadPage : System.Windows.Controls.UserControl
    {
        private LANG iLang => Constant.GetLang();
        private static readonly Fixed8 net_fee = Fixed8.FromDecimal(0.001m);
        private Fixed8 currentFee = Fixed8.Zero;
        public List<FileVerifierItem> verifierList;
        private byte[] FileEncryptKey;

        private string uploadURL;

        public UploadPage()
        {
            verifierList = new List<FileVerifierItem>();
            InitializeComponent();
        }

        public void RefreshLanguage()
        {
            /*TxbHeader.Text = StringTable.GetInstance().GetString("STR_AAP_TITLE", iLang);
            TxbComment.Text = StringTable.GetInstance().GetString("STR_AAP_COMMENT", iLang);
            TxbRegAssetTitle.Text = StringTable.GetInstance().GetString("STR_AAP_REG_ASSET_TITLE", iLang);
            TxbRegComment1.Text = StringTable.GetInstance().GetString("STR_AAP_REG_COMMENT1", iLang);*/
            
        }

        private void InitInterface()
        {

        }

        public bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public void InitBorderFields()
        {
        }

        

        public string CheckFields()
        {
            string ret = "STR_SUCCESS";
            return ret;
        }

        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }

            return data;
        }

        private bool FileEncrypt(string inputFile, string password, bool overwrite = false)
        {
            //generate random salt
            byte[] salt = GenerateRandomSalt();

            //create output file name
            FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create);

            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            //Set Rijndael symmetric encryption algorithm
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;

            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            AES.Mode = CipherMode.CFB;

            fsCrypt.Write(salt, 0, salt.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            byte[] buffer = new byte[1048576];
            int read;

            try
            {
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                }

                // Close up
                fsIn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                cs.Close();
                fsCrypt.Close();
            }
            if (overwrite == true)
            {
                try
                {
                    File.Copy(inputFile + ".aes",inputFile, true);
                    File.Delete(inputFile + ".aes");
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        private void FileDecrypt(string inputFile, string outputFile, string password)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int read;
            byte[] buffer = new byte[1048576];

            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsOut.Write(buffer, 0, read);
                }
            }
            catch (CryptographicException ex_CryptographicException)
            {
                Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
            }
            finally
            {
                fsOut.Close();
                fsCrypt.Close();
            }
        }

        private void BtnLaunch_Click(object sender, RoutedEventArgs e)
        {
            if (TxbChooseFile.Text == "")
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_NO_FILE_SELECTED", iLang));
                return;
            }

            if (TxbFileDescription.Text == "")
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_NO_FILE_DESCRIPTION", iLang));
                return;
            }
            if (stackOutPuts.Children.Count == 0)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_NO_VERIFIERS", iLang));
                return;
            }

            byte[] privateKey = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(privateKey);
            }
            FileEncryptKey = privateKey;

            KeyPair Filekey = new KeyPair(FileEncryptKey);

            //TODO: File Encrypt
            bool bResult;
            bResult = FileEncrypt(TxbChooseFile.Text, Filekey.PublicKey.ToString());
            if (bResult == true)
            {
                for (int i = 0; i < verifierList.Count; i++)
                {
                    string verifierPass = Wallet.ToScriptHash(verifierList[i].TxbAddress.Text).ToString();
                    bResult = FileEncrypt(TxbChooseFile.Text+".aes", verifierPass, true);
                    if (bResult == false)
                        break;
                }
            }

            if (bResult == true)
            {
                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_ENCRYPT_SUCCEED", iLang));
            }
            else
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ENCRYPT_FAILED", iLang));
                return;
            }

            //END: File Encrypt
            //=================
            //TODO: File Decrypt

            /*string prevPath = TxbChooseFile.Text + ".aes";
            for (int i = verifierList.Count - 1; i >= 0 ; i --)
            {
                string verifierPass = Wallet.ToScriptHash(verifierList[i].TxbAddress.Text).ToString();
                FileDecrypt(prevPath, prevPath += ".0", verifierPass);
            }
            FileDecrypt(prevPath, prevPath + ".0", FileEncryptKey.ToHexString());
            prevPath += ".0";*/

            //END: File Decrypt
            //=================
            //TODO: File Upload

            if (UploadFile(TxbChooseFile.Text + ".aes") == true)
            {
                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_UPLOAD_SUCCEED", iLang));
            }
            else
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_UPLOAD_FAILED", iLang));
                return;
            }

            
            //END: File upload

            //TODO: UploadRequestTransaction

            UInt160 walletAddrHash = UInt160.Zero;
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                if (Wallet.GetAddressVersion(Wallet.ToAddress(scriptHash)) == Wallet.AddressVersion)
                    walletAddrHash = scriptHash;
            }

            List<Quras.Cryptography.ECC.ECPoint> verifiers = new List<Quras.Cryptography.ECC.ECPoint>();

            for (int i = 0; i < verifierList.Count; i++)
            {
                UInt256 hashKey1 = new UInt256(Quras.Cryptography.Crypto.Default.Hash256(walletAddrHash.ToArray()));
                UInt256 hashKey2 = new UInt256(Quras.Cryptography.Crypto.Default.Hash256(Wallet.ToScriptHash(verifierList[i].TxbAddress.Text).ToArray()));
                verifiers.Add(UploadRequestTransaction.Encrypt_Verifier(hashKey1, hashKey2));
            }

            UploadRequestTransaction finalTx = Constant.CurrentWallet.MakeTransaction(new UploadRequestTransaction
            {
                Version = 1,
                FileName = TxbChooseFile.Text,
                FileDescription = TxbFileDescription.Text,
                FileURL = uploadURL,
                PayAmount = Fixed8.Parse(TxbPayAmount.Text),
                FileVerifiers = verifiers.ToArray(),
                Attributes = new TransactionAttribute[0],
                uploadHash = walletAddrHash,
                Inputs = new CoinReference[0],
                Outputs = new TransactionOutput[0]
            });

            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                if (Wallet.GetAddressVersion(Wallet.ToAddress(scriptHash)) == Wallet.AddressVersion)
                {
                    walletAddrHash = scriptHash;

                    KeyPair key = (KeyPair)Constant.CurrentWallet.GetKeyByScriptHash(walletAddrHash);
                    UInt160 pubkey = key.PublicKeyHash;
                    privateKey = key.PrivateKey;

                    finalTx.SetEncryptKey(privateKey, FileEncryptKey);
                    break;
                }
            }

            if (finalTx == null)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_SP_SEDDING_FAILED", iLang));
                return;
            }
            Global.Helper.SignAndShowInformation(finalTx);

            StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TX_SUCCESSED", iLang));
        }

        public InvocationTransaction GetTransaction()
        {
            return null;
        }

        


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitInterface();
            RefreshLanguage();
        }

        private bool UploadFile(string filePath)
        {
            try
            {
                var wc = new WebClient();
                byte[] response_binary = wc.UploadFile("http://13.112.100.149/fUpload/upload.php", "POST", filePath);
                string response = System.Text.Encoding.UTF8.GetString(response_binary);
                dynamic stuff = JsonConvert.DeserializeObject(response);
                string url = stuff[0];
                string compare_url = "http://13.112.100.149/fUpload";
                uploadURL = url;
                return url.Substring(0, compare_url.Length) == compare_url;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = "*";
            dlg.Filter = "Choose Files to upload (*.*)|*.*";
            dlg.AddExtension = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                TxbChooseFile.Text = dlg.FileName;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TxbAddressToAdd.Text == "")
            {
                return;
            }
            FileVerifierItem item = new FileVerifierItem();
            item.TxbAddress.Text = TxbAddressToAdd.Text;
            verifierList.Add(item);
            stackOutPuts.Children.Add(item);
        }
    }
}
