using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CompetitionBoard_Net8.WpfApp.Helper.Converters
{
    public class MarkVisConverter : IValueConverter
    {
        public virtual object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility vis;
            if (value is double && ((double)value < 0.001))
                vis = Visibility.Hidden;
            else
                vis = Visibility.Visible;
            return vis;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0.0;
        }
    }
}
