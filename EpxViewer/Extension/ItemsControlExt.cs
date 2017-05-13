using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace EpxViewer.Extension
{
    //Smooth load extension for ItemsControl
    public class ItemsControlExt
    {
        public static bool GetIsInView(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsInViewProperty);
        }
        public static void SetIsInView(DependencyObject obj, bool value)
        {
            obj.SetValue(IsInViewProperty, value);
        }

        public static bool GetSmoothLoad(DependencyObject obj)
        {
            return (bool)obj.GetValue(SmoothLoadProperty);
        }
        public static void SetSmoothLoad(DependencyObject obj, bool value)
        {
            obj.SetValue(SmoothLoadProperty, value);
        }

        public static object GetApplyControl(DependencyObject obj)
        {
            return (object)obj.GetValue(ApplyControlProperty);
        }
        public static void SetApplyControl(DependencyObject obj, object value)
        {
            obj.SetValue(ApplyControlProperty, value);
        }


        public static readonly DependencyProperty IsInViewProperty =
            DependencyProperty.RegisterAttached("IsInView", typeof(bool), typeof(ItemsControlExt), new PropertyMetadata(false, OnIsInViewChangedCallback));
        public static readonly DependencyProperty SmoothLoadProperty =
            DependencyProperty.RegisterAttached("SmoothLoad", typeof(bool), typeof(ItemsControlExt), new PropertyMetadata(true));
        public static readonly DependencyProperty ApplyControlProperty =
            DependencyProperty.RegisterAttached("ApplyControl", typeof(object), typeof(ItemsControlExt), new PropertyMetadata(null));

        private static void OnIsInViewChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (GetSmoothLoad(d))
                {
                    var control = GetApplyControl(d) as ItemsControl;
                    if (control != null)
                    {
                        animationInView(control);
                        System.Diagnostics.Debug.WriteLine("animationInView");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Not animationInView, " + GetApplyControl(d).ToString());
                    }
                }
            }
        }
        public static void animationInView(ItemsControl itemscontrol)
        {
            var next = new Point();
            var items = itemscontrol.ItemContainerGenerator.Items;
            var countTime = 0.1d * items.Count;
            var easingFunc = new CubicEase() { EasingMode = EasingMode.EaseOut };
            var defDuration = TimeSpan.FromMilliseconds(200d);
            foreach (var item in items)
            {
                var container = itemscontrol.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                if (container != null)
                {
                    next.Y += container.RenderSize.Height;
                    TranslateTransform transform = container.RenderTransform as TranslateTransform;
                    if (transform == null)
                        container.RenderTransform = transform = new TranslateTransform();
                    transform.BeginAnimation(TranslateTransform.YProperty, null);

                    var desireY = next.Y;
                    transform.Y = -desireY;
                    var animation = new DoubleAnimation(0, defDuration)
                    {
                        BeginTime = TimeSpan.FromSeconds(countTime),
                        EasingFunction = easingFunc
                    };
                    transform.BeginAnimation(TranslateTransform.YProperty, animation);

                    countTime -= 0.1d;
                }
            }
        }
    }
}
