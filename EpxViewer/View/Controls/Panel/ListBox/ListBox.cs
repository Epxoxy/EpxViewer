using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EpxViewer.ListBox
{
    public class ListBox : System.Windows.Controls.ListBox
    {
        #region ScrollOffset

        public static readonly DependencyProperty ScrollOffsetProperty =
            DependencyProperty.Register("ScrollOffset", typeof(double), typeof(ListBox),
                new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnScrollOffsetChanged)));

        public double ScrollOffset
        {
            get { return (double)GetValue(ScrollOffsetProperty); }
            set { SetValue(ScrollOffsetProperty, value); }
        }

        private static void OnScrollOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ListBox listbox = obj as ListBox;
            listbox?.ScrollView();
        }
        
        private void ScrollView()
        {
            if (scrollviewer != null)
            {
                scrollviewer.ScrollToVerticalOffset(ScrollOffset);
            }
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            scrollviewer = TreeHelper.FindVisualChild<ScrollViewer>(this);
            if(scrollviewer != null)
            {
                scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
        }
        
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            this.ScrollIntoViewCentered(SelectedItem);
        }

        #endregion

        public ScrollViewer scrollviewer { get; private set; }
    }
}
