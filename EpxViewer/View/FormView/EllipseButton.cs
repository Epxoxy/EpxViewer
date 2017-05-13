using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EpxViewer
{
    public class EllipseButton : System.Windows.Forms.Button
    {
        public EllipseButton() : base() { }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            Graphics g = pevent.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (Image != null)
            {
                int minlength = Width < Height ? Width : Height;
                int offsetX = (Width - minlength) / 2;
                int offsetY = (Height - minlength) / 2;
                g.DrawImage(Image, offsetX, offsetY, minlength, minlength);
            }
            g.Dispose();
        }
    }
}
