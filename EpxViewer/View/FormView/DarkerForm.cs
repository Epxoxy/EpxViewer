using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EpxViewer
{
    public partial class DarkerForm : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }

        public DarkerForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        #region Event handler

        private async void FadeIn(int interval = 1)
        {
            this.TransparencyKey = Color.Empty;
            Opacity = 0;

            //Object is not fully invisible. Fade it in
            while (Opacity < 0.5)
            {
                await Task.Delay(interval);
                Opacity += 0.01;
            }
        }

        private void onFormLoad(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = 200;
            timer.Start();
            timer.Tick += (arg1, arg2) =>
            {
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                FadeIn();
                timer.Stop();
                timer.Dispose();
            };
        }

        private void onClick(object sender, EventArgs e)
        {
            closeForm();
        }

        private void closeForm()
        {
            this.Close();
            this.Dispose();
        }
        
        bool isMouseDown;
        private void onMouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                System.Diagnostics.Debug.WriteLine("Mouse move");
            }
        }

        private void onMouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                System.Diagnostics.Debug.WriteLine("Mouse up");
                onClick(sender, e);
            }
        }

        #endregion
    }
}
