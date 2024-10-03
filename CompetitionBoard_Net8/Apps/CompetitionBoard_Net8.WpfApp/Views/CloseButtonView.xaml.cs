using System.Windows;
using System.Windows.Controls;

namespace CompetitionBoard_Net8.WpfApp.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CloseButtonView : UserControl
    {
        public CloseButtonView()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
