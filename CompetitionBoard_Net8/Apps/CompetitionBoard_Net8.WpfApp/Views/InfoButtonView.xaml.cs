using CompetitionBoard_Net8.WpfApp.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CompetitionBoard_Net8.WpfApp.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class InfoButtonView : UserControl
    {
        public InfoButtonView()
        {
            InitializeComponent();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            DocuConfig docuConfig = new DocuConfig();
            config.GetSection("Docu").Bind(docuConfig);
            ProcessStartInfo processStartInfo = new ProcessStartInfo(docuConfig.BrowserPath, docuConfig.DocuPath);
            Process.Start(processStartInfo);
        }
    }
}
