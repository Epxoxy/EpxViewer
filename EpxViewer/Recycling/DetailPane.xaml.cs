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
    public class VisibilityState
    {
        public const string Collapsed = "Collapsed";
        public const string Visible = "Visible";
    }

    /// <summary>
    /// Interaction logic for DetailPane.xaml
    /// </summary>
    public partial class DetailPane : UserControl
    {
        static DetailPane()
        {
            VisibilityProperty.OverrideMetadata(typeof(DetailPane), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnVisibilityChangedCallBack));
        }

        private static void OnVisibilityChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DetailPane dp = d as DetailPane;
            if (dp != null) dp.updateVisibilityState();
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

        public DetailPane()
        {
            InitializeComponent();

            root.DataContext = this;
        }



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public object ContentDetail
        {
            get { return (object)GetValue(ContentDetailProperty); }
            set { SetValue(ContentDetailProperty, value); }
        }
        
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DetailPane), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ContentDetailProperty =
            DependencyProperty.Register("ContentDetail", typeof(object), typeof(DetailPane), new PropertyMetadata(null));
    }
}
