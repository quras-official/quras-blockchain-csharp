namespace Quras_gui_wpf.Dialogs.NotifyMessage
{
    public class AnimatedLocation
    {
        private readonly double fromLeft;
        private readonly double toLeft;
        private readonly double fromTop;
        private readonly double toTop;

        public double FromLeft
        {
            get { return fromLeft; }
        }

        public double ToLeft
        {
            get { return toLeft; }
        }

        public double FromTop
        {
            get { return fromTop; }
        }

        public double ToTop
        {
            get { return toTop; }
        }

        public AnimatedLocation(double fromLeft, double toLeft, double fromTop, double toTop)
        {
            this.fromLeft = fromLeft;
            this.toLeft = toLeft;
            this.fromTop = fromTop;
            this.toTop = toTop;
        }
    }
}
