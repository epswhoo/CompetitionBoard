using System.Globalization;
using System.Windows.Data;

namespace CompetitionBoard_Net8.WpfApp.Helper.Converters
{
    public class MarkFormatConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                string strValue = ((double)value).ToString("0.0");
                return strValue;
            }
            else
                return "0.0";
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0.0;
        }
    }
}
