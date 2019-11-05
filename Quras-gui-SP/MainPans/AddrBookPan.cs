using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Quras_gui_SP.Dialogs;
using Quras_gui_SP.Controls;

namespace Quras_gui_SP.MainPans
{
    public partial class AddrBookPan : UserControl
    {
        public event EventHandler AddAddressBookEvent;
        public event EventHandler EditAddressBookEvent;
        public event EventHandler DeleteAddressBookEvent;

        public List<AddrItem> AddrbookItems;

        public AddrBookPan()
        {
            InitializeComponent();
            InitInstance();
            InitInterface();
        }

        public void InitInstance()
        {
            AddrbookItems = new List<AddrItem>();
            vsb_addrbook.Hide();

            pan_addrbook.MouseWheel += (s, e) => {
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
                if (vsb_addrbook.Value > 10)
                {
                    vsb_addrbook.Value = vsb_addrbook.Value - 10;
                }
                else
                {
                    vsb_addrbook.Value = 0;
                }
            }
            else
            {
                if (vsb_addrbook.Value < vsb_addrbook.Maximum - 10)
                {
                    vsb_addrbook.Value = vsb_addrbook.Value + 10;
                }
                else
                {
                    vsb_addrbook.Value = vsb_addrbook.Maximum;
                }
            }
            
        }

        public void RemoveContact(string contact_name, string address)
        {
            for (int i = 0; i < AddrbookItems.Count; i++)
            {
                AddrbookItems[i].AddrEditEvent -= Event_AddrEdit;
                AddrbookItems[i].AddrDeleteEvent -= Event_AddrDelete;
                pan_addrbook.Controls.Remove(AddrbookItems[i]);
            }

            bool isRemove = false;
            for (int i = 0; i < AddrbookItems.Count; i++)
            {
                if (AddrbookItems[i].Address == address)
                {
                    isRemove = true;
                    AddrbookItems.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < AddrbookItems.Count; i ++)
            {
                AddrItem item = AddrbookItems[i];

                item.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
                item.Location = new System.Drawing.Point(26, 16 + i * 80);
                item.Name = "addrItem" + i;
                item.Size = new System.Drawing.Size(518, 70);
                item.TabIndex = 100 + i;

                this.pan_addrbook.Controls.Add(item);

                if (AddrbookItems.Count * 80 + 16 > pan_addrbook.Height)
                {
                    vsb_addrbook.Maximum = i * 80 + 16 - pan_addrbook.Height + 16;
                    vsb_addrbook.Show();
                }

                item.AddrEditEvent += Event_AddrEdit;
                item.AddrDeleteEvent += Event_AddrDelete;
            }

            if (AddrbookItems.Count > 0)
            {
                lbl_no_history.Hide();
            }
            else
            {
                lbl_no_history.Show();
            }
        }

        public void EditContact(string old_addr, string contact_name, string address)
        {
            for (int i = 0; i < AddrbookItems.Count; i++)
            {
                if (AddrbookItems[i].Address == old_addr)
                {
                    AddrbookItems[i].RefreshItem(contact_name, address);
                    break;
                }
            }

            
            if (AddrbookItems.Count > 0)
            {
                lbl_no_history.Hide();
            }
            else
            {
                lbl_no_history.Show();
            }
        }

        public void AddContact(string contact_name, string address)
        {
            bool isAdd = true;
            for (int i = 0; i < AddrbookItems.Count; i++)
            {
                if (AddrbookItems[i].Address == address)
                {
                    isAdd = false;
                    break;
                }
            }

            if (isAdd)
            {

                AddrItem item = new AddrItem(contact_name, address);

                item.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
                item.Location = new System.Drawing.Point(26, 16 + AddrbookItems.Count * 80);
                item.Name = "addrItem1";
                item.Size = new System.Drawing.Size(518, 70);
                item.TabIndex = 3;

                this.pan_addrbook.Controls.Add(item);

                AddrbookItems.Add(item);

                if (AddrbookItems.Count * 80 + 16 > pan_addrbook.Height)
                {
                    vsb_addrbook.Maximum = AddrbookItems.Count * 80 + 16 - pan_addrbook.Height + 16;
                    vsb_addrbook.Show();
                }

                item.AddrEditEvent += Event_AddrEdit;
                item.AddrDeleteEvent += Event_AddrDelete;
            }

            if (AddrbookItems.Count > 0)
            {
                lbl_no_history.Hide();
            }
            else
            {
                lbl_no_history.Show();
            }
        }

        private void Event_AddrEdit(object sender, EventArgs e)
        {
            EditAddressBookEvent?.Invoke(sender, e);
        }

        private void Event_AddrDelete(object sender, EventArgs e)
        {
            DeleteAddressBookEvent?.Invoke(sender, e);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            using (AddAddrDlg dialog = new AddAddrDlg())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string[] param = new string[2];

                    param[0] = dialog.GetName();
                    param[1] = dialog.GetAddress();

                    AddAddressBookEvent?.Invoke(param, e);
                    return;
                }
            }
        }

        private void vsb_addrbook_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < AddrbookItems.Count; i++)
            {
                AddrbookItems[i].Top = 16 + 80 * i - vsb_addrbook.Value;
            }
        }
    }
}
