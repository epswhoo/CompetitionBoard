using CompetitionBoard_Net8.WpfApp.Helper.StandByModus;
using System.Windows;

namespace CompetitionBoard_Net8.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            StandBySvc.SuppressStandby();

            //Compose();

            //var window = new MainWindow();
            //window.Show();

        }
    }

}
