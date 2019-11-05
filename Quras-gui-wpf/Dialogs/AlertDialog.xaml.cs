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
using System.Windows.Shapes;

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;

namespace Quras_gui_wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for AlertDialog.xaml
    /// </summary>
    public partial class AlertDialog : Window, System.IDisposable
    {
        private LANG iLang => Constant.GetLang();
        public AlertDialog()
        {
            InitializeComponent();
            InitInterface(null, null);
        }

        public AlertDialog(Window parent, string title = null, string body = null)
        {
            Owner = parent;
            InitializeComponent();
            InitInterface(title, body);
        }
        
        private void InitInterface(string title, string body)
        {
            if (title != null)
            {
                TxbAlertHeader.Text = title;
            }

            if (body != null)
            {
                TxbAlertBody.Text = body;
            }

            btnYes.Content = StringTable.GetInstance().GetString("STR_BUTTON_YES", iLang);
            btnNo.Content = StringTable.GetInstance().GetString("STR_BUTTON_NO", iLang);
        }
        public void Dispose()
        {
            
        }

        private void GridMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
