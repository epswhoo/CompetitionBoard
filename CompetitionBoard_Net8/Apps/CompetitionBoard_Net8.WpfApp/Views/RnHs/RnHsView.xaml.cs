using System.Windows;
using System.Windows.Controls;
using CompetitionBoard_Net8.ViewModels.UI;

namespace CompetitionBoard_Net8.WpfApp.Views
{
    /// <summary>
    /// Interaction logic for TitleView.xaml
    /// </summary>
    public partial class RnHsView : UserControl
    {
        public RnHsView()
        {
            InitializeComponent();
        }

        private void RnHsControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetRowHeight();
        }

        private void SetRowHeight()
        {
            double rowHeight = GetRowHeight();
            RnHs1.RnHsDataGrid.RowHeight = rowHeight;
            RnHs2.RnHsDataGrid.RowHeight = rowHeight;
            RnHs3.RnHsDataGrid.RowHeight = rowHeight;
            RnHs4.RnHsDataGrid.RowHeight = rowHeight;
            RnHs5.RnHsDataGrid.RowHeight = rowHeight;
        }

        private double GetRowHeight()
        {
            IEnumerable<RnHViewModel>? rnhsDC = RnHs1.DataContext as IEnumerable<RnHViewModel>;
            if (rnhsDC == null)
            {
                return 0;
            }
            int rowCount = rnhsDC.Count();
            if (rowCount == 0)
            {
                return 0;
            }
            return RnHs1.ActualHeight / rowCount - 1;
        }

        private void RnHs1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetRowHeight();
        }
    }
}
