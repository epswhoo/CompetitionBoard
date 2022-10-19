
using System.Windows;

namespace CompetitionBoardWpfApp.Helper.Converters
{
    public class InvBoolToVisConverter : BooleanConverter<Visibility>
    {
        public InvBoolToVisConverter() :
            base(Visibility.Collapsed, Visibility.Visible)
        { }
    }
}
