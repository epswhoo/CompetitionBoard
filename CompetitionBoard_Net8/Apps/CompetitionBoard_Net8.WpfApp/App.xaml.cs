using CompetitionBoard_Net8.Models.IDBSvc;
using CompetitionBoard_Net8.WpfApp.Configs;
using CompetitionBoard_Net8.WpfApp.Helper.StandByModus;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Windows;

namespace CompetitionBoard_Net8.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IConfiguration Config { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            StandBySvc.SuppressStandby();

        }


    }

}
