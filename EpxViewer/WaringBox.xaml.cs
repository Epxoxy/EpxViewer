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

namespace EpxViewer
{
    /// <summary>
    /// Interaction logic for WaringBox.xaml
    /// </summary>
    public partial class WaringBox : Window
    {
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(WaringBox), new PropertyMetadata(""));
        
        public WaringBox()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            boxroot.DataContext = this;
            //boxroot.PreviewMouseDown += boxrootPreviewMouseDown;
            closeTimer = new System.Windows.Threading.DispatcherTimer();
            closeTimer.Interval = TimeSpan.FromSeconds(5d);
            closeTimer.Tick += OnTimerTick;
            closeTimer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            closeTimer.Stop();
            closeTimer.Tick -= OnTimerTick;
            this.Close();
        }

        private System.Windows.Threading.DispatcherTimer closeTimer;
        private void boxrootPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            boxroot.PreviewMouseDown -= boxrootPreviewMouseDown;
            this.Close();
        }
    }
}
