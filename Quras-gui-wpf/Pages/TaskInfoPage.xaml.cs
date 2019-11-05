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

using Quras_gui_wpf.Global;
using Quras_gui_wpf.Controls;

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for TaskInfoPage.xaml
    /// </summary>
    public partial class TaskInfoPage : UserControl
    {
        public TaskInfoPage()
        {
            InitializeComponent();
            InitInstance();
        }

        public void InitInstance()
        {
            spAssetInfoPan.Children.Clear();
        }

        public void AddTask(TaskMessage message)
        {
            TaskItem item = new TaskItem(message);
            spAssetInfoPan.Children.Add(item);
        }
    }
}
