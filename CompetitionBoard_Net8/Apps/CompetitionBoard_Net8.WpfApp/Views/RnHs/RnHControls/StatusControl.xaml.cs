using CompetitionBoard_Net8.Models.Common;
using System.Windows.Controls;

namespace CompetitionBoard_Net8.WpfApp.Views.RnHs.RnHControls
{
    /// <summary>
    /// Interaction logic for StatusControl.xaml
    /// </summary>
    public partial class StatusControl : UserControl
    {
        public StatusControl()
        {
            InitializeComponent();
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(RnHStatus)).Cast<RnHStatus>();
        }
    }
}
