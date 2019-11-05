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

using Pure.SmartContract;

using Quras_gui_wpf.Controls;

namespace Quras_gui_wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for SmartContractParamsDialog.xaml
    /// </summary>
    public partial class SmartContractParamsDialog : Window, IDisposable
    {
        private ContractParameter[] parameters;
        public SmartContractParamsDialog()
        {
            InitializeComponent();
            InitInterface();
        }

        public SmartContractParamsDialog(Window parent, ContractParameter[] parameters)
        {
            Owner = parent;
            InitializeComponent();
            this.parameters = parameters;
            InitInterface();
        }

        public void InitInterface()
        {
            stackParameters.Children.Clear();
            if (parameters == null)
            {
                return;
            }

            foreach(ContractParameter param in parameters)
            {
                SCParam1 paramItem = new SCParam1(param);
                stackParameters.Children.Add(paramItem);
            }
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            foreach(SCParam1 item in stackParameters.Children)
            {
                parameters[index] = item.GetParameter();
                index++;
            }
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
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
    }
}
