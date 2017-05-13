using System;
using System.Windows;

namespace EpxViewer
{
    /// <summary>
    /// Alert Result
    /// </summary>
    public enum AlertResult
    {
        OK = 1,
        Cancel = 2,
        Click = 3,
    }

    public class Alert
    {

        #region Rewrite Alert
        
        /// <summary>
        /// Show AlertBox that will auto close
        /// </summary>
        /// <param name="content">Alert Content</param>
        public static void ShowOnly(object content)
        {
            AlertBox alertbox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox = new AlertBox();
            }));
            alertbox.Owner = Application.Current.Windows[0];
            alertbox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            alertbox.AlertContent = content;
            alertbox.AddOkCanCel = false;
            alertbox.AutoClose = true;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox.ShowDialog();
                alertbox.Closed += OnAlertBoxClosed;
            }));
        }
        /// <summary>
        /// Show AlertBox that will auto close
        /// </summary>
        /// <param name="title">Alert Title</param>
        /// <param name="content">Alert Content</param>
        public static void ShowOnly(string title, object content)
        {
            WaringBox alertbox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox = new WaringBox();
            }));
            alertbox.Owner = Application.Current.Windows[0];
            alertbox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            alertbox.Title = title;
            alertbox.Message = content.ToString();
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox.ShowDialog();
                alertbox.Closed += OnAlertBoxClosed;
            }));
        }

        private static void OnAlertBoxClosed(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Show AlertBox and get Ok/Cancel result
        /// </summary>
        /// <param name="content">Alert Content</param>
        /// <returns></returns>
        public static AlertResult Show(object content)
        {
            AlertBox alertbox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox = new AlertBox();
            }));
            alertbox.Owner = Application.Current.Windows[0];
            alertbox.AlertContent = content;
            alertbox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox.ShowDialog();
            }));
            return alertbox.Result;
        }
        /// <summary>
        /// Show AlertBox and get Ok/Cancel result
        /// </summary>
        /// <param name="title">Alert Title</param>
        /// <param name="content">Alert Content</param>
        /// <returns></returns>
        public static AlertResult Show(string title, object content)
        {
            AlertBox alertbox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox = new AlertBox();
            }));
            alertbox.Owner = Application.Current.Windows[0];
            alertbox.AlertTitle = title;
            alertbox.AlertContent = content;
            alertbox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox.ShowDialog();
            }));
            return alertbox.Result;
        }

        /// <summary>
        /// Show AlertBox with selection menu
        /// </summary>
        /// <param name="content">Alert Content</param>
        /// <param name="okMenus">When user click ok to show</param>
        /// <returns></returns>
        public static int Show(System.Collections.Generic.List<string> okMenus, object content)
        {
            AlertBox alertbox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox = new AlertBox();
            }));
            alertbox.Owner = Application.Current.Windows[0];
            alertbox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            alertbox.AlertContent = content;
            alertbox.updateOkMenu(okMenus);
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox.ShowDialog();
            }));
            return alertbox.SelectedIndex;
        }
        /// <summary>
        /// Show AlertBox with selection menu
        /// </summary>
        /// <param name="title">Alert Title</param>
        /// <param name="content">Alert Content</param>
        /// <param name="okMenus">When user click ok to show</param>
        /// <returns></returns>
        public static int Show(System.Collections.Generic.List<string> okMenus, object content, string title)
        {
            AlertBox alertbox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox = new AlertBox();
            }));
            alertbox.Owner = Application.Current.Windows[0];
            alertbox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            alertbox.AlertTitle = title;
            alertbox.AlertContent = content;
            alertbox.updateOkMenu(okMenus);
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                alertbox.ShowDialog();
            }));
            return alertbox.SelectedIndex;
        }
        
        #endregion
    }
}
