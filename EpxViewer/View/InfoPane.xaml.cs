using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace EpxViewer
{
    /// <summary>
    /// Interaction logic for InfoPane.xaml
    /// </summary>
    public partial class InfoPane : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(InfoPane), new PropertyMetadata("Message", OnTitleChangedCallBack));
        
        public object AlertContent
        {
            get { return (object)GetValue(AlertContentProperty); }
            set { SetValue(AlertContentProperty, value); }
        }
        
        public static readonly DependencyProperty AlertContentProperty =
            DependencyProperty.Register("AlertContent", typeof(object), typeof(InfoPane), new PropertyMetadata(null, OnAlertContentChangedCallBack));
        
        public object Description
        {
            get { return (object)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(object), typeof(InfoPane), new PropertyMetadata(null, OnDescriptionChangedCallBack));
        
        static InfoPane()
        {
            VisibilityProperty.OverrideMetadata(typeof(InfoPane), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnVisibilityChangedCallBack));
        }

        public InfoPane()
        {
            InitializeComponent();
        }
        
        private static void OnTitleChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InfoPane dp = d as InfoPane;
            if (dp != null) dp.updateTitle();
        }

        private static void OnAlertContentChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InfoPane dp = d as InfoPane;
            if (dp != null) dp.updateAlertContent();
        }

        private static void OnDescriptionChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InfoPane dp = d as InfoPane;
            if (dp != null) dp.updateDescription();
        }

        private static void OnVisibilityChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InfoPane dp = d as InfoPane;
            if (dp != null) dp.updateVisibilityState();
        }
        
        private void updateVisibilityState()
        {
            if (Visibility == Visibility.Visible)
            {
                VisualStateManager.GoToState(this, InfoPaneState.Visible, true);
                updateExpandState(true);
            }
            else
            {
                VisualStateManager.GoToState(this, InfoPaneState.Collapsed, true);
                VisualStateManager.GoToState(this, InfoPaneState.UnExpand, true);
            }
        }

        private async void updateExpandState(bool delay)
        {
            if (Visibility != Visibility.Visible) return;
            if (delay)  await Task.Delay(300);
            if (Description != null) VisualStateManager.GoToState(this, InfoPaneState.Expand, true);
            else VisualStateManager.GoToState(this, InfoPaneState.UnExpand, true);
        }

        private void updateAlertContent()
        {
            if (AlertContent == null) Visibility = Visibility.Collapsed;
            else
            {
                var newContent = AlertContent as FrameworkElement;
                if (newContent != null)
                {
                    PartCenterContent.Visibility = Visibility.Visible;
                    PartCenterTextBlock.Visibility = Visibility.Collapsed;
                    PartCenterContent.Content = AlertContent;
                }
                else
                {
                    PartCenterTextBlock.Visibility = Visibility.Visible;
                    PartCenterContent.Visibility = Visibility.Collapsed;
                    PartCenterTextBlock.Text = AlertContent.ToString();
                }
            }
        }

        private void updateDescription()
        {
            BottomContent.Content = Description;
            updateExpandState(false);
        }

        private void updateTitle()
        {
            PartTop.Text = Title;
        }
        
    }

    public class InfoPaneState
    {
        public const string Collapsed = "Collapsed";
        public const string Visible = "Visible";
        public const string Expand = "Expand";
        public const string UnExpand = "UnExpand";
    }
}
