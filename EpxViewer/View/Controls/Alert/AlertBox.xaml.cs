using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EpxViewer
{
    /// <summary>
    /// Interaction logic for AlertBox.xaml
    /// </summary>
    public partial class AlertBox : Window
    {
        private Lazy<System.Windows.Threading.DispatcherTimer> closeTimer = new Lazy<System.Windows.Threading.DispatcherTimer>();
        private List<string> OKMenus { get; set; }
        public AlertResult Result { get; private set; }
        public int SelectedIndex { get; private set; } = -1;

        public AlertBox()
        {
            InitializeComponent();
            //this.WindowStartupLocation = WindowStartupLocation.Manual;
            layerCanvas.DataContext = this;
        }

        #region Properities
        
        public string AlertTitle
        {
            get { return (string)GetValue(AlertTitleProperty); }
            set { SetValue(AlertTitleProperty, value); }
        }
        public object AlertContent
        {
            get { return (object)GetValue(AlertContentProperty); }
            set { SetValue(AlertContentProperty, value); }
        }
        public bool AutoClose
        {
            get { return (bool)GetValue(AutoCloseProperty); }
            set { SetValue(AutoCloseProperty, value); }
        }
        public bool AddOkCanCel
        {
            get { return (bool)GetValue(AddOkCanCelProperty); }
            set { SetValue(AddOkCanCelProperty, value); }
        }

        public static readonly DependencyProperty AlertContentProperty =
            DependencyProperty.Register("AlertContent", typeof(object), typeof(AlertBox), new PropertyMetadata(null, OnAlertContentChanged));
        public static readonly DependencyProperty AlertTitleProperty =
            DependencyProperty.Register("AlertTitle", typeof(string), typeof(AlertBox), new PropertyMetadata("Alert", OnAlertTitleChanged));
        public static readonly DependencyProperty AutoCloseProperty =
            DependencyProperty.Register("AutoClose", typeof(bool), typeof(AlertBox), new PropertyMetadata(false, OnAutoCloseChanged));
        public static readonly DependencyProperty AddOkCanCelProperty =
            DependencyProperty.Register("AddOkCanCel", typeof(bool), typeof(AlertBox), new PropertyMetadata(true, OnAddOkCanCelChanged));

        private static void OnAlertContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AlertBox alertbox = d as AlertBox;
            if (alertbox != null) alertbox.setContent(e.NewValue);
        }
        private static void OnAlertTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AlertBox alertbox = d as AlertBox;
            if (alertbox != null) alertbox.setTitle((string)e.NewValue);
        }
        private static void OnAutoCloseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AlertBox alertbox = d as AlertBox;
            if (alertbox != null) alertbox.setAutoClose((bool)e.NewValue);
        }
        private static void OnAddOkCanCelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AlertBox alertbox = d as AlertBox;
            if (alertbox != null) alertbox.updateOkCalcelVisibility((bool)e.NewValue);
        }

        #endregion

        #region Private Method

        private void setTitle(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            PART_Title.Text = value;
        }

        private void setContent(object value)
        {
            if (value == null) return;
            PART_Content.Content = value;
        }

        #endregion

        #region Public Method

        public void setAutoClose(bool isAutoClose)
        {
            if (isAutoClose)
            {
                contentSViewer.PreviewMouseDown += onPreviewMouseDown;
                bottomCanvas.MouseDown += onPreviewMouseDown;
                closeTimer.Value.Interval = TimeSpan.FromSeconds(5d);
                closeTimer.Value.Tick += (o, arg0) =>
                {
                    closeTimer.Value.Stop();
                    contentSViewer.PreviewMouseDown -= onPreviewMouseDown;
                    bottomCanvas.MouseDown -= onPreviewMouseDown;
                    gotoExit();
                };
                closeTimer.Value.Start();
            }
            else
            {
                contentSViewer.PreviewMouseDown -= onPreviewMouseDown;
                bottomCanvas.MouseDown -= onPreviewMouseDown;
            }
        }

        public void updateOkCalcelVisibility(bool isVisibleNeed)
        {
            if (isVisibleNeed)
            {
                okBtn.Visibility = Visibility.Visible;
                cancelBtn.Visibility = Visibility.Visible;
                bottomCanvas.Height = 78;
            }
            else
            {
                okBtn.Visibility = Visibility.Collapsed;
                cancelBtn.Visibility = Visibility.Collapsed;
                bottomCanvas.Height = 68;
            }
        }

        public void updateOkMenu(List<string> menus)
        {
            okMenusItemControl.ItemsSource = menus;
            OKMenus = menus;
        }

        #endregion

        #region Event handelr

        private void onPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Result = AlertResult.Click;
            gotoExit();
            e.Handled = true;
        }

        private void okBtnClick(object sender, RoutedEventArgs e)
        {
            if(OKMenus != null && OKMenus.Count > 0)
            {
                okMenuPopup.IsOpen = true;
            }
            else
            {
                Result = AlertResult.OK;
                gotoExit();
            }
        }

        private void cancelBtnClick(object sender, RoutedEventArgs e)
        {
            Result = AlertResult.Cancel;
            gotoExit();
        }

        private void topPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void gotoExit()
        {
            var sb = FindResource("exitStoryboard") as System.Windows.Media.Animation.Storyboard;
            if (sb != null)
            {
                sb.Completed += storyboardComleted;
                sb.Begin();
            }
            else
            {
                storyboardComleted(this, EventArgs.Empty);
            }
        }

        private void storyboardComleted(object sender, EventArgs e)
        {
            this.Close();
        }

        private void okMenusItemControlSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedIndex = okMenusItemControl.SelectedIndex;
            Result = AlertResult.OK;
            okMenuPopup.IsOpen = false;
            gotoExit();
        }

        #endregion

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                okBtnClick(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }
    }
}
