using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace EpxViewer
{
    /// <summary>
    /// Interaction logic for EllipseWindow.xaml
    /// </summary>
    public partial class FloatEllipse : Window, IDisposable
    {
        public FloatEllipse()
        {
            InitializeComponent();
            this.Loaded += onLoaded;
        }
        
        private void onLoaded(object sender, RoutedEventArgs e)
        {
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = SystemParameters.WorkArea.Width;
            this.Top = 0;
            configPopup();
            this.Hide();
            subscribe(Hook.GlobalEvents());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ExtPopup.SetIsInView(monitor2Popup, false);
            ExtPopup.SetIsInView(menuPanPopup, false);
            ExtPopup.SetIsInView(ellipsePopup, false);
            logPopupConfig();
            Logger.getLogger().save();
            Dispose();
            base.OnClosing(e);
        }

        #region MouseKeyHook

        private void updateMenuPanState()
        {
            if (menuPanPopup.IsOpen)
            {
                menuPan.HideCommand.Execute(null);
            }
            else
            {
                //Make popup visible
                ExtPopup.SetIsInView(menuPanPopup, true);
            }
        }

        private void subscribe(IKeyboardMouseEvents events)
        {
            kmEvents = events;
            kmEvents.MouseDragStartedExt += onKmMouseDragStarted;
            kmEvents.MouseDragFinishedExt += onKmMouseDragFinished;
        }

        private void unsubscribe()
        {
            if (kmEvents == null) return;
            kmEvents.MouseDragStarted -= onKmMouseDragStarted;
            kmEvents.MouseDragFinished -= onKmMouseDragFinished;
        }

        private void onKmMouseDragStarted(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            startPoint = new Point(e.X, e.Y);
            Debug.WriteLine(e.X - 644 + ", " + (e.Y - 175));
        }

        private void onKmMouseDragFinished(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!menuPanPopup.IsOpen)
            {
                double xoffset = Math.Abs(e.X - startPoint.X);
                double yoffset = Math.Abs(e.Y - startPoint.Y);
                if(xoffset < 100 && yoffset > 150 && ((xoffset / yoffset) > (1 / 3)))
                {
                    //644 -> Transform.X + Canvas.Left + Ellipse.Width
                    //276 -> MenuPan.Height - EllipseListBox.Height + Ellipse.Width + Fix(32)
                    ExtPopup.GetConfigs(menuPanPopup).XOffset = e.X - 644;
                    ExtPopup.GetConfigs(menuPanPopup).YOffset = e.Y - 276;
                    updateMenuPanState();
                }
            }
        }

        private Point startPoint;
        private IKeyboardMouseEvents kmEvents;
        #endregion

        #region Config Popup

        private void configPopup()
        {
            //Load config
            PopupConfigs epConfig, mpconfig, monitorConfig;
            getPopupConfig(out mpconfig, "mpconfig");
            getPopupConfig(out epConfig, "epConfig");
            getPopupConfig(out monitorConfig, "monitorConfig");
            ExtPopup.SetConfigs(ellipsePopup, epConfig);
            ExtPopup.SetConfigs(menuPanPopup, mpconfig);
            ExtPopup.SetConfigs(monitor2Popup, monitorConfig);
            
            menuPanPopup.Opened += OnMenuPaneOpened;
            menuPanPopup.Closed += OnMenuPaneClosed;
            ExtPopup.SetIsInView(ellipsePopup, true);
            ExtPopup.SetIsInView(monitor2Popup, true);
        }

        private void getPopupConfig(out PopupConfigs pConfig, string name)
        {
            //Get config from logger
            PopupConfigs config = null;
            object obj = Logger.getLogger().getLogOfObject(name);
            //Check data
            if (obj != null) config = obj as PopupConfigs;
            if (config != null) pConfig = config;
            else pConfig = new PopupConfigs();
        }

        private void logPopupConfig()
        {
            //Log value
            var monitorConfig = ExtPopup.GetConfigs(monitor2Popup);
            var menuconfig = ExtPopup.GetConfigs(menuPanPopup);
            var ellipseConfig = ExtPopup.GetConfigs(ellipsePopup);

            Logger.getLogger().logValue("mpconfig", menuconfig);
            Logger.getLogger().logValue("epConfig", ellipseConfig);
            Logger.getLogger().logValue("monitorConfig", monitorConfig);
            menuPan.saveMenu();
        }
        
        private void OnMenuPaneClosed(object sender, EventArgs e)
        {
            button.BeginAnimation(OpacityProperty, new DoubleAnimation(1, TimeSpan.FromSeconds(0.3d)) { BeginTime = TimeSpan.FromSeconds(0.2d) });
            subscribe(Hook.GlobalEvents());
        }

        private void OnMenuPaneOpened(object sender, EventArgs e)
        {
            button.BeginAnimation(OpacityProperty, new DoubleAnimation(0, TimeSpan.FromSeconds(0.3d)));
            unsubscribe();
        }

        #endregion
        
        #region EllipseBtn Drag&Click support

        private void onPreMouseLButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            e.Handled = true;
        }
        
        private void onPreMouseLButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                updateMenuPanState();
                isMouseDown = false;
            }
            e.Handled = true;
        }
        
        private void onPreMouseRButtonUp(object sender, MouseButtonEventArgs e)
        {
            rightMenuPopup.IsOpen = rightMenuPopup.IsOpen ? false : true;
            e.Handled = true;
        }

        private void onPreMouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                ExtPopup.dragPopupHandler();
                isMouseDown = false;
            }
            e.Handled = true;
        }

        bool isMouseDown;
        #endregion

        #region Click event

        private void shutdownClick(object sender, RoutedEventArgs e)
        {
            SpecialCommand.ExitAppCommand.Execute(null);
        }

        private void notebtnClick(object sender, RoutedEventArgs e)
        {
            removeableTest();
        }

        #endregion
        
        private void showWithBackForm()
        {
            Thread thread = new Thread(new ParameterizedThreadStart(o =>
            {
                var form = new DarkerForm();
                System.Windows.Forms.Application.Run(form);
            }));
            thread.IsBackground = true;
            thread.Start();
            System.Windows.Threading.DispatcherTimer dtimer = new System.Windows.Threading.DispatcherTimer();
            dtimer.Interval = TimeSpan.FromSeconds(2d);
            dtimer.Tick += (o, arg0) =>
            {
                System.Diagnostics.Debug.WriteLine("DispatcherTimer action");
                this.Activate();
                updateMenuPanState();
                dtimer.Stop();
            };
            dtimer.Start();
        }

        private void removeableTest()
        {
            Alert.Show("Message is hello!");
            Alert.ShowOnly("Title","Message is hello!");
            Debug.Write(Alert.Show( new List<string>() { "Menu0", "Menu1", "Menu2", "Menu3" }, "Selection tester."));
            Thread thread = new Thread(new ParameterizedThreadStart(o =>
            {
                var form = new MessageForm();
                System.Windows.Forms.Application.Run(form);
            }));
            thread.IsBackground = true;
            thread.Start();
        }
        
        private void OnSetTopChanged(object sender, RoutedEventArgs e)
        {
            ExtPopup.SetTopMost(monitor2Popup, !ExtPopup.GetTopMost(monitor2Popup));
        }
        
        private void OnStartMonitorChanged(object sender, RoutedEventArgs e)
        {
            monitor2.updateMonitorState();
        }

        private void OnLockChanged(object sender, RoutedEventArgs e)
        {
            ExtPopup.SetDraggable(monitor2, !ExtPopup.GetDraggable(monitor2));
        }

        private void RemoveItemClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(sender.ToString());
            //ExtPopup.SetIsInView(monitor2Popup, false);
        }

        public void Dispose()
        {
            unsubscribe();
            kmEvents.Dispose();
        }
    }
}
