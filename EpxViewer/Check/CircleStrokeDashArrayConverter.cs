using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EpxViewer
{
    public class CircleStrokeDashArrayConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value is Shape);
            Shape shape = (Shape)value;
            double segNum = double.Parse(parameter.ToString());

            double offset = shape.StrokeDashOffset;
            double width = shape.Width;
            double thickness = shape.StrokeThickness;

            double visibleLen = offset * 2;
            double length = (width - thickness) * Math.PI / segNum / thickness;
            return new DoubleCollection(new[] { visibleLen, (length - visibleLen) });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
