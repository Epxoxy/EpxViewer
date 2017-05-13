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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EpxViewer
{

    /// <summary>
    /// Interaction logic for MessagePane.xaml
    /// </summary>
    public partial class MessagePane : UserControl
    {
        static MessagePane()
        {
            VisibilityProperty.OverrideMetadata(typeof(MessagePane), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnVisibilityChangedCallBack));
        }

        public MessagePane()
        {
            InitializeComponent();
            Visibility = Visibility.Visible;
        }
        private static void OnVisibilityChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessagePane m = d as MessagePane;
            if (m != null) m.updateVisibilityState();
        }
        private void updateVisibilityState()
        {
            if (Visibility == Visibility.Visible)
            {
                VisualStateManager.GoToState(this, VisibilityState.Visible, true);
            }
            else
            {
                VisualStateManager.GoToState(this, VisibilityState.Collapsed, true);
            }
        }
        
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
