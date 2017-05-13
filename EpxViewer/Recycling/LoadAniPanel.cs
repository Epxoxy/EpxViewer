using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace EpxViewer
{
    class LoadAniPanel : System.Windows.Controls.Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var retSize = new Size();
            foreach(UIElement ui in InternalChildren)
            {
                ui.Measure(new Size(availableSize.Width, availableSize.Height));
                retSize.Height += ui.DesiredSize.Height;
                retSize.Width = Math.Max(retSize.Width, ui.DesiredSize.Width);
            }
            return retSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var next = new Point();
            foreach (UIElement ui in InternalChildren)
            {
                ui.Arrange(new Rect(new Point(0, next.Y), ui.DesiredSize));
                next.Y += ui.RenderSize.Height;
            }
            return finalSize;
        }
    }
}
