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
namespace Quras_gui_SP.MainPans
{
    public partial class TransactionsPan : UserControl
    {
        public List<TxItem> txItems;

        public TransactionsPan()
        {
            InitializeComponent();

            InitInstance();
            InitInterface();
        }

        public void Reset()
        {
            bool ret = false;
            int index = 0;
            while (ret == false)
            {
                if (this.pan_history.Controls.Count > 2)
                {
                    Control control = this.pan_history.Controls[index];

                    if (!(control == vsb_history | control == lbl_no_history))
                    {
                        this.pan_history.Controls.Remove(control);
                    }
                    else
                    {
                        index++;
                    }
                }
                else
                {
                    ret = true;
                }
            }

            this.txItems.Clear();
        }

        public void InitInstance()
        {
            txItems = new List<TxItem>();
            vsb_history.Hide();

            pan_history.MouseWheel += (s, e) => {
                HandleScroll(s, e);
            };
        }

        public void InitInterface()
        {
            lbl_no_history.Show();
        }

        private void HandleScroll(object sender, EventArgs e)
        {
            MouseEventArgs param = (MouseEventArgs)e;

            if (param.Delta > 0)
            {
                if (vsb_history.Value > 10)
                {
                    vsb_history.Value = vsb_history.Value - 10;
                }
                else
                {
                    vsb_history.Value = 0;
                }
            }
            else
            {
                if (vsb_history.Value < vsb_history.Maximum - 10)
                {
                    vsb_history.Value = vsb_history.Value + 10;
                }
                else
                {
                    vsb_history.Value = vsb_history.Maximum;
                }
            }

        }

        public void AddTransaction(TransactionInfo info)
        {
            bool isAdd = true;
            for (int i = 0; i < txItems.Count; i ++)
            {
                if (txItems[i]._info.Transaction.Hash == info.Transaction.Hash)
                {
                    isAdd = false;
                    break;
                }
            }

            if (isAdd)
            {
                TxItem item = new TxItem(info);

                item.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
                item.Location = new System.Drawing.Point(30, 16 + (txItems.Count) * (70 + 10));
                item.Name = "txItem" + info.Transaction.Hash.ToString();
                item.Size = new System.Drawing.Size(500, 70);
                item.TabIndex = 100;

                this.pan_history.Controls.Add(item);

                txItems.Add(item);

                if (txItems.Count * 80 + 16 > pan_history.Height)
                {
                    vsb_history.Maximum = txItems.Count * 80 + 16 - pan_history.Height + 16;
                    vsb_history.Show();
                }
            }

            if (txItems.Count > 0)
            {
                lbl_no_history.Hide();
            }
        }

        private void vsb_history_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < txItems.Count; i++)
            {
                txItems[i].Top = 16 + 80 * i - vsb_history.Value;
            }
        }
    }
}
