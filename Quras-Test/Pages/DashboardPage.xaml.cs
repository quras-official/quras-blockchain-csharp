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

namespace Quras_Test.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : UserControl
    {
        public DashboardPage()
        {
            InitializeComponent();
            InitInterface();
        }

        public void InitInterface()
        {
            dbcTransparentTest.SetTitle("Transparent Transaction Test");
            dbcAnonymousTokenTest.SetTitle("Anonymous Token Tx Test");
            dbcATATest.SetTitle("Anonymous to Anonymous Tx Test");
        }
    }
}
