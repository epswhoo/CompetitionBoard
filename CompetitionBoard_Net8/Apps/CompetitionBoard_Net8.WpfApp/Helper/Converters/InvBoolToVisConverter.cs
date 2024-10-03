
using System.Windows;

namespace CompetitionBoard_Net8.WpfApp.Helper.Converters
{
    public class InvBoolToVisConverter : BooleanConverter<Visibility>
    {
        public InvBoolToVisConverter() :
            base(Visibility.Collapsed, Visibility.Visible)
        { }
    }
}
