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

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;
using Quras.Wallets;
using Quras;
using Quras.Implementations.Wallets.EntityFramework;

namespace Quras_gui_wpf.Pages
{
    public enum FileDeliveryPageStatus
    {
        Upload,
        Download,
        Approve,
        Other
    }

    /// <summary>
    /// Interaction logic for FileDeliveryPage.xaml
    /// </summary>
    public partial class FileDeliveryPage : UserControl
    {

        private LANG iLang => Constant.GetLang();

        public FileDeliveryPage()
        {
            InitializeComponent();
        }
    }
}
