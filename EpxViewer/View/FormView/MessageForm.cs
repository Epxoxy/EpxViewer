using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EpxViewer
{
    public partial class MessageForm : Form
    {
        public MessageForm()
        {
            InitializeComponent();/*
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.Selectable
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.SupportsTransparentBackColor,
                true);
            this.UpdateStyles();*/
            this.Load += OnMessageFormLoad;
        }

        public void updateMessage(string value)
        {
            MessageLabel.Text = value;
        }

        public void updateTitle(string value)
        {
            AlertLabel.Text = value;
        }

        #region Event handler

        private void OnMessageFormLoad(object sender, EventArgs e)
        {
            this.AlertLabel.Visible = false;
            OkBtn.Visible = false;
            CancelBtn.Visible = false;
            MessageLabel.Visible = false;
            FadeShow();
        }
        
        private Padding HoriPadding = new Padding(70, 50, 70, 50);
        private async void FadeShow(int interval = 1)
        {
            this.Padding = HoriPadding;
            while(Opacity < 1)
            {
                this.Opacity += 0.3;
            }
            //Object is not fully invisible. Fade it in
            while (HoriPadding.Left > 0)
            {
                await Task.Delay(interval);
                Padding = HoriPadding;
                HoriPadding.Left -= 8;
                HoriPadding.Right -= 8;
            }
            //Object is not fully invisible. Fade it in
            while (HoriPadding.Top > 0)
            {
                await Task.Delay(interval);
                Padding = HoriPadding;
                HoriPadding.Top -= 3;
                HoriPadding.Bottom -= 3;
            }
            OkBtn.Visible = true;
            CancelBtn.Visible = true;
            AlertLabel.Visible = true;
            MessageLabel.Visible = true;
        }
        
        private void OkBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void OkBtnMouseHover(object sender, EventArgs e)
        {
            OkBtn.Image = Properties.Resources.ok_hover;
        }

        private void OkBtnMouseLeave(object sender, EventArgs e)
        {
            OkBtn.Image = Properties.Resources.ok_normal;
        }

        private void CancelBtn_MouseHover(object sender, EventArgs e)
        {
            CancelBtn.Image = Properties.Resources.cancel_hover;
        }

        private void CancelBtn_MouseLeave(object sender, EventArgs e)
        {
            CancelBtn.Image = Properties.Resources.cancel_normal;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void MovePane_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }

        }

        #endregion

        private void RootPane_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, RootPane.ClientRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }
    }
}
