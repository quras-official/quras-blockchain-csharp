using System;

namespace Quras_gui_wpf.Dialogs.NotifyMessage
{
    public class NotifyMessage
    {
        private readonly System.Windows.Media.Brush skinBrush;
        private readonly string bodyText;
        private readonly Action clickAction;

        public NotifyMessage(System.Windows.Media.Brush skinBrush, string bodyText, Action clickAction)
        {
            this.skinBrush = skinBrush;
            this.bodyText = bodyText;
            this.clickAction = clickAction;
        }

        public System.Windows.Media.Brush SkinBrush
        {
            get
            {
                return skinBrush;
            }
        }

        public string BodyText
        {
            get
            {
                return bodyText;
            }
        }

        public Action OnClick
        {
            get
            {
                return clickAction;
            }
        }
    }
}
