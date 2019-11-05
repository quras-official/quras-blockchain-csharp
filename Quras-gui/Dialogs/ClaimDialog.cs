using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using MaterialSkin;
using MaterialSkin.Controls;

using Pure;
using Pure.Core;
using Quras_gui.Global;

namespace Quras_gui.Dialogs
{
    public partial class ClaimDialog : MaterialForm
    {
        public ClaimDialog()
        {
            InitializeComponent();

            // Create a material theme manager and add the form to manage (this)
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Configure color schema
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue700, Primary.Blue700,
                Primary.Blue700, Accent.LightBlue200,
                TextShade.WHITE
            );

            InitInterface();
        }

        private void InitInterface()
        {

        }

        private void ClaimDialog_Load(object sender, EventArgs e)
        {
            Fixed8 bonus_available = Blockchain.CalculateBonus(Constant.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference));
            txb_available.Text = bonus_available.ToString();
            if (bonus_available == Fixed8.Zero) btn_claim.Enabled = false;
            CalculateBonusUnavailable(Blockchain.Default.Height + 1);
            Blockchain.PersistCompleted += Blockchain_PersistCompleted;
        }

        private void Blockchain_PersistCompleted(object sender, Block block)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, Block>(Blockchain_PersistCompleted), sender, block);
            }
            else
            {
                CalculateBonusUnavailable(block.Index + 1);
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

            txb_max.Text = Blockchain.CalculateBonus(references, height).ToString();
        }

        private void ClaimDialog_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Blockchain.PersistCompleted -= Blockchain_PersistCompleted;
        }

        private void btn_claim_Click(object sender, EventArgs e)
        {
            CoinReference[] claims = Constant.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference).ToArray();
            if (claims.Length == 0) return;
            Quras_gui.FormUI.Helper.SignAndShowInformation(new ClaimTransaction
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
            Close();
        }
    }
}
