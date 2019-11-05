using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Pure.Implementations.Wallets.EntityFramework;
using Quras_gui_SP.Controls;

namespace Quras_gui_SP.RightSidePans
{
    public partial class TransactionRightSidePan : UserControl
    {
        public List<SideTxItem> txItems;
        public TransactionRightSidePan()
        {
            InitializeComponent();

            txItems = new List<SideTxItem>();
        }

        public void Reset()
        {
            bool ret = false;
            while (ret == false)
            {
                if (this.pan_side_tx.Controls.Count > 0)
                {
                    Control control = this.pan_side_tx.Controls[0];
                    this.pan_side_tx.Controls.Remove(control);
                }
                else
                {
                    ret = true;
                }
            }

            txItems.Clear();
        }

        public void AddTransaction(TransactionInfo info)
        {
            bool isAdd = true;
            for (int i = 0; i < txItems.Count; i++)
            {
                if (txItems[i]._info.Transaction.Hash == info.Transaction.Hash)
                {
                    isAdd = false;
                    break;
                }
            }

            if (isAdd)
            {
                SideTxItem item = new SideTxItem(info);

                txItems.Add(item);
            }
        }

        public void LayoutItems()
        {
            bool ret = false;
            while (ret == false)
            {
                if (this.pan_side_tx.Controls.Count > 0)
                {
                    Control control = this.pan_side_tx.Controls[0];
                    this.pan_side_tx.Controls.Remove(control);
                }
                else
                {
                    ret = true;
                }
            }

            int pos = 0;
            for (int i = txItems.Count - 1; i > txItems.Count - 9 && i >= 0; i --)
            {
                txItems[i].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(31)))), ((int)(((byte)(37)))));
                txItems[i].Location = new System.Drawing.Point(0, pos * txItems[i].Height);
                txItems[i].Name = "sideTxItem" + i.ToString();
                txItems[i].Size = new System.Drawing.Size(193, 65);
                txItems[i].TabIndex = 100 + i;

                this.pan_side_tx.Controls.Add(txItems[i]);

                pos++;
            }
        }
    }
}
