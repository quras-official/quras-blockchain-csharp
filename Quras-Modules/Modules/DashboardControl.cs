using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DT_GUI_Modules.Modules
{
    public struct AddrStruct
    {
        public string Address;
        public string AmountQRS;
        public string AmountUSD;
    }

    public partial class DashboardControl : UserControl
    {
        public List<Controls.QurasItems.DashBoardItem> addrItem;

        public DashboardControl()
        {
            InitializeComponent();
            addrItem = new List<DT_GUI_Modules.Controls.QurasItems.DashBoardItem>();
        }

        public void AddAddress(AddrStruct addrInfo)
        {
            DT_GUI_Modules.Controls.QurasItems.DashBoardItem item = new DT_GUI_Modules.Controls.QurasItems.DashBoardItem(addrInfo.Address, addrInfo.AmountQRS, addrInfo.AmountUSD);

            item.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            //item.BackColor = System.Drawing.Color.Transparent;
            item.Cursor = System.Windows.Forms.Cursors.Arrow;
            item.Location = new System.Drawing.Point(34, 16 + 126 * addrItem.Count);
            item.Name = addrInfo.Address + "item";
            item.Size = new System.Drawing.Size(395, 116);
            item.TabIndex = 0;

            this.pan_flow.Controls.Add(item);

            addrItem.Add(item);
        }
    }
}
