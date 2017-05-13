using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

namespace EpxViewer
{
    public class ExtPopup
    {
        //To control location before Popup's IsOpen property changed
        //Use this means you can't change Popup's IsOpen property by youself
        //Otherwise, location can't be remember
        public static bool GetIsInView(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsInViewProperty);
        }
        public static void SetIsInView(DependencyObject obj, bool value)
        {
            obj.SetValue(IsInViewProperty, value);
        }
        //Help log value that location changed when IsInView property changed
        public static PopupConfigs GetConfigs(DependencyObject obj)
        {
            return (PopupConfigs)obj.GetValue(ConfigsProperty);
        }
        public static void SetConfigs(DependencyObject obj, PopupConfigs value)
        {
            obj.SetValue(ConfigsProperty, value);
        }
        //Make Popup become dragable by listen 'PreviewMouseLeftButtonDown' Event
        public static bool GetDraggable(DependencyObject obj)
        {
            return (bool)obj.GetValue(DraggableProperty);
        }
        public static void SetDraggable(DependencyObject obj, bool value)
        {
            obj.SetValue(DraggableProperty, value);
        }
        //Make Popup TopMost or not
        public static bool GetTopMost(DependencyObject obj)
        {
            return (bool)obj.GetValue(TopMostProperty);
        }
        public static void SetTopMost(DependencyObject obj, bool value)
        {
            obj.SetValue(TopMostProperty, value);
        }
        
        public static readonly DependencyProperty IsInViewProperty =
            DependencyProperty.RegisterAttached("IsInView", typeof(bool), typeof(ExtPopup), new PropertyMetadata(false, OnIsInViewChanged));
        public static readonly DependencyProperty ConfigsProperty =
            DependencyProperty.RegisterAttached("Configs", typeof(PopupConfigs), typeof(ExtPopup), new PropertyMetadata(null));
        public static readonly DependencyProperty DraggableProperty =
            DependencyProperty.RegisterAttached("Draggable", typeof(bool), typeof(ExtPopup), new PropertyMetadata(false, OnDraggableChanged));
        public static readonly DependencyProperty TopMostProperty =
            DependencyProperty.RegisterAttached("TopMost", typeof(bool), typeof(ExtPopup), new PropertyMetadata(true, OnTopMostChanged));

        //Callback on 'Draggable' property changed
        private static void OnDraggableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as Control;
            if (element != null)
            {
                if ((bool)e.NewValue)
                {
                    element.PreviewMouseLeftButtonDown += onPreviewMouseLDown;
                }
                else
                {
                    element.PreviewMouseLeftButtonDown -= onPreviewMouseLDown;
                }
            }
        }
        //Callback on 'IsInView' property changed
        private static void OnIsInViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var popup = d as Popup;
            if (popup != null)
            {
                var config = GetConfigs(d);
                if ((bool)e.NewValue)
                {
                    if (!popup.IsOpen) popup.IsOpen = true;
                    //Set location
                    if (config == null) { config = new PopupConfigs(); SetConfigs(d, config); }
                    else
                    {
                        setAlwaysOnTop(popup, config.OnTop);
                        setPopupLocation(popup, (int)config.XOffset, (int)config.YOffset);
                    }
                }
                else
                {
                    //Log location
                    if (config == null) { config = new PopupConfigs(); SetConfigs(d, config); }
                    var point = getPopupLocation(popup);
                    config.XOffset = point.X;
                    config.YOffset = point.Y;
                    if (popup.IsOpen) popup.IsOpen = false;
                }
            }
        }
        //Callback on 'TopMost' property changed
        private static void OnTopMostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var popup = d as Popup;
            if (popup != null)
            {
                var config = GetConfigs(d);
                if (config == null)
                {
                    config = new PopupConfigs();
                    SetConfigs(d, config);
                }
                config.OnTop = (bool)e.NewValue;
                if(popup.IsOpen) setAlwaysOnTop(popup, config.OnTop);
            }
        }
        //'PreviewMouseLeftButtonDown' EventHandler
        private static void onPreviewMouseLDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                dragPopupHandler();
                e.Handled = true;
            }
            //Disable Left button Click
            //if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed) e.Handled = false;
            //else e.Handled = true;
        }

        #region Popup extension method

        public static void dragPopupHandler()
        {
            Win32API.POINT curPos;
            IntPtr hWndPopup;

            Win32API.GetCursorPos(out curPos);
            hWndPopup = Win32API.WindowFromPoint(curPos);

            Win32API.ReleaseCapture();
            Win32API.SendMessage(hWndPopup, Win32API.WM_NCLBUTTONDOWN, new IntPtr(Win32API.HT_CAPTION), IntPtr.Zero);
        }
        
        public static Point getPopupLocation(Popup pop)
        {
            HwndSource hwndSource = PresentationSource.FromVisual(pop.Child) as HwndSource;
            if (hwndSource == null) return new Point(0,0);

            IntPtr hwnd = hwndSource.Handle;
            Win32API.RECT rect = new Win32API.RECT();
            Win32API.GetWindowRect(hwnd, out rect);
            System.Diagnostics.Debug.WriteLine("Win32API " + rect.Left + ", " + rect.Top);
            return new Point(rect.Left, rect.Top);
        }

        public static void setPopupLocation(Popup pop, int x, int y)
        {
            HwndSource hwndSource = PresentationSource.FromVisual(pop.Child) as HwndSource;
            if (hwndSource == null) return;

            IntPtr hwnd = hwndSource.Handle;
            Win32API.SetWindowPos(hwnd, Win32API.HWND_TOPMOST, x, y, 0, 0, Win32API.SWP_NOSIZE);
            System.Diagnostics.Debug.WriteLine("Win32API " + x + ", " + y);
            getPopupLocation(pop);
        }

        public static void setAlwaysOnTop(Popup popup, bool alwayOnTop)
        {
            HwndSource hwndSource = PresentationSource.FromVisual(popup.Child) as HwndSource;
            if (hwndSource == null) return;

            IntPtr hwnd = hwndSource.Handle;
            Win32API.RECT rect;
            if (Win32API.GetWindowRect(hwnd, out rect))
            {
                Win32API.SetWindowPos(hwnd, alwayOnTop? Win32API.HWND_TOPMOST : Win32API.HWND_NOTOPMOST, rect.Left, rect.Top, (int)popup.Width, (int)popup.Height, 0);
            }
            /*Unknow different , part way2
            if (alwayOnTop)
            {
                Win32API.SetWindowPos(hwnd, Win32API.HWND_TOPMOST, rect.Left, rect.Top, (int)popup.Width, (int)popup.Height, Win32API.TOPMOST_FLAGS);
            }
            else
            {
                // Z-Order would only get refreshed/reflected if clicking the
                // the titlebar (as opposed to other parts of the external
                // window) unless I first set the popup to HWND_BOTTOM
                // then HWND_TOP before HWND_NOTOPMOST
                Win32API.SetWindowPos(hwnd, Win32API.HWND_BOTTOM, rect.Left, rect.Top, (int)popup.Width, (int)popup.Height, Win32API.TOPMOST_FLAGS);
                Win32API.SetWindowPos(hwnd, Win32API.HWND_TOP, rect.Left, rect.Top, (int)popup.Width, (int)popup.Height, Win32API.TOPMOST_FLAGS);
                Win32API.SetWindowPos(hwnd, Win32API.HWND_NOTOPMOST, rect.Left, rect.Top, (int)popup.Width, (int)popup.Height, Win32API.TOPMOST_FLAGS);
            }*/
        }

        #endregion

        /*
         
        private static void OnLocationChanged(UIElement obj)
        {
            var parent = GetParent(obj);
            if (parent != null)
            {
                double xOffset, yOffset;
                Point location = getPopupLocation(parent);
                xOffset = location.X + obj.DesiredSize.Width - SystemParameters.WorkArea.Width;
                yOffset = location.Y;
                //Point p3 = parent.Child.PointToScreen(new Point(obj.DesiredSize.Width, 0));
                //xOffset = p3.X - SystemParameters.WorkArea.Width;
                //yOffset = p3.Y;
                
                parent.HorizontalOffset = xOffset;
                parent.VerticalOffset = yOffset;
                System.Diagnostics.Debug.WriteLine("xOffset " + xOffset +", yOffset "+yOffset + ", Width"+ obj.DesiredSize.Width);
            }
        }
        
        private static void beginAnimation(Popup pop, double xOffset, double yOffset)
        {
            var time = TimeSpan.FromSeconds(0.2d);

            DoubleAnimation xAnimation = new DoubleAnimation();
            Storyboard.SetTarget(xAnimation, pop);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath(Popup.HorizontalOffsetProperty));
            xAnimation.To = xOffset;
            xAnimation.Duration = time;

            DoubleAnimation yAnimation = new DoubleAnimation();
            Storyboard.SetTarget(yAnimation, pop);
            Storyboard.SetTargetProperty(yAnimation, new PropertyPath(Popup.VerticalOffsetProperty));
            yAnimation.To = yOffset;
            yAnimation.Duration = time;

            Storyboard sb = new Storyboard();
            sb.Children.Add(xAnimation);
            sb.Children.Add(yAnimation);
            sb.Begin();
        }*/
    }

    [Serializable]
    public class PopupConfigs
    {
        public double XOffset { get; set; }
        public double YOffset { get; set; }
        public bool OnTop { get; set; }
        public bool Lock { get; set; }

        public PopupConfigs() { }

        public PopupConfigs(double x, double y, bool onTop)
        {
            XOffset = x;
            YOffset = y;
            OnTop = onTop;
        }
    }
}
