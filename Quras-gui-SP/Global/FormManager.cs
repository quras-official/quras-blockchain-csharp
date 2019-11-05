using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quras_gui_SP.Global
{
    public class FormManager
    {
        private static FormManager instance_;
        private List<Form> formList_;

        public static FormManager GetInstance()
        {
            if (instance_ == null)
            {
                instance_ = new FormManager();
            }

            return instance_;
        }

        public void Push(Form form)
        {
            if (formList_ == null)
            {
                formList_ = new List<Form>();
            }

            formList_.Add(form);
        }

        public Form Pop()
        {
            Form current = formList_[formList_.Count - 1];
            formList_.RemoveAt(formList_.Count - 1);
            return current;
        }
    }
}
