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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quras_Test.Controls
{
    /// <summary>
    /// Interaction logic for DashboardCard.xaml
    /// </summary>
    public partial class DashboardCard : UserControl
    {
        public DashboardCard()
        {
            InitializeComponent();
        }

        private void grdBackground_MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard sb = this.FindResource("MouseOver") as Storyboard;
            sb.Begin();
        }

        public void SetTitle(string title)
        {
            TxbDashCardTitle.Text = title;
        }

        public void SetStartTime(string time)
        {
            TxbStartedTime.Text = "Start Time : " + time;
        }

        public void SetEndTime(string time)
        {
            TxbEndTime.Text = "End Time : " + time;
        }

        public void SetLastUpdateTime(string time)
        {
            TxbLastEndedTime.Text = "Last ended time : " + time;
        }
    }
}
